using System;
using System.Runtime.InteropServices;
using ReClassNET.Debugger;
using ReClassNET.Extensions;
using ReClassNET.Native;

namespace ReClassNET.Core
{
	public class NativeCoreWrapper : ICoreProcessFunctions
	{
		#region Native Delegates

		private delegate void EnumerateProcessesDelegate([MarshalAs(UnmanagedType.FunctionPtr)] EnumerateProcessCallback callbackProcess);

		private delegate void EnumerateRemoteSectionsAndModulesDelegate(IntPtr process, [MarshalAs(UnmanagedType.FunctionPtr)] EnumerateRemoteSectionCallback callbackSection, [MarshalAs(UnmanagedType.FunctionPtr)] EnumerateRemoteModuleCallback callbackModule);

		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool IsProcessValidDelegate(IntPtr process);

		private delegate IntPtr OpenRemoteProcessDelegate(IntPtr pid, ProcessAccess desiredAccess);

		private delegate void CloseRemoteProcessDelegate(IntPtr process);

		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool ReadRemoteMemoryDelegate(IntPtr process, IntPtr address, [Out] byte[] buffer, int offset, int size);

		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool WriteRemoteMemoryDelegate(IntPtr process, IntPtr address, [In] byte[] buffer, int offset, int size);

		private delegate void ControlRemoteProcessDelegate(IntPtr process, ControlRemoteProcessAction action);

		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool AttachDebuggerToProcessDelegate(IntPtr id);

		private delegate void DetachDebuggerFromProcessDelegate(IntPtr id);

		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool AwaitDebugEventDelegate([In, Out] ref DebugEvent evt, int timeoutInMilliseconds);

		private delegate void HandleDebugEventDelegate([In, Out] ref DebugEvent evt);

		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool SetHardwareBreakpointDelegate(IntPtr id, IntPtr address, HardwareBreakpointRegister register, HardwareBreakpointTrigger trigger, HardwareBreakpointSize size, [param: MarshalAs(UnmanagedType.I1)] bool set);

		private readonly EnumerateProcessesDelegate enumerateProcessesDelegate;
		private readonly EnumerateRemoteSectionsAndModulesDelegate enumerateRemoteSectionsAndModulesDelegate;
		private readonly OpenRemoteProcessDelegate openRemoteProcessDelegate;
		private readonly IsProcessValidDelegate isProcessValidDelegate;
		private readonly CloseRemoteProcessDelegate closeRemoteProcessDelegate;
		private readonly ReadRemoteMemoryDelegate readRemoteMemoryDelegate;
		private readonly WriteRemoteMemoryDelegate writeRemoteMemoryDelegate;
		private readonly ControlRemoteProcessDelegate controlRemoteProcessDelegate;
		private readonly AttachDebuggerToProcessDelegate attachDebuggerToProcessDelegate;
		private readonly DetachDebuggerFromProcessDelegate detachDebuggerFromProcessDelegate;
		private readonly AwaitDebugEventDelegate awaitDebugEventDelegate;
		private readonly HandleDebugEventDelegate handleDebugEventDelegate;
		private readonly SetHardwareBreakpointDelegate setHardwareBreakpointDelegate;

		#endregion

