using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;

namespace ReClassNET.Input
{
	public class KeyboardInput : IDisposable
	{
		private readonly IntPtr handle;

		public KeyboardInput()
		{
			handle = Program.CoreFunctions.InitializeInput();
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();

			GC.SuppressFinalize(this);
		}

		~KeyboardInput()
		{
			ReleaseUnmanagedResources();
		}

		private void ReleaseUnmanagedResources()
		{
			Program.CoreFunctions.ReleaseInput(handle);
		}

		public Keys[] GetPressedKeys()
		{
			Contract.Ensures(Contract.Result<Keys[]>() != null);

			return Program.CoreFunctions.GetPressedKeys(handle);
		}
	}
}
