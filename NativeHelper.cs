using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;

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
			DisassembleRemoteCode,
			ControlRemoteProcess
		}

		public class MethodInfo
		{
			public RequestFunction Method { get; set; }
			public string Provider { get; set; }
			public IntPtr FunctionPtr { get; set; }

			public T GetDelegate<T>()
			{
				return Marshal.GetDelegateForFunctionPointer<T>(FunctionPtr);
			}
		}

		private IntPtr nativeHelperHandle;

		private readonly Dictionary<RequestFunction, List<MethodInfo>> methodRegistry = new Dictionary<RequestFunction, List<MethodInfo>>();

		public IReadOnlyDictionary<RequestFunction, List<MethodInfo>> MethodRegistry => methodRegistry;

		#region Delegates

		public delegate IntPtr RequestFunctionPtrCallback(RequestFunction request);
		private delegate void InitializeDelegate(RequestFunctionPtrCallback requestCallback);

		//private delegate int GetLastErrorDelegate();

		private delegate bool IsProcessValidDelegate(IntPtr process);

		private delegate IntPtr OpenRemoteProcessDelegate(int pid, int desiredAccess);

		private delegate void CloseRemoteProcessDelegate(IntPtr process);

		private delegate bool ReadRemoteMemoryDelegate(IntPtr process, IntPtr address, IntPtr buffer, uint size);

		private delegate bool WriteRemoteMemoryDelegate(IntPtr process, IntPtr address, IntPtr buffer, uint size);

		public delegate void EnumerateProcessCallback(uint pid, [MarshalAs(UnmanagedType.LPWStr)]string modulePath);
		private delegate void EnumerateProcessesDelegate(EnumerateProcessCallback callbackProcess);

		public delegate void EnumerateRemoteSectionCallback(IntPtr baseAddress, IntPtr regionSize, [MarshalAs(UnmanagedType.LPStr)]string name, NativeMethods.StateEnum state, NativeMethods.AllocationProtectEnum protection, NativeMethods.TypeEnum type, [MarshalAs(UnmanagedType.LPWStr)]string modulePath);
		public delegate void EnumerateRemoteModuleCallback(IntPtr baseAddress, IntPtr regionSize, [MarshalAs(UnmanagedType.LPWStr)]string modulePath);
		private delegate void EnumerateRemoteSectionsAndModulesDelegate(IntPtr process, EnumerateRemoteSectionCallback callbackSection, EnumerateRemoteModuleCallback callbackModule);

		public delegate void DisassembleRemoteCodeCallback(IntPtr address, int length, [MarshalAs(UnmanagedType.LPStr)]string instruction);
		private delegate void DisassembleRemoteCodeDelegate(IntPtr process, IntPtr address, int length, DisassembleRemoteCodeCallback callbackDisassembledCode);

		public enum ControlRemoteProcessAction
		{
			Suspend,
			Resume,
			Terminate
		}
		private delegate void ControlRemoteProcessDelegate(IntPtr process, ControlRemoteProcessAction action);

		#endregion

		private IntPtr fnIsProcessValid;
		private IsProcessValidDelegate isProcessValidDelegate;
		private IntPtr fnOpenRemoteProcess;
		private OpenRemoteProcessDelegate openRemoteProcessDelegate;
		private IntPtr fnCloseRemoteProcess;
		private CloseRemoteProcessDelegate closeRemoteProcessDelegate;
		private IntPtr fnReadRemoteMemory;
		private ReadRemoteMemoryDelegate readRemoteMemoryDelegate;
		private IntPtr fnWriteRemoteMemory;
		private WriteRemoteMemoryDelegate writeRemoteMemoryDelegate;
		private IntPtr fnEnumerateProcesses;
		private EnumerateProcessesDelegate enumerateProcessesDelegate;
		private IntPtr fnEnumerateRemoteSectionsAndModules;
		private EnumerateRemoteSectionsAndModulesDelegate enumerateRemoteSectionsAndModulesDelegate;
		private IntPtr fnDisassembleRemoteCode;
		private DisassembleRemoteCodeDelegate disassembleRemoteCodeDelegate;
		private IntPtr fnControlRemoteProcess;
		private ControlRemoteProcessDelegate controlRemoteProcessDelegate;

		private RequestFunctionPtrCallback requestFunctionPtrReference;

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

			nativeHelperHandle = NativeMethods.LoadLibrary(NativeHelperDll);
			if (nativeHelperHandle.IsNull())
			{
				throw new Exception();
			}

			InintializeNativeModule(nativeHelperHandle);

			RegisterProvidedNativeMethods(nativeHelperHandle, "Default");

			SetActiveNativeMethod(methodRegistry[RequestFunction.IsProcessValid].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.OpenRemoteProcess].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.CloseRemoteProcess].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.ReadRemoteMemory].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.WriteRemoteMemory].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.EnumerateProcesses].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.EnumerateRemoteSectionsAndModules].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.DisassembleRemoteCode].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.ControlRemoteProcess].First());
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
					NativeMethods.FreeLibrary(nativeHelperHandle);

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

		public void InintializeNativeModule(IntPtr module)
		{
			Contract.Requires(!module.IsNull());

			var fnInitialize = NativeMethods.GetProcAddress(module, "Initialize");
			if (fnInitialize.IsNull())
			{
				throw new Exception();
			}

			var initializeDelegate = Marshal.GetDelegateForFunctionPointer<InitializeDelegate>(fnInitialize);
			initializeDelegate(requestFunctionPtrReference);
		}

		#region Registry

		public void RegisterProvidedNativeMethods(IntPtr module, string provider)
		{
			Contract.Requires(provider != null);

			foreach (var method in new RequestFunction[]
			{
				RequestFunction.IsProcessValid,
				RequestFunction.OpenRemoteProcess,
				RequestFunction.CloseRemoteProcess,
				RequestFunction.ReadRemoteMemory,
				RequestFunction.WriteRemoteMemory,
				RequestFunction.EnumerateProcesses,
				RequestFunction.EnumerateRemoteSectionsAndModules,
				RequestFunction.DisassembleRemoteCode,
				RequestFunction.ControlRemoteProcess
			})
			{
				var functionPtr = NativeMethods.GetProcAddress(module, method.ToString());
				if (!functionPtr.IsNull())
				{
					RegisterMethod(method, new MethodInfo { Method = method, Provider = provider, FunctionPtr = functionPtr });
				}
			}
		}

		private void RegisterMethod(RequestFunction method, MethodInfo methodInfo)
		{
			Contract.Requires(methodInfo != null);

			List<MethodInfo> infos;
			if (!methodRegistry.TryGetValue(method, out infos))
			{
				infos = new List<MethodInfo>();

				methodRegistry.Add(method, infos);
			}

			infos.Add(methodInfo);
		}

		public void SetActiveNativeMethod(MethodInfo methodInfo)
		{
			switch (methodInfo.Method)
			{
				case RequestFunction.EnumerateProcesses:
					fnEnumerateProcesses = methodInfo.FunctionPtr;
					enumerateProcessesDelegate = Marshal.GetDelegateForFunctionPointer<EnumerateProcessesDelegate>(fnEnumerateProcesses);
					break;
				case RequestFunction.EnumerateRemoteSectionsAndModules:
					fnEnumerateRemoteSectionsAndModules = methodInfo.FunctionPtr;
					enumerateRemoteSectionsAndModulesDelegate = Marshal.GetDelegateForFunctionPointer<EnumerateRemoteSectionsAndModulesDelegate>(fnEnumerateRemoteSectionsAndModules);
					break;
				case RequestFunction.IsProcessValid:
					fnIsProcessValid = methodInfo.FunctionPtr;
					isProcessValidDelegate = Marshal.GetDelegateForFunctionPointer<IsProcessValidDelegate>(fnIsProcessValid);
					break;
				case RequestFunction.OpenRemoteProcess:
					fnOpenRemoteProcess = methodInfo.FunctionPtr;
					openRemoteProcessDelegate = Marshal.GetDelegateForFunctionPointer<OpenRemoteProcessDelegate>(fnOpenRemoteProcess);
					break;
				case RequestFunction.CloseRemoteProcess:
					fnCloseRemoteProcess = methodInfo.FunctionPtr;
					closeRemoteProcessDelegate = Marshal.GetDelegateForFunctionPointer<CloseRemoteProcessDelegate>(fnCloseRemoteProcess);
					break;
				case RequestFunction.ReadRemoteMemory:
					fnReadRemoteMemory = methodInfo.FunctionPtr;
					readRemoteMemoryDelegate = Marshal.GetDelegateForFunctionPointer<ReadRemoteMemoryDelegate>(fnReadRemoteMemory);
					break;
				case RequestFunction.WriteRemoteMemory:
					fnWriteRemoteMemory = methodInfo.FunctionPtr;
					writeRemoteMemoryDelegate = Marshal.GetDelegateForFunctionPointer<WriteRemoteMemoryDelegate>(fnWriteRemoteMemory);
					break;
				case RequestFunction.DisassembleRemoteCode:
					fnDisassembleRemoteCode = methodInfo.FunctionPtr;
					disassembleRemoteCodeDelegate = Marshal.GetDelegateForFunctionPointer<DisassembleRemoteCodeDelegate>(fnDisassembleRemoteCode);
					break;
				case RequestFunction.ControlRemoteProcess:
					fnControlRemoteProcess = methodInfo.FunctionPtr;
					controlRemoteProcessDelegate = Marshal.GetDelegateForFunctionPointer<ControlRemoteProcessDelegate>(fnControlRemoteProcess);
					break;
			}
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
				case RequestFunction.ControlRemoteProcess:
					return fnControlRemoteProcess;
			}

			throw new ArgumentException(nameof(request));
		}

		#region Delegate Wrapper

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
			Contract.Requires(buffer != null);

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			var result = readRemoteMemoryDelegate(process, address, handle.AddrOfPinnedObject(), size);
			handle.Free();

			return result;
		}

		public bool WriteRemoteMemory(IntPtr process, IntPtr address, byte[] buffer, uint size)
		{
			Contract.Requires(buffer != null);

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

		public void ControlRemoteProcess(IntPtr process, ControlRemoteProcessAction action)
		{
			controlRemoteProcessDelegate(process, action);
		}

		#endregion
	}
}
