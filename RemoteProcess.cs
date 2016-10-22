using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ReClassNET.AddressParser;
using ReClassNET.SymbolReader;

namespace ReClassNET
{
	public class RemoteProcess
	{
		private readonly NativeHelper nativeHelper;
		public NativeHelper NativeHelper => nativeHelper;

		private ProcessInfo process;
		public ProcessInfo Process
		{
			get { return process; }
			set { if (process != value) { process = value; rttiCache.Clear(); ProcessChanged?.Invoke(this); } }
		}

		public delegate void RemoteProcessChangedEvent(RemoteProcess sender);
		public event RemoteProcessChangedEvent ProcessChanged;

		private Dictionary<IntPtr, string> rttiCache = new Dictionary<IntPtr, string>();

		public class Module
		{
			public IntPtr Start;
			public IntPtr End;
			public string Name;
			public string Path;
		}

		public class Section
		{
			public IntPtr Start;
			public IntPtr End;
			public string Name;
			public string Category;
			public NativeMethods.StateEnum State;
			public NativeMethods.AllocationProtectEnum Protection;
			public NativeMethods.TypeEnum Type;
			public string ModuleName;
			public string ModulePath;
		}

		private readonly List<Module> modules = new List<Module>();
		public IEnumerable<Module> Modules => modules;

		private readonly List<Section> sections = new List<Section>();
		public IEnumerable<Section> Sections => sections;

		private readonly Symbols symbols = new Symbols();
		public Symbols Symbols => symbols;

		public bool IsValid => process != null && nativeHelper.IsProcessValid(process.Handle);

		public RemoteProcess(NativeHelper nativeHelper)
		{
			Contract.Requires(nativeHelper != null);

			this.nativeHelper = nativeHelper;
		}

		#region ReadMemory

		public void ReadRemoteMemoryIntoBuffer(IntPtr address, ref byte[] data)
		{
			if (!IsValid)
			{
				Process = null;

				data.FillWithZero();

				return;
			}

			nativeHelper.ReadRemoteMemory(Process.Handle, address, data, (uint)data.Length);
		}

		public byte[] ReadRemoteMemory(IntPtr address, int size)
		{
			var data = new byte[size];
			ReadRemoteMemoryIntoBuffer(address, ref data);
			return data;
		}

		public T ReadRemoteObject<T>(IntPtr address)
		{
			var data = ReadRemoteMemory(address, Marshal.SizeOf<T>());

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var obj = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();

			return obj;
		}

		public string ReadRemoteString(Encoding encoding, IntPtr address, int length)
		{
			var data = ReadRemoteMemory(address, length);
			if (data == null)
			{
				return null;
			}

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

		public string ReadRemoteRawUTF8String(IntPtr address, int length)
		{
			var data = ReadRemoteMemory(address, length);
			if (data == null)
			{
				return null;
			}

			int index = -1;
			for (index = 0; index < data.Length; ++index)
			{
				if (data[index] == 0)
				{
					break;
				}
			}

			return Encoding.UTF8.GetString(data, 0, Math.Min(index, data.Length));
		}

		public string ReadRemoteRuntimeTypeInformation(IntPtr address)
		{
			if (address.MayBeValid())
			{
				string rtti = null;
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
									var name = ReadRemoteRawUTF8String(typeDescriptorPtr + 0x0C, 60);
									if (name.EndsWith("@@"))
									{
										name = NativeMethods.UnDecorateSymbolName("?" + name);
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

										var name = ReadRemoteRawUTF8String(typeDescriptorPtr + 0x14, 60);
										if (name.EndsWith("@@"))
										{
											name = NativeMethods.UnDecorateSymbolName("?" + name);
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

			return nativeHelper.WriteRemoteMemory(Process.Handle, address, data, (uint)data.Length);
		}

		public bool WriteRemoteMemory<T>(IntPtr address, T value) where T : struct
		{
			var data = new byte[Marshal.SizeOf<T>()];

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			Marshal.StructureToPtr(value, handle.AddrOfPinnedObject(), false);
			handle.Free();

			return WriteRemoteMemory(address, data);
		}

		#endregion

		public string GetNamedAddress(IntPtr address)
		{
			var section = sections.Where(s => s.Category != null).Where(s => address.InRange(s.Start, s.End)).FirstOrDefault();
			if (section != null)
			{
				return $"<{section.Category}>{section.ModuleName}.{address.ToString("X")}";
			}
			var module = modules.Where(m => address.InRange(m.Start, m.End)).FirstOrDefault();
			if (module != null)
			{
				return $"{module.Name}.{address.ToString("X")}";
			}
			return null;
		}

		public void UpdateProcessInformations()
		{
			modules.Clear();
			sections.Clear();

			if (!IsValid)
			{
				return;
			}

			nativeHelper.EnumerateRemoteSectionsAndModules(
				process.Handle,
				delegate (IntPtr baseAddress, IntPtr regionSize, string name, NativeMethods.StateEnum state, NativeMethods.AllocationProtectEnum protection, NativeMethods.TypeEnum type, string modulePath)
				{
					var section = new Section
					{
						Start = baseAddress,
						End = baseAddress.Add(regionSize),
						Name = name,
						State = state,
						Protection = protection,
						Type = type,
						ModulePath = modulePath,
						ModuleName = Path.GetFileName(modulePath)
					};
					switch (section.Name)
					{
						case ".text":
						case "code":
							section.Category = "CODE";
							break;
						case ".data":
						case "data":
						case ".rdata":
						case ".idata":
							section.Category = "DATA";
							break;
					}
					sections.Add(section);
				},
				delegate (IntPtr baseAddress, IntPtr regionSize, string modulePath)
				{
					modules.Add(new Module
					{
						Start = baseAddress,
						End = baseAddress.Add(regionSize),
						Path = modulePath,
						Name = Path.GetFileName(modulePath)
					});
				}
			);
		}

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

		public void LoadAllSymbols()
		{
			LoadAllSymbolsAsync(null).Wait();
		}

		public delegate void LoadModuleSymbols(Module module);
		public Task LoadAllSymbolsAsync(LoadModuleSymbols callback)
		{
			var copy = this.modules.ToList();

			return Task.Run(() =>
			{
				foreach (var module in copy)
				{
					try
					{
						callback?.Invoke(module);

						Symbols.LoadSymbolsForModule(module);
					}
					catch
					{
						//ignore
					}
				}
			});
		}
	}
}
