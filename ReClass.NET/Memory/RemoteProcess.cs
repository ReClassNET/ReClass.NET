using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReClassNET.AddressParser;
using ReClassNET.Core;
using ReClassNET.Debugger;
using ReClassNET.Extensions;
using ReClassNET.MemoryScanner;
using ReClassNET.Native;
using ReClassNET.Symbols;

namespace ReClassNET.Memory
{
	public delegate void RemoteProcessEvent(RemoteProcess sender);

	public class RemoteProcess : IDisposable, IRemoteMemoryReader, IRemoteMemoryWriter, IProcessReader
	{
		private readonly object processSync = new object();

		private readonly CoreFunctionsManager coreFunctions;

		private readonly RemoteDebugger debugger;

		private readonly Dictionary<string, Func<RemoteProcess, IntPtr>> formulaCache = new Dictionary<string, Func<RemoteProcess, IntPtr>>();

		private readonly Dictionary<IntPtr, string> rttiCache = new Dictionary<IntPtr, string>();

		private readonly List<Module> modules = new List<Module>();

		private readonly List<Section> sections = new List<Section>();

		private readonly SymbolStore symbols = new SymbolStore();

		private ProcessInfo process;
		private IntPtr handle;

		/// <summary>Event which gets invoked when a process was opened.</summary>
		public event RemoteProcessEvent ProcessAttached;

		/// <summary>Event which gets invoked before a process gets closed.</summary>
		public event RemoteProcessEvent ProcessClosing;

		/// <summary>Event which gets invoked after a process was closed.</summary>
		public event RemoteProcessEvent ProcessClosed;

		public CoreFunctionsManager CoreFunctions => coreFunctions;

		public RemoteDebugger Debugger => debugger;

		public ProcessInfo UnderlayingProcess => process;

		public SymbolStore Symbols => symbols;

		/// <summary>Gets a copy of the current modules list. This list may change if the remote process (un)loads a module.</summary>
		public IEnumerable<Module> Modules
		{
			get
			{
				List<Module> cpy;
				lock (modules)
				{
					cpy = modules.ToList();
				}
				return cpy;
			}
		}

		/// <summary>Gets a copy of the current sections list. This list may change if the remote process (un)loads a section.</summary>
		public IEnumerable<Section> Sections
		{
			get
			{
				List<Section> cpy;
				lock (sections)
				{
					cpy = sections.ToList();
				}
				return cpy;
			}
		}

		/// <summary>A map of named addresses.</summary>
		public Dictionary<IntPtr, string> NamedAddresses { get; } = new Dictionary<IntPtr, string>();

		public bool IsValid => process != null && coreFunctions.IsProcessValid(handle);

		public RemoteProcess(CoreFunctionsManager coreFunctions)
		{
			Contract.Requires(coreFunctions != null);

			this.coreFunctions = coreFunctions;

			debugger = new RemoteDebugger(this);
		}

		public void Dispose()
		{
			Close();
		}

		/// <summary>Opens the given process to gather informations from.</summary>
		/// <param name="info">The process information.</param>
		public void Open(ProcessInfo info)
		{
			Contract.Requires(info != null);

			if (process != info)
			{
				lock (processSync)
				{
					Close();

					rttiCache.Clear();

					process = info;

					handle = coreFunctions.OpenRemoteProcess(process.Id, ProcessAccess.Full);
				}

				ProcessAttached?.Invoke(this);
			}
		}

		/// <summary>Closes the underlaying process. If the debugger is attached, it will automaticly detached.</summary>
		public void Close()
		{
			if (process != null)
			{
				ProcessClosing?.Invoke(this);

				lock (processSync)
				{
					debugger.Terminate();

					coreFunctions.CloseRemoteProcess(handle);

					handle = IntPtr.Zero;

					process = null;
				}

				ProcessClosed?.Invoke(this);
			}
		}

		#region ReadMemory

		public bool ReadRemoteMemoryIntoBuffer(IntPtr address, ref byte[] buffer)
		{
			Contract.Requires(buffer != null);
			Contract.Ensures(Contract.ValueAtReturn(out buffer) != null);

			return ReadRemoteMemoryIntoBuffer(address, ref buffer, 0, buffer.Length);
		}

		public bool ReadRemoteMemoryIntoBuffer(IntPtr address, ref byte[] buffer, int offset, int length)
		{
			Contract.Requires(buffer != null);
			Contract.Requires(offset >= 0);
			Contract.Requires(length >= 0);
			Contract.Requires(offset + length <= buffer.Length);
			Contract.Ensures(Contract.ValueAtReturn(out buffer) != null);
			Contract.EndContractBlock();

			if (!IsValid)
			{
				Close();

				buffer.FillWithZero();

				return false;
			}

			return coreFunctions.ReadRemoteMemory(handle, address, ref buffer, offset, length);
		}

