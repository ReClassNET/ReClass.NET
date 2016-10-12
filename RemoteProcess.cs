using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
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
				if (char.IsControl(sb[i]))
				{
					sb[i] = '.';
				}
			}
			return sb.ToString();
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

		public IntPtr ParseAddress(string addressStr)
		{


			long address;
			if (long.TryParse(addressStr, NumberStyles.HexNumber, null, out address))
			{
#if WIN32
				return unchecked((IntPtr)(int)address);
#else
				return unchecked((IntPtr)address);
#endif
			}

			return IntPtr.Zero;
		}
	}
}
