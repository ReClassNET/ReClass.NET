using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ReClassNET.Core
{
	internal class InternalCoreFunctions : NativeCoreWrapper
	{
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool DisassembleCodeDelegate(IntPtr address, IntPtr length, IntPtr virtualAddress, out InstructionData instruction);

		private delegate IntPtr InitializeInputDelegate();

		private delegate bool GetPressedKeysDelegate(IntPtr handle, out IntPtr pressedKeysArrayPtr, out int length);

		private delegate void ReleaseInputDelegate(IntPtr handle);

		private readonly DisassembleCodeDelegate disassembleCodeDelegate;

		private readonly InitializeInputDelegate initializeInputDelegate;
		private readonly GetPressedKeysDelegate getPressedKeysDelegate;
		private readonly ReleaseInputDelegate releaseInputDelegate;

		public InternalCoreFunctions(IntPtr handle)
			: base(handle)
		{
			disassembleCodeDelegate = GetFunctionDelegate<DisassembleCodeDelegate>(handle, "DisassembleCode");

			initializeInputDelegate = GetFunctionDelegate<InitializeInputDelegate>(handle, "InitializeInput");
			getPressedKeysDelegate = GetFunctionDelegate<GetPressedKeysDelegate>(handle, "GetPressedKeys");
			releaseInputDelegate = GetFunctionDelegate<ReleaseInputDelegate>(handle, "ReleaseInput");
		}

		public bool DisassembleCode(IntPtr address, int length, IntPtr virtualAddress, out InstructionData instruction)
		{
			return disassembleCodeDelegate(address, (IntPtr)length, virtualAddress, out instruction);
		}

		public IntPtr InitializeInput()
		{
			return initializeInputDelegate();
		}

		private static readonly Keys[] empty = new Keys[0];

		public Keys[] GetPressedKeys(IntPtr handle)
		{
			if (!getPressedKeysDelegate(handle, out var buffer, out var length) || length == 0)
			{
				return empty;
			}

			var keys = new int[length];
			Marshal.Copy(buffer, keys, 0, length);
			return (Keys[])(object)keys; // Yes, it's legal...
			//return Array.ConvertAll(keys, k => (Keys)k);
		}

		public void ReleaseInput(IntPtr handle)
		{
			releaseInputDelegate(handle);
		}
	}
}
