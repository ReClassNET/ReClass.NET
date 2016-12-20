using System;
using ReClassNET.Debugger;
using ReClassNET.Memory;
using ReClassNET.Native;

namespace ReClassNET.Core
{
	public interface ICoreProcessFunctions
	{
		void EnumerateProcesses(Action<Tuple<IntPtr, string>> callbackProcess);

		void EnumerateRemoteSectionsAndModules(IntPtr process, Action<Section> callbackSection, Action<Module> callbackModule);

		IntPtr OpenRemoteProcess(IntPtr pid, ProcessAccess desiredAccess);

		bool IsProcessValid(IntPtr process);

		void CloseRemoteProcess(IntPtr process);

		bool ReadRemoteMemory(IntPtr process, IntPtr address, ref byte[] buffer, int offset, int size);

		bool WriteRemoteMemory(IntPtr process, IntPtr address, ref byte[] buffer, int offset, int size);

		void ControlRemoteProcess(IntPtr process, ControlRemoteProcessAction action);

		bool AttachDebuggerToProcess(IntPtr id);

		void DetachDebuggerFromProcess(IntPtr id);

		bool AwaitDebugEvent(ref DebugEvent evt, int timeoutInMilliseconds);

		void HandleDebugEvent(ref DebugEvent evt);

		bool SetHardwareBreakpoint(IntPtr id, IntPtr address, HardwareBreakpointRegister register, HardwareBreakpointTrigger trigger, HardwareBreakpointSize size, bool set);
	}
}
