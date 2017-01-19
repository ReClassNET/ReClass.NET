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
using ReClassNET.Native;
using ReClassNET.Symbols;
using ReClassNET.Util;

namespace ReClassNET.Memory
{
	public delegate void RemoteProcessEvent(RemoteProcess sender);

	public class RemoteProcess : IDisposable
	{
		private readonly object processSync = new object();

		private readonly CoreFunctionsManager coreFunctions;

		private readonly RemoteDebugger debugger;

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

		/// <summary>Reads remote memory from the address into the buffer.</summary>
		/// <param name="address">The address to read from.</param>
		/// <param name="buffer">[out] The data buffer to fill. If the remote process is not valid, the buffer will get filled with zeros.</param>
		public bool ReadRemoteMemoryIntoBuffer(IntPtr address, ref byte[] buffer)
		{
			Contract.Requires(buffer != null);
			Contract.Ensures(Contract.ValueAtReturn(out buffer) != null);

			return ReadRemoteMemoryIntoBuffer(address, ref buffer, 0, buffer.Length);
		}

		/// <summary>Reads remote memory from the address into the buffer.</summary>
		/// <param name="address">The address to read from.</param>
		/// <param name="buffer">[out] The data buffer to fill. If the remote process is not valid, the buffer will get filled with zeros.</param>
		/// <param name="offset">The offset in the data.</param>
		/// <param name="length">The number of bytes to read.</param>
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

		/// <summary>Reads <paramref name="size"/> bytes from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <param name="size">The size in bytes to read.</param>
		/// <returns>An array of bytes.</returns>
		public byte[] ReadRemoteMemory(IntPtr address, int size)
		{
			Contract.Requires(size >= 0);
			Contract.Ensures(Contract.Result<byte[]>() != null);

			var data = new byte[size];
			ReadRemoteMemoryIntoBuffer(address, ref data);
			return data;
		}

