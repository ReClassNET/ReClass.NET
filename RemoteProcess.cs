using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET
{
	class RemoteProcess
	{
		private readonly NativeHelper nativeHelper;
		public NativeHelper NativeHelper => nativeHelper;

		private ProcessInfo process;
		public ProcessInfo Process
		{
			get { return process; }
			set { if (process != value) { process = value; ProcessChanged?.Invoke(this); } }
		}

		public delegate void RemoteProcessChangedEvent(RemoteProcess sender);
		public event RemoteProcessChangedEvent ProcessChanged;

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
			public Natives.StateEnum State;
			public Natives.AllocationProtectEnum Protection;
			public Natives.TypeEnum Type;
			public string ModulePath;
		}

		private readonly List<Module> modules = new List<Module>();
		public IEnumerable<Module> Modules => modules;

		private readonly List<Section> sections = new List<Section>();
		public IEnumerable<Section> Sections => sections;

		public bool IsValid => process != null && nativeHelper.IsProcessValid(process.Handle);

		public RemoteProcess(NativeHelper nativeHelper)
		{
			Contract.Requires(nativeHelper != null);

			this.nativeHelper = nativeHelper;
		}

		#region ReadMemory

		public void ReadMemoryIntoBuffer(IntPtr address, ref byte[] data)
		{
			if (!IsValid)
			{
				Process = null;

				data.FillWithZero();

				return;
			}

			nativeHelper.ReadRemoteMemory(Process.Handle, address, data, (uint)data.Length);
		}

		public byte[] ReadMemory(IntPtr address, int size)
		{
			var data = new byte[size];
			ReadMemoryIntoBuffer(address, ref data);
			return data;
		}

		private string ReadString(Encoding encoding, IntPtr address, int length)
		{
			var data = ReadMemory(address, length);
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

		public string ReadRawUTF8String(IntPtr address, int length)
		{
			var data = ReadMemory(address, length);
			if (data == null)
			{
				return null;
			}
			return Encoding.UTF8.GetString(data);
		}

		public string ReadUTF8String(IntPtr address, int length)
		{
			return ReadString(Encoding.UTF8, address, length);
		}

		public string ReadUTF16String(IntPtr address, int length)
		{
			return ReadString(Encoding.Unicode, address, length);
		}

		public string ReadUTF32String(IntPtr address, int length)
		{
			return ReadString(Encoding.UTF32, address, length);
		}

		#endregion

		#region WriteMemory

		public bool WriteRemoteMemory(IntPtr address, byte[] data)
		{
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
				return $"<{section.Category}>{section.Name}.{address.ToString("X")}";
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
				delegate (IntPtr baseAddress, IntPtr regionSize, string name, Natives.StateEnum state, Natives.AllocationProtectEnum protection, Natives.TypeEnum type, string modulePath)
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

		public IntPtr ParseAddress(string addressStr)
		{
			IntPtr finalAddress = IntPtr.Zero;

			if (addressStr.StartsWith("0x"))
			{
				addressStr = addressStr.Substring(2);
			}

			long address;
			if (long.TryParse(addressStr, NumberStyles.HexNumber, null, out address))
			{
#if WIN32
				return unchecked((IntPtr)(int)address);
#else
				return unchecked((IntPtr)address);
#endif
			}

			return finalAddress;
		}
	}
}
