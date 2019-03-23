using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ReClassNET.Extensions;
using ReClassNET.Native;
using ReClassNET.Util;

namespace ReClassNET.Core
{
	public delegate bool EnumerateInstructionCallback(ref InstructionData data);

	internal class InternalCoreFunctions : NativeCoreWrapper, IInternalCoreFunctions, IDisposable
	{
		private const string CoreFunctionsModuleWindows = "NativeCore.dll";
		private const string CoreFunctionsModuleUnix = "NativeCore.so";

		private readonly IntPtr handle;

		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool DisassembleCodeDelegate(IntPtr address, IntPtr length, IntPtr virtualAddress, [MarshalAs(UnmanagedType.I1)] bool determineStaticInstructionBytes, [MarshalAs(UnmanagedType.FunctionPtr)] EnumerateInstructionCallback callback);

		private delegate IntPtr InitializeInputDelegate();

		private delegate bool GetPressedKeysDelegate(IntPtr handle, out IntPtr pressedKeysArrayPtr, out int length);

		private delegate void ReleaseInputDelegate(IntPtr handle);

		private readonly DisassembleCodeDelegate disassembleCodeDelegate;

		private readonly InitializeInputDelegate initializeInputDelegate;
		private readonly GetPressedKeysDelegate getPressedKeysDelegate;
		private readonly ReleaseInputDelegate releaseInputDelegate;

		private InternalCoreFunctions(IntPtr handle)
			: base(handle)
		{
			this.handle = handle;

			disassembleCodeDelegate = GetFunctionDelegate<DisassembleCodeDelegate>(handle, "DisassembleCode");

			initializeInputDelegate = GetFunctionDelegate<InitializeInputDelegate>(handle, "InitializeInput");
			getPressedKeysDelegate = GetFunctionDelegate<GetPressedKeysDelegate>(handle, "GetPressedKeys");
			releaseInputDelegate = GetFunctionDelegate<ReleaseInputDelegate>(handle, "ReleaseInput");
		}

		public static InternalCoreFunctions Create()
		{
			var libraryName = NativeMethods.IsUnix() ? CoreFunctionsModuleUnix : CoreFunctionsModuleWindows;
			var libraryPath = Path.Combine(PathUtil.ExecutableFolderPath, libraryName);

			var handle = NativeMethods.LoadLibrary(libraryPath);
			if (handle.IsNull())
			{
				throw new FileNotFoundException($"Failed to load native core functions! Couldnt find at location {libraryPath}");
			}

			return new InternalCoreFunctions(handle);
		}

		#region IDisposable Support

		~InternalCoreFunctions()
		{
			ReleaseUnmanagedResources();
		}

		private void ReleaseUnmanagedResources()
		{
			NativeMethods.FreeLibrary(handle);
		}

		public void Dispose()
		{
			ReleaseUnmanagedResources();

			GC.SuppressFinalize(this);
		}

		#endregion

		public bool DisassembleCode(IntPtr address, int length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback)
		{
			return disassembleCodeDelegate(address, (IntPtr)length, virtualAddress, determineStaticInstructionBytes, callback);
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
