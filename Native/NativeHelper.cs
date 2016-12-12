using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ReClassNET.Debugger;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace ReClassNET.Native
{
	public class NativeHelper : IDisposable
	{
		private const string NativeHelperDll = "NativeHelper.dll";

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

		private delegate bool IsProcessValidDelegate(IntPtr process);

		private delegate IntPtr OpenRemoteProcessDelegate(IntPtr pid, ProcessAccess desiredAccess);

		private delegate void CloseRemoteProcessDelegate(IntPtr process);

		private delegate bool ReadRemoteMemoryDelegate(IntPtr process, IntPtr address, IntPtr buffer, IntPtr size);

		private delegate bool WriteRemoteMemoryDelegate(IntPtr process, IntPtr address, IntPtr buffer, IntPtr size);

		public delegate bool EnumerateProcessCallback(ref EnumerateProcessData data);
		private delegate void EnumerateProcessesDelegate(EnumerateProcessCallback callbackProcess);

		public delegate void EnumerateRemoteSectionCallback(ref EnumerateRemoteSectionData data);
		public delegate void EnumerateRemoteModuleCallback(ref EnumerateRemoteModuleData data);
		private delegate void EnumerateRemoteSectionsAndModulesDelegate(IntPtr process, EnumerateRemoteSectionCallback callbackSection, EnumerateRemoteModuleCallback callbackModule);

		private delegate bool DisassembleCodeDelegate(IntPtr address, IntPtr length, IntPtr virtualAddress, out InstructionData instruction);

		private delegate void ControlRemoteProcessDelegate(IntPtr process, ControlRemoteProcessAction action);

		private delegate bool DebuggerAttachToProcessDelegate(IntPtr id);
		private delegate void DebuggerDetachFromProcessDelegate(IntPtr id);
		private delegate bool DebuggerWaitForDebugEventDelegate(ref DebugEvent e);
		private delegate void DebuggerContinueEventDelegate(ref DebugEvent e);
		private delegate bool DebuggerSetHardwareBreakpointDelegate(IntPtr id, IntPtr address, HardwareBreakpointRegister register, HardwareBreakpointType type, HardwareBreakpointSize size, bool set);

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
		private IntPtr fnDisassembleCode;
		private DisassembleCodeDelegate disassembleCodeDelegate;
		private IntPtr fnControlRemoteProcess;
		private ControlRemoteProcessDelegate controlRemoteProcessDelegate;
		private IntPtr fnDebuggerAttachToProcess;
		private DebuggerAttachToProcessDelegate debuggerAttachToProcessDelegate;
		private IntPtr fnDebuggerDetachFromProcess;
		private DebuggerDetachFromProcessDelegate debuggerDetachFromProcessDelegate;
		private IntPtr fnDebuggerWaitForDebugEvent;
		private DebuggerWaitForDebugEventDelegate debuggerWaitForDebugEventDelegate;
		private IntPtr fnDebuggerContinueEvent;
		private DebuggerContinueEventDelegate debuggerContinueEventDelegate;
		private IntPtr fnDebuggerSetHardwareBreakpoint;
		private DebuggerSetHardwareBreakpointDelegate debuggerSetHardwareBreakpointDelegate;

		private readonly RequestFunctionPtrCallback requestFunctionPtrReference;

		#endregion

		private bool disposedValue = false;

		public NativeHelper()
		{
			requestFunctionPtrReference = RequestFunctionPtr;

			nativeHelperHandle = NativeMethods.LoadLibrary(NativeHelperDll);
			if (nativeHelperHandle.IsNull())
			{
				throw new FileNotFoundException(NativeHelperDll);
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
			SetActiveNativeMethod(methodRegistry[RequestFunction.DisassembleCode].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.ControlRemoteProcess].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.DebuggerAttachToProcess].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.DebuggerDetachFromProcess].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.DebuggerWaitForDebugEvent].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.DebuggerContinueEvent].First());
			SetActiveNativeMethod(methodRegistry[RequestFunction.DebuggerSetHardwareBreakpoint].First());
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
				RequestFunction.DisassembleCode,
				RequestFunction.ControlRemoteProcess,
				RequestFunction.DebuggerAttachToProcess,
				RequestFunction.DebuggerDetachFromProcess,
				RequestFunction.DebuggerWaitForDebugEvent,
				RequestFunction.DebuggerContinueEvent,
				RequestFunction.DebuggerSetHardwareBreakpoint
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
			Contract.Requires(methodInfo != null);

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
				case RequestFunction.DisassembleCode:
					fnDisassembleCode = methodInfo.FunctionPtr;
					disassembleCodeDelegate = Marshal.GetDelegateForFunctionPointer<DisassembleCodeDelegate>(fnDisassembleCode);
					break;
				case RequestFunction.ControlRemoteProcess:
					fnControlRemoteProcess = methodInfo.FunctionPtr;
					controlRemoteProcessDelegate = Marshal.GetDelegateForFunctionPointer<ControlRemoteProcessDelegate>(fnControlRemoteProcess);
					break;
				case RequestFunction.DebuggerAttachToProcess:
					fnDebuggerAttachToProcess = methodInfo.FunctionPtr;
					debuggerAttachToProcessDelegate = Marshal.GetDelegateForFunctionPointer<DebuggerAttachToProcessDelegate>(fnDebuggerAttachToProcess);
					break;
				case RequestFunction.DebuggerDetachFromProcess:
					fnDebuggerDetachFromProcess = methodInfo.FunctionPtr;
					debuggerDetachFromProcessDelegate = Marshal.GetDelegateForFunctionPointer<DebuggerDetachFromProcessDelegate>(fnDebuggerDetachFromProcess);
					break;
				case RequestFunction.DebuggerWaitForDebugEvent:
					fnDebuggerWaitForDebugEvent = methodInfo.FunctionPtr;
					debuggerWaitForDebugEventDelegate = Marshal.GetDelegateForFunctionPointer<DebuggerWaitForDebugEventDelegate>(fnDebuggerWaitForDebugEvent);
					break;
				case RequestFunction.DebuggerContinueEvent:
					fnDebuggerContinueEvent = methodInfo.FunctionPtr;
					debuggerContinueEventDelegate = Marshal.GetDelegateForFunctionPointer<DebuggerContinueEventDelegate>(fnDebuggerContinueEvent);
					break;
				case RequestFunction.DebuggerSetHardwareBreakpoint:
					fnDebuggerSetHardwareBreakpoint = methodInfo.FunctionPtr;
					debuggerSetHardwareBreakpointDelegate = Marshal.GetDelegateForFunctionPointer<DebuggerSetHardwareBreakpointDelegate>(fnDebuggerSetHardwareBreakpoint);
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
				case RequestFunction.DisassembleCode:
					return fnDisassembleCode;
				case RequestFunction.ControlRemoteProcess:
					return fnControlRemoteProcess;
				case RequestFunction.DebuggerAttachToProcess:
					return fnDebuggerAttachToProcess;
				case RequestFunction.DebuggerDetachFromProcess:
					return fnDebuggerDetachFromProcess;
				case RequestFunction.DebuggerWaitForDebugEvent:
					return fnDebuggerWaitForDebugEvent;
				case RequestFunction.DebuggerContinueEvent:
					return fnDebuggerContinueEvent;
				case RequestFunction.DebuggerSetHardwareBreakpoint:
					return fnDebuggerSetHardwareBreakpoint;
			}

			throw new ArgumentException(nameof(request));
		}

		#region Delegate Wrapper

		public bool IsProcessValid(IntPtr process)
		{
			return isProcessValidDelegate(process);
		}

		public IntPtr OpenRemoteProcess(IntPtr pid, ProcessAccess desiredAccess)
		{
			return openRemoteProcessDelegate(pid, desiredAccess);
		}

		public void CloseRemoteProcess(IntPtr process)
		{
			closeRemoteProcessDelegate(process);
		}

		public bool ReadRemoteMemory(IntPtr process, IntPtr address, byte[] buffer, int offset, int length)
		{
			Contract.Requires(buffer != null);

			if (offset + length > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset + length");
			}

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			var result = readRemoteMemoryDelegate(process, address, handle.AddrOfPinnedObject() + offset, (IntPtr)length);
			handle.Free();

			return result;
		}

		public bool WriteRemoteMemory(IntPtr process, IntPtr address, byte[] buffer, int size)
		{
			Contract.Requires(buffer != null);

			GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			var result = writeRemoteMemoryDelegate(process, address, handle.AddrOfPinnedObject(), (IntPtr)size);
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

		public void EnumerateRemoteSectionsAndModules(IntPtr process, Action<Section> callbackSection, Action<Module> callbackModule)
		{
			var c1 = callbackSection == null ? (EnumerateRemoteSectionCallback)null : delegate (ref EnumerateRemoteSectionData data)
			{
				var section = new Section
				{
					Start = data.BaseAddress,
					End = data.BaseAddress.Add(data.Size),
					Size = data.Size,
					Name = data.Name,
					Protection = data.Protection,
					Type = data.Type,
					ModulePath = data.ModulePath,
					ModuleName = Path.GetFileName(data.ModulePath),
					Category = data.Type == SectionType.Private ? SectionCategory.HEAP : SectionCategory.Unknown
				};
				switch (section.Name)
				{
					case ".text":
					case "code":
						section.Category = SectionCategory.CODE;
						break;
					case ".data":
					case "data":
					case ".rdata":
					case ".idata":
						section.Category = SectionCategory.DATA;
						break;
				}
				callbackSection(section);
			};
			
			var c2 = callbackModule == null ? (EnumerateRemoteModuleCallback)null : delegate (ref EnumerateRemoteModuleData data)
			{
				callbackModule(new Module
				{
					Start = data.BaseAddress,
					End = data.BaseAddress.Add(data.Size),
					Size = data.Size,
					Path = data.Path,
					Name = Path.GetFileName(data.Path)
				});
			};

			EnumerateRemoteSectionsAndModules(process, c1, c2);
		}

		public bool DisassembleCode(IntPtr address, int length, IntPtr virtualAddress, out InstructionData instruction)
		{
			return disassembleCodeDelegate(address, (IntPtr)length, virtualAddress, out instruction);
		}

		public void ControlRemoteProcess(IntPtr process, ControlRemoteProcessAction action)
		{
			controlRemoteProcessDelegate(process, action);
		}

		public bool DebuggerAttachToProcess(IntPtr id)
		{
			return debuggerAttachToProcessDelegate(id);
		}

		public void DebuggerDetachFromProcess(IntPtr id)
		{
			debuggerDetachFromProcessDelegate(id);
		}

		public bool DebuggerWaitForDebugEvent(ref DebugEvent e)
		{
			return debuggerWaitForDebugEventDelegate(ref e);
		}

		public void DebuggerContinueEvent(ref DebugEvent e)
		{
			debuggerContinueEventDelegate(ref e);
		}

		public bool DebuggerSetHardwareBreakpoint(IntPtr id, HardwareBreakpoint hwbp, bool set)
		{
			return debuggerSetHardwareBreakpointDelegate(id, hwbp.Address, hwbp.Register, hwbp.Type, hwbp.Size, set);
		}

		#endregion
	}
}
