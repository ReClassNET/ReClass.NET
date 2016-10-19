using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET
{
	public class NativeHelper : IDisposable
	{
		public enum RequestFunction
		{
			IsProcessValid,
			OpenRemoteProcess,
			CloseRemoteProcess,
			ReadRemoteMemory,
			WriteRemoteMemory,
			EnumerateProcesses,
			EnumerateRemoteSectionsAndModules,
			DisassembleRemoteCode
		}

		private IntPtr nativeHelperHandle;

		public delegate IntPtr RequestFunctionPtrCallback(RequestFunction request);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void InitializeDelegate(RequestFunctionPtrCallback requestCallback);

		private RequestFunctionPtrCallback requestFunctionPtrReference;
		public RequestFunctionPtrCallback RequestFunctionPtrReference => requestFunctionPtrReference;

		private IntPtr fnGetLastError;
		private IntPtr fnIsProcessValid;
		private IntPtr fnOpenRemoteProcess;
		private IntPtr fnCloseRemoteProcess;
		private IntPtr fnReadRemoteMemory;
		private IntPtr fnWriteRemoteMemory;
		private IntPtr fnEnumerateProcesses;
		private IntPtr fnEnumerateRemoteSectionsAndModules;
		private IntPtr fnDisassembleRemoteCode;

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate int GetLastErrorDelegate();
		private GetLastErrorDelegate getLastErrorDelegate;

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate bool IsProcessValidDelegate(IntPtr process);
		private IsProcessValidDelegate isProcessValidDelegate;

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate IntPtr OpenRemoteProcessDelegate(int pid, int desiredAccess);
		private OpenRemoteProcessDelegate openRemoteProcessDelegate;

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void CloseRemoteProcessDelegate(IntPtr process);
		private CloseRemoteProcessDelegate closeRemoteProcessDelegate;

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate bool ReadRemoteMemoryDelegate(IntPtr process, IntPtr address, IntPtr buffer, uint size);
		private ReadRemoteMemoryDelegate readRemoteMemoryDelegate;

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate bool WriteRemoteMemoryDelegate(IntPtr process, IntPtr address, IntPtr buffer, uint size);
		private WriteRemoteMemoryDelegate writeRemoteMemoryDelegate;

		public delegate void EnumerateProcessCallback(uint pid, [MarshalAs(UnmanagedType.LPWStr)]string modulePath);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void EnumerateProcessesDelegate(EnumerateProcessCallback callbackProcess);
		private EnumerateProcessesDelegate enumerateProcessesDelegate;

		public delegate void EnumerateRemoteSectionCallback(IntPtr baseAddress, IntPtr regionSize, [MarshalAs(UnmanagedType.LPStr)]string name, Natives.StateEnum state, Natives.AllocationProtectEnum protection, Natives.TypeEnum type, [MarshalAs(UnmanagedType.LPWStr)]string modulePath);
		public delegate void EnumerateRemoteModuleCallback(IntPtr baseAddress, IntPtr regionSize, [MarshalAs(UnmanagedType.LPWStr)]string modulePath);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void EnumerateRemoteSectionsAndModulesDelegate(IntPtr process, EnumerateRemoteSectionCallback callbackSection, EnumerateRemoteModuleCallback callbackModule);
		private EnumerateRemoteSectionsAndModulesDelegate enumerateRemoteSectionsAndModulesDelegate;

		public delegate void DisassembleRemoteCodeCallback(IntPtr address, int length, [MarshalAs(UnmanagedType.LPStr)]string instruction);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void DisassembleRemoteCodeDelegate(IntPtr process, IntPtr address, int length, DisassembleRemoteCodeCallback callbackDisassembledCode);
		private DisassembleRemoteCodeDelegate disassembleRemoteCodeDelegate;

		private bool disposedValue = false;

		public NativeHelper()
		{
			requestFunctionPtrReference = RequestFunctionPtr;

#if DEBUG
#if WIN32
			const string NativeHelperDll = "NativeHelper_x86d.dll";
#else
			const string NativeHelperDll = "NativeHelper_x64d.dll";
#endif
#else
#if WIN32
			const string NativeHelperDll = "NativeHelper_x86.dll";
#else
			const string NativeHelperDll = "NativeHelper_x64.dll";
#endif
#endif

			nativeHelperHandle = Natives.LoadLibrary(NativeHelperDll);
			if (nativeHelperHandle.IsNull())
			{
				throw new Exception();
			}

			var fnInitialize = Natives.GetProcAddress(nativeHelperHandle, "Initialize");
			var initializeDelegate = Marshal.GetDelegateForFunctionPointer<InitializeDelegate>(fnInitialize);
			initializeDelegate(RequestFunctionPtrReference);

			fnGetLastError = Natives.GetProcAddress(nativeHelperHandle, "GetLastErrorCode");
			getLastErrorDelegate = Marshal.GetDelegateForFunctionPointer<GetLastErrorDelegate>(fnGetLastError);

			fnIsProcessValid = Natives.GetProcAddress(nativeHelperHandle, "IsProcessValid");
			isProcessValidDelegate = Marshal.GetDelegateForFunctionPointer<IsProcessValidDelegate>(fnIsProcessValid);

			fnOpenRemoteProcess = Natives.GetProcAddress(nativeHelperHandle, "OpenRemoteProcess");
			openRemoteProcessDelegate = Marshal.GetDelegateForFunctionPointer<OpenRemoteProcessDelegate>(fnOpenRemoteProcess);

			fnCloseRemoteProcess = Natives.GetProcAddress(nativeHelperHandle, "CloseRemoteProcess");
			closeRemoteProcessDelegate = Marshal.GetDelegateForFunctionPointer<CloseRemoteProcessDelegate>(fnCloseRemoteProcess);

			fnReadRemoteMemory = Natives.GetProcAddress(nativeHelperHandle, "ReadRemoteMemory");
			readRemoteMemoryDelegate = Marshal.GetDelegateForFunctionPointer<ReadRemoteMemoryDelegate>(fnReadRemoteMemory);

			fnWriteRemoteMemory = Natives.GetProcAddress(nativeHelperHandle, "WriteRemoteMemory");
			writeRemoteMemoryDelegate = Marshal.GetDelegateForFunctionPointer<WriteRemoteMemoryDelegate>(fnWriteRemoteMemory);

			fnEnumerateProcesses = Natives.GetProcAddress(nativeHelperHandle, "EnumerateProcesses");
			enumerateProcessesDelegate = Marshal.GetDelegateForFunctionPointer<EnumerateProcessesDelegate>(fnEnumerateProcesses);

			fnEnumerateRemoteSectionsAndModules = Natives.GetProcAddress(nativeHelperHandle, "EnumerateRemoteSectionsAndModules");
			enumerateRemoteSectionsAndModulesDelegate = Marshal.GetDelegateForFunctionPointer<EnumerateRemoteSectionsAndModulesDelegate>(fnEnumerateRemoteSectionsAndModules);

			fnDisassembleRemoteCode = Natives.GetProcAddress(nativeHelperHandle, "DisassembleRemoteCode");
			disassembleRemoteCodeDelegate = Marshal.GetDelegateForFunctionPointer<DisassembleRemoteCodeDelegate>(fnDisassembleRemoteCode);
		}

		#region IDisposable Support

		~NativeHelper()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{

				}

				if (!nativeHelperHandle.IsNull())
				{
					Natives.FreeLibrary(nativeHelperHandle);

					nativeHelperHandle = IntPtr.Zero;
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion

		public IntPtr RequestFunctionPtr(RequestFunction request)
		{
			switch (request)
			{
				case RequestFunction.IsProcessValid:
					return fnIsProcessValid;
				case RequestFunction.OpenRemoteProcess:
					return fnOpenRemoteProcess;
				case RequestFunction.CloseRemoteProcess:
					return fnCloseRemoteProcess;
				case RequestFunction.ReadRemoteMemory:
					return fnReadRemoteMemory;
				case RequestFunction.WriteRemoteMemory:
					return fnWriteRemoteMemory;
				case RequestFunction.EnumerateProcesses:
					return fnEnumerateProcesses;
				case RequestFunction.EnumerateRemoteSectionsAndModules:
					return fnEnumerateRemoteSectionsAndModules;
				case RequestFunction.DisassembleRemoteCode:
					return fnDisassembleRemoteCode;
			}

			return IntPtr.Zero;
		}

		public bool IsProcessValid(IntPtr process)
		{
			return isProcessValidDelegate(process);
		}

		public IntPtr OpenRemoteProcess(int pid, int desiredAccess)
		{
			return openRemoteProcessDelegate(pid, desiredAccess);
		}

		public void CloseRemoteProcess(IntPtr process)
		{
			closeRemoteProcessDelegate(process);
		}

		public bool ReadRemoteMemory(IntPtr process, IntPtr address, byte[] buffer, uint size)
		{
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			var result = readRemoteMemoryDelegate(process, address, handle.AddrOfPinnedObject(), size);
			handle.Free();

			return result;
		}

		public bool WriteRemoteMemory(IntPtr process, IntPtr address, byte[] buffer, uint size)
		{
			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			var result = writeRemoteMemoryDelegate(process, address, handle.AddrOfPinnedObject(), size);
			handle.Free();

			return result;
		}

		public void EnumerateProcesses(EnumerateProcessCallback callbackProcess)
		{
			enumerateProcessesDelegate(callbackProcess);
		}

		public void EnumerateRemoteSectionsAndModules(IntPtr process, EnumerateRemoteSectionCallback callbackSection, EnumerateRemoteModuleCallback callbackModule)
		{
			enumerateRemoteSectionsAndModulesDelegate(process, callbackSection, callbackModule);
		}

		public void DisassembleRemoteCode(IntPtr process, IntPtr address, int length, DisassembleRemoteCodeCallback remoteCodeCallback)
		{
			disassembleRemoteCodeDelegate(process, address, length, remoteCodeCallback);
		}
	}
}
