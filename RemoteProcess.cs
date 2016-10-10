using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET
{
	class RemoteProcess
	{
		private readonly NativeHelper nativeHelper;

		private ProcessInfo process;
		public ProcessInfo Process
		{
			get { return process; }
			set { if (process != value) { process = value; ProcessChanged?.Invoke(this); } }
		}

		public delegate void RemoteProcessChangedEvent(RemoteProcess sender);

		public event RemoteProcessChangedEvent ProcessChanged;

		public RemoteProcess(NativeHelper nativeHelper)
		{
			Contract.Requires(nativeHelper != null);

			this.nativeHelper = nativeHelper;
		}

		public void ReadMemoryIntoBuffer(IntPtr address, ref byte[] data)
		{
			if (Process == null || !nativeHelper.IsProcessValid(Process.Handle))
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
	}
}