		/// <summary>Reads the object from the address in the remote process.</summary>
		/// <typeparam name="T">Type of the value to read.</typeparam>
		/// <param name="address">The address to read from.</param>
		/// <returns>The remote object.</returns>
		public T ReadRemoteObject<T>(IntPtr address) where T : struct
		{
			var data = ReadRemoteMemory(address, Marshal.SizeOf<T>());

			var gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var obj = (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
			gcHandle.Free();

			return obj;
		}

		/// <summary>Reads a string from the address in the remote process with the given length using the provided encoding.</summary>
		/// <param name="encoding">The encoding used by the string.</param>
		/// <param name="address">The address of the string.</param>
		/// <param name="length">The length of the string.</param>
		/// <returns>The string.</returns>
		public string ReadRemoteString(Encoding encoding, IntPtr address, int length)
		{
			Contract.Requires(encoding != null);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			var data = ReadRemoteMemory(address, length);

			try
			{
				var sb = new StringBuilder(encoding.GetString(data));
				for (var i = 0; i < sb.Length; ++i)
				{
					if (sb[i] == 0)
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

		/// <summary>Reads a string from the address in the remote process with the given length using UTF8 encoding. The string gets truncated at the first zero character.</summary>
		/// <param name="address">The address of the string.</param>
		/// <param name="length">The length of the string.</param>
		/// <returns>The string.</returns>
		public string ReadRemoteUTF8StringUntilFirstNullCharacter(IntPtr address, int length)
		{
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			var data = ReadRemoteMemory(address, length);

			int index = 0;
			for (; index < data.Length; ++index)
			{
				if (data[index] == 0)
				{
					break;
				}
			}

			try
			{
				return Encoding.UTF8.GetString(data, 0, Math.Min(index, data.Length));
			}
			catch
			{
				return string.Empty;
			}
		}

		/// <summary>Reads remote runtime type information for the given address from the remote process.</summary>
		/// <param name="address">The address.</param>
		/// <returns>A string containing the runtime type information or null if no information could get found.</returns>
		public string ReadRemoteRuntimeTypeInformation(IntPtr address)
		{
			if (address.MayBeValid())
			{
				string rtti;
				if (!rttiCache.TryGetValue(address, out rtti))
				{
					var objectLocatorPtr = ReadRemoteObject<IntPtr>(address - IntPtr.Size);
					if (objectLocatorPtr.MayBeValid())
					{
					
#if WIN64
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
			var classHierarchyDescriptorPtr = ReadRemoteObject<IntPtr>(address + 0x10);
			if (classHierarchyDescriptorPtr.MayBeValid())
			{
				var baseClassCount = ReadRemoteObject<int>(classHierarchyDescriptorPtr + 8);
				if (baseClassCount > 0 && baseClassCount < 25)
				{
					var baseClassArrayPtr = ReadRemoteObject<IntPtr>(classHierarchyDescriptorPtr + 0xC);
					if (baseClassArrayPtr.MayBeValid())
					{
						var sb = new StringBuilder();
						for (var i = 0; i < baseClassCount; ++i)
						{
							var baseClassDescriptorPtr = ReadRemoteObject<IntPtr>(baseClassArrayPtr + (4 * i));
							if (baseClassDescriptorPtr.MayBeValid())
							{
								var typeDescriptorPtr = ReadRemoteObject<IntPtr>(baseClassDescriptorPtr);
								if (typeDescriptorPtr.MayBeValid())
								{
									var name = ReadRemoteUTF8StringUntilFirstNullCharacter(typeDescriptorPtr + 0x0C, 60);
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
			int baseOffset = ReadRemoteObject<int>(address + 0x14);
			if (baseOffset != 0)
			{
				var baseAddress = address - baseOffset;

				var classHierarchyDescriptorOffset = ReadRemoteObject<int>(address + 0x10);
				if (classHierarchyDescriptorOffset != 0)
				{
					var classHierarchyDescriptorPtr = baseAddress + classHierarchyDescriptorOffset;

					var baseClassCount = ReadRemoteObject<int>(classHierarchyDescriptorPtr + 0x08);
					if (baseClassCount > 0 && baseClassCount < 25)
					{
						var baseClassArrayOffset = ReadRemoteObject<int>(classHierarchyDescriptorPtr + 0x0C);
						if (baseClassArrayOffset != 0)
						{
							var baseClassArrayPtr = baseAddress + baseClassArrayOffset;

							var sb = new StringBuilder();
							for (var i = 0; i < baseClassCount; ++i)
							{
								var baseClassDescriptorOffset = ReadRemoteObject<int>(baseClassArrayPtr + (4 * i));
								if (baseClassDescriptorOffset != 0)
								{
									var baseClassDescriptorPtr = baseAddress + baseClassDescriptorOffset;

									var typeDescriptorOffset = ReadRemoteObject<int>(baseClassDescriptorPtr);
									if (typeDescriptorOffset != 0)
									{
										var typeDescriptorPtr = baseAddress + typeDescriptorOffset;

										var name = ReadRemoteUTF8StringUntilFirstNullCharacter(typeDescriptorPtr + 0x14, 60);
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

		/// <summary>Writes the given <paramref name="data"/> to the <paramref name="address"/> in the remote process.</summary>
		/// <param name="address">The address to write to.</param>
		/// <param name="data">The data to write.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		public bool WriteRemoteMemory(IntPtr address, byte[] data)
		{
			Contract.Requires(data != null);

			if (!IsValid)
			{
				return false;
			}

			return coreFunctions.WriteRemoteMemory(handle, address, ref data, 0, data.Length);
		}

		/// <summary>Writes the given <paramref name="value"/> to the <paramref name="address"/> in the remote process.</summary>
		/// <typeparam name="T">Type of the value to write.</typeparam>
		/// <param name="address">The address to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
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
				return sections.BinaryFind(s => address.CompareToRange(s.Start, s.End));
			}
		}

		public Module GetModuleToPointer(IntPtr address)
		{
			lock (modules)
			{
				return modules.BinaryFind(m => address.CompareToRange(m.Start, m.End));
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
			var section = GetSectionToPointer(address);
			if (section != null)
			{
				if (section.Category == SectionCategory.CODE || section.Category == SectionCategory.DATA)
				{
					// Code and Data sections belong to a module.
					return $"<{section.Category}>{section.ModuleName}.{address.ToString("X")}";
				}
				else if (section.Category == SectionCategory.HEAP)
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

		public void EnumerateRemoteSectionsAndModules(Action<Section> callbackSection, Action<Module> callbackModule)
		{
			if (!IsValid)
			{
				return;
			}

			coreFunctions.EnumerateRemoteSectionsAndModules(handle, callbackSection, callbackModule);
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
				var newModules = new List<Module>();
				var newSections = new List<Section>();

				EnumerateRemoteSectionsAndModules(newSections.Add, newModules.Add);

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

			var reader = new TokenReader();
			var tokens = reader.Read(addressFormula);

			var astBuilder = new AstBuilder();
			var operation = astBuilder.Build(tokens);

			if (operation == null)
			{
				return IntPtr.Zero;
			}

			var interpreter = new Interpreter();
			return interpreter.Execute(operation, this);
		}

		/// <summary>Loads all symbols asynchronous.</summary>
		/// <param name="progress">The progress reporter is called for every module. Can be null.</param>
		/// <param name="token">The token used to cancel the task.</param>
		/// <returns>The task.</returns>
		public Task LoadAllSymbolsAsync(IProgress<Tuple<Module, IEnumerable<Module>>> progress, CancellationToken token)
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

						progress?.Report(Tuple.Create<Module, IEnumerable<Module>>(module, copy));

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
