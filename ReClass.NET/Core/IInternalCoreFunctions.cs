using System;
using System.Windows.Forms;

namespace ReClassNET.Core
{
	public interface IInternalCoreFunctions
	{
		bool DisassembleCode(IntPtr address, int length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback);

		IntPtr InitializeInput();

		Keys[] GetPressedKeys(IntPtr handle);

		void ReleaseInput(IntPtr handle);
	}
}