		public NativeCoreWrapper(IntPtr handle)
		{
			if (handle.IsNull())
			{
				throw new ArgumentNullException();
			}

			enumerateProcessesDelegate = GetFunctionDelegate<EnumerateProcessesDelegate>(handle, "EnumerateProcesses");
			enumerateRemoteSectionsAndModulesDelegate = GetFunctionDelegate<EnumerateRemoteSectionsAndModulesDelegate>(handle, "EnumerateRemoteSectionsAndModules");
			openRemoteProcessDelegate = GetFunctionDelegate<OpenRemoteProcessDelegate>(handle, "OpenRemoteProcess");
			isProcessValidDelegate = GetFunctionDelegate<IsProcessValidDelegate>(handle, "IsProcessValid");
			closeRemoteProcessDelegate = GetFunctionDelegate<CloseRemoteProcessDelegate>(handle, "CloseRemoteProcess");
			readRemoteMemoryDelegate = GetFunctionDelegate<ReadRemoteMemoryDelegate>(handle, "ReadRemoteMemory");
			writeRemoteMemoryDelegate = GetFunctionDelegate<WriteRemoteMemoryDelegate>(handle, "WriteRemoteMemory");
			controlRemoteProcessDelegate = GetFunctionDelegate<ControlRemoteProcessDelegate>(handle, "ControlRemoteProcess");
			attachDebuggerToProcessDelegate = GetFunctionDelegate<AttachDebuggerToProcessDelegate>(handle, "AttachDebuggerToProcess");
			detachDebuggerFromProcessDelegate = GetFunctionDelegate<DetachDebuggerFromProcessDelegate>(handle, "DetachDebuggerFromProcess");
			awaitDebugEventDelegate = GetFunctionDelegate<AwaitDebugEventDelegate>(handle, "AwaitDebugEvent");
			handleDebugEventDelegate = GetFunctionDelegate<HandleDebugEventDelegate>(handle, "HandleDebugEvent");
			setHardwareBreakpointDelegate = GetFunctionDelegate<SetHardwareBreakpointDelegate>(handle, "SetHardwareBreakpoint");
		}

		protected static TDelegate GetFunctionDelegate<TDelegate>(IntPtr handle, string function)
		{
			var address = NativeMethods.GetProcAddress(handle, function);
			if (address.IsNull())
			{
				throw new Exception($"Function '{function}' not found.");
			}
			return Marshal.GetDelegateForFunctionPointer<TDelegate>(address);
		}

		public void EnumerateProcesses(EnumerateProcessCallback callbackProcess)
		{
			enumerateProcessesDelegate(callbackProcess);
		}

		public void EnumerateRemoteSectionsAndModules(IntPtr process, EnumerateRemoteSectionCallback callbackSection, EnumerateRemoteModuleCallback callbackModule)
		{
			enumerateRemoteSectionsAndModulesDelegate(process, callbackSection, callbackModule);
		}

		public IntPtr OpenRemoteProcess(IntPtr pid, ProcessAccess desiredAccess)
		{
			return openRemoteProcessDelegate(pid, desiredAccess);
		}

		public bool IsProcessValid(IntPtr process)
		{
			return isProcessValidDelegate(process);
		}

		public void CloseRemoteProcess(IntPtr process)
		{
			closeRemoteProcessDelegate(process);
		}

		public bool ReadRemoteMemory(IntPtr process, IntPtr address, ref byte[] buffer, int offset, int size)
		{
			return readRemoteMemoryDelegate(process, address, buffer, offset, size);
		}

		public bool WriteRemoteMemory(IntPtr process, IntPtr address, ref byte[] buffer, int offset, int size)
		{
			return writeRemoteMemoryDelegate(process, address, buffer, offset, size);
		}

		public void ControlRemoteProcess(IntPtr process, ControlRemoteProcessAction action)
		{
			controlRemoteProcessDelegate(process, action);
		}

		public bool AttachDebuggerToProcess(IntPtr id)
		{
			return attachDebuggerToProcessDelegate(id);
		}

		public void DetachDebuggerFromProcess(IntPtr id)
		{
			detachDebuggerFromProcessDelegate(id);
		}

		public bool AwaitDebugEvent(ref DebugEvent evt, int timeoutInMilliseconds)
		{
			return awaitDebugEventDelegate(ref evt, timeoutInMilliseconds);
		}

		public void HandleDebugEvent(ref DebugEvent evt)
		{
			handleDebugEventDelegate(ref evt);
		}

		public bool SetHardwareBreakpoint(IntPtr id, IntPtr address, HardwareBreakpointRegister register, HardwareBreakpointTrigger trigger, HardwareBreakpointSize size, bool set)
		{
			return setHardwareBreakpointDelegate(id, address, register, trigger, size, set);
		}
	}
}