		public byte[] ReadRemoteMemory(IntPtr address, int size)
		{
			Contract.Requires(size >= 0);
			Contract.Ensures(Contract.Result<byte[]>() != null);

			var data = new byte[size];
			ReadRemoteMemoryIntoBuffer(address, ref data);
			return data;
		}

		public T ReadRemoteObject<T>(IntPtr address) where T : struct
		{
			var data = ReadRemoteMemory(address, Marshal.SizeOf<T>());

			var gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var obj = (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
			gcHandle.Free();

			return obj;
		}

		#region Read Remote Primitive Types

		public sbyte ReadRemoteInt8(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(sbyte));

			return (sbyte)data[0];
		}

		public byte ReadRemoteUInt8(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(byte));

			return data[0];
		}

		public short ReadRemoteInt16(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(short));

			return BitConverter.ToInt16(data, 0);
		}

		public ushort ReadRemoteUInt16(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(ushort));

			return BitConverter.ToUInt16(data, 0);
		}

		public int ReadRemoteInt32(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(int));

			return BitConverter.ToInt32(data, 0);
		}

		public uint ReadRemoteUInt32(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(uint));

			return BitConverter.ToUInt32(data, 0);
		}

		public long ReadRemoteInt64(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(long));

			return BitConverter.ToInt64(data, 0);
		}

		public ulong ReadRemoteUInt64(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(ulong));

			return BitConverter.ToUInt64(data, 0);
		}

		public float ReadRemoteFloat(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(float));

			return BitConverter.ToSingle(data, 0);
		}

		public double ReadRemoteDouble(IntPtr address)
		{
			var data = ReadRemoteMemory(address, sizeof(double));

			return BitConverter.ToDouble(data, 0);
		}

		public IntPtr ReadRemoteIntPtr(IntPtr address)
		{
#if RECLASSNET64
			return (IntPtr)ReadRemoteInt64(address);
#else
			return (IntPtr)ReadRemoteInt32(address);
#endif
		}

		#endregion

		public string ReadRemoteString(Encoding encoding, IntPtr address, int length)
		{
			Contract.Requires(encoding != null);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			var data = ReadRemoteMemory(address, length * encoding.GuessByteCountPerChar());

			try
			{
				var sb = new StringBuilder(encoding.GetString(data));
				for (var i = 0; i < sb.Length; ++i)
				{
					if (sb[i] == '\0')
					{
						sb.Length = i;
						break;
					}
					if (!sb[i].IsPrintable())
					{
						sb[i] = '.';
					}
				}
				return sb.ToString();
			}
			catch
			{
				return string.Empty;
			}
		}

		public string ReadRemoteStringUntilFirstNullCharacter(Encoding encoding, IntPtr address, int length)
		{
			Contract.Requires(encoding != null);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			var data = ReadRemoteMemory(address, length * encoding.GuessByteCountPerChar());

			// TODO We should cache the pattern per encoding.
			var index = PatternScanner.FindPattern(BytePattern.From(new byte[encoding.GuessByteCountPerChar()]), data);
			if (index == -1)
			{
				index = data.Length;
			}

			try
			{
				return encoding.GetString(data, 0, Math.Min(index, data.Length));
			}
			catch
			{
				return string.Empty;
			}
		}

		public string ReadRemoteRuntimeTypeInformation(IntPtr address)
		{
			if (address.MayBeValid())
			{
				if (!rttiCache.TryGetValue(address, out var rtti))
				{
					var objectLocatorPtr = ReadRemoteIntPtr(address - IntPtr.Size);
					if (objectLocatorPtr.MayBeValid())
					{

#if RECLASSNET64
						rtti = ReadRemoteRuntimeTypeInformation64(objectLocatorPtr);
#else
						rtti = ReadRemoteRuntimeTypeInformation32(objectLocatorPtr);
#endif

						rttiCache[address] = rtti;
					}
				}
				return rtti;
			}

			return null;
		}

		private string ReadRemoteRuntimeTypeInformation32(IntPtr address)
		{
			var classHierarchyDescriptorPtr = ReadRemoteIntPtr(address + 0x10);
			if (classHierarchyDescriptorPtr.MayBeValid())
			{
				var baseClassCount = ReadRemoteInt32(classHierarchyDescriptorPtr + 8);
				if (baseClassCount > 0 && baseClassCount < 25)
				{
					var baseClassArrayPtr = ReadRemoteIntPtr(classHierarchyDescriptorPtr + 0xC);
					if (baseClassArrayPtr.MayBeValid())
					{
						var sb = new StringBuilder();
						for (var i = 0; i < baseClassCount; ++i)
						{
							var baseClassDescriptorPtr = ReadRemoteIntPtr(baseClassArrayPtr + (4 * i));
							if (baseClassDescriptorPtr.MayBeValid())
							{
								var typeDescriptorPtr = ReadRemoteIntPtr(baseClassDescriptorPtr);
								if (typeDescriptorPtr.MayBeValid())
								{
									var name = ReadRemoteStringUntilFirstNullCharacter(Encoding.UTF8, typeDescriptorPtr + 0x0C, 60);
									if (name.EndsWith("@@"))
									{
										name = NativeMethods.UndecorateSymbolName("?" + name);
									}

									sb.Append(name);
									sb.Append(" : ");

									continue;
								}
							}

							break;
						}

						if (sb.Length != 0)
						{
							sb.Length -= 3;

							return sb.ToString();
						}
					}
				}
			}

			return null;
		}

		private string ReadRemoteRuntimeTypeInformation64(IntPtr address)
		{
			int baseOffset = ReadRemoteInt32(address + 0x14);
			if (baseOffset != 0)
			{
				var baseAddress = address - baseOffset;

				var classHierarchyDescriptorOffset = ReadRemoteInt32(address + 0x10);
				if (classHierarchyDescriptorOffset != 0)
				{
					var classHierarchyDescriptorPtr = baseAddress + classHierarchyDescriptorOffset;

					var baseClassCount = ReadRemoteInt32(classHierarchyDescriptorPtr + 0x08);
					if (baseClassCount > 0 && baseClassCount < 25)
					{
						var baseClassArrayOffset = ReadRemoteInt32(classHierarchyDescriptorPtr + 0x0C);
						if (baseClassArrayOffset != 0)
						{
							var baseClassArrayPtr = baseAddress + baseClassArrayOffset;

							var sb = new StringBuilder();
							for (var i = 0; i < baseClassCount; ++i)
							{
								var baseClassDescriptorOffset = ReadRemoteInt32(baseClassArrayPtr + (4 * i));
								if (baseClassDescriptorOffset != 0)
								{
									var baseClassDescriptorPtr = baseAddress + baseClassDescriptorOffset;

									var typeDescriptorOffset = ReadRemoteInt32(baseClassDescriptorPtr);
									if (typeDescriptorOffset != 0)
									{
										var typeDescriptorPtr = baseAddress + typeDescriptorOffset;

										var name = ReadRemoteStringUntilFirstNullCharacter(Encoding.UTF8, typeDescriptorPtr + 0x14, 60);
										if (string.IsNullOrEmpty(name))
										{
											break;
										}

										if (name.EndsWith("@@"))
										{
											name = NativeMethods.UndecorateSymbolName("?" + name);
										}

										sb.Append(name);
										sb.Append(" : ");

										continue;
									}
								}

								break;
							}

							if (sb.Length != 0)
							{
								sb.Length -= 3;

								return sb.ToString();
							}
						}
					}
				}
			}

			return null;
		}

		#endregion

		#region WriteMemory

		public bool WriteRemoteMemory(IntPtr address, byte[] data)
		{
			Contract.Requires(data != null);

			if (!IsValid)
			{
				return false;
			}

			return coreFunctions.WriteRemoteMemory(handle, address, ref data, 0, data.Length);
		}

		public bool WriteRemoteMemory<T>(IntPtr address, T value) where T : struct
		{
			var data = new byte[Marshal.SizeOf<T>()];

			var gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
			Marshal.StructureToPtr(value, gcHandle.AddrOfPinnedObject(), false);
			gcHandle.Free();

			return WriteRemoteMemory(address, data);
		}

		#endregion

		public Section GetSectionToPointer(IntPtr address)
		{
			lock (sections)
			{
				var index = sections.BinarySearch(s => address.CompareToRange(s.Start, s.End));
				return index < 0 ? null : sections[index];
			}
		}

		public Module GetModuleToPointer(IntPtr address)
		{
			lock (modules)
			{
				var index = modules.BinarySearch(m => address.CompareToRange(m.Start, m.End));
				return index < 0 ? null : modules[index];
			}
		}

		public Module GetModuleByName(string name)
		{
			lock (modules)
			{
				return modules
					.FirstOrDefault(m => m.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
			}
		}

		/// <summary>Tries to map the given address to a section or a module of the process.</summary>
		/// <param name="address">The address to map.</param>
		/// <returns>The named address or null if no mapping exists.</returns>
		public string GetNamedAddress(IntPtr address)
		{
			if (NamedAddresses.TryGetValue(address, out var namedAddress))
			{
				return namedAddress;
			}

			var section = GetSectionToPointer(address);
			if (section != null)
			{
				if (section.Category == SectionCategory.CODE || section.Category == SectionCategory.DATA)
				{
					// Code and Data sections belong to a module.
					return $"<{section.Category}>{section.ModuleName}.{address.ToString("X")}";
				}
				if (section.Category == SectionCategory.HEAP)
				{
					return $"<HEAP>{address.ToString("X")}";
				}
			}
			var module = GetModuleToPointer(address);
			if (module != null)
			{
				return $"{module.Name}.{address.ToString("X")}";
			}
			return null;
		}

		public bool EnumerateRemoteSectionsAndModules(out List<Section> sections, out List<Module> modules)
		{
			if (!IsValid)
			{
				sections = null;
				modules = null;

				return false;
			}

			sections = new List<Section>();
			modules = new List<Module>();

			coreFunctions.EnumerateRemoteSectionsAndModules(handle, sections.Add, modules.Add);

			return true;
		}

		/// <summary>Updates the process informations.</summary>
		public void UpdateProcessInformations()
		{
			UpdateProcessInformationsAsync().Wait();
		}

		/// <summary>Updates the process informations asynchronous.</summary>
		/// <returns>The Task.</returns>
		public Task UpdateProcessInformationsAsync()
		{
			Contract.Ensures(Contract.Result<Task>() != null);

			if (!IsValid)
			{
				lock(modules)
				{
					modules.Clear();
				}
				lock(sections)
				{
					sections.Clear();
				}

				// TODO: Mono doesn't support Task.CompletedTask at the moment.
				//return Task.CompletedTask;
				return Task.FromResult(true);
			}

			return Task.Run(() =>
			{
				EnumerateRemoteSectionsAndModules(out var newSections, out var newModules);

				newModules.Sort((m1, m2) => m1.Start.CompareTo(m2.Start));
				newSections.Sort((s1, s2) => s1.Start.CompareTo(s2.Start));

				lock (modules)
				{
					modules.Clear();
					modules.AddRange(newModules);
				}
				lock (sections)
				{
					sections.Clear();
					sections.AddRange(newSections);
				}
			});
		}

		/// <summary>Parse the address formula.</summary>
		/// <param name="addressFormula">The address formula.</param>
		/// <returns>The result of the parsed address or <see cref="IntPtr.Zero"/>.</returns>
		public IntPtr ParseAddress(string addressFormula)
		{
			Contract.Requires(addressFormula != null);

			if (!formulaCache.TryGetValue(addressFormula, out var func))
			{
				var expression = Parser.Parse(addressFormula);

				func = DynamicCompiler.CompileExpression(expression);

				formulaCache.Add(addressFormula, func);
			}

			return func(this);
		}

		/// <summary>Loads all symbols asynchronous.</summary>
		/// <param name="progress">The progress reporter is called for every module. Can be null.</param>
		/// <param name="token">The token used to cancel the task.</param>
		/// <returns>The task.</returns>
		public Task LoadAllSymbolsAsync(IProgress<Tuple<Module, IReadOnlyList<Module>>> progress, CancellationToken token)
		{
			List<Module> copy;
			lock (modules)
			{
				copy = modules.ToList();
			}

			// Try to resolve all symbols in a background thread. This can take a long time because symbols are downloaded from the internet.
			// The COM objects can only be used in the thread they were created so we can't use them.
			// Thats why an other task loads the real symbols afterwards in the UI thread context.
			return Task.Run(
				() =>
				{
					foreach (var module in copy)
					{
						token.ThrowIfCancellationRequested();

						progress?.Report(Tuple.Create<Module, IReadOnlyList<Module>>(module, copy));

						Symbols.TryResolveSymbolsForModule(module);
					}
				},
				token
			)
			.ContinueWith(
				_ =>
				{
					foreach (var module in copy)
					{
						token.ThrowIfCancellationRequested();

						try
						{
							Symbols.LoadSymbolsForModule(module);
						}
						catch
						{
							//ignore
						}
					}
				},
				token,
				TaskContinuationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext()
			);
		}

		public void ControlRemoteProcess(ControlRemoteProcessAction action)
		{
			if (!IsValid)
			{
				return;
			}

			coreFunctions.ControlRemoteProcess(handle, action);
		}
	}
}
