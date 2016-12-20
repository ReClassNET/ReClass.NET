using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Debugger;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.Util;

namespace ReClassNET.Core
{
	public class CoreFunctionsManager : IDisposable
	{
		private const string CoreFunctionsModuleWindows = "NativeHelper.dll";
		private const string CoreFunctionsModuleUnix = "NativeHelper.so";

		private readonly Dictionary<string, ICoreProcessFunctions> functionsRegistry = new Dictionary<string, ICoreProcessFunctions>();

		public IEnumerable<string> FunctionProviders => functionsRegistry.Keys;

		private IntPtr internalCoreFunctionsHandle;

		private readonly InternalCoreFunctions internalCoreFunctions;

		private ICoreProcessFunctions currentFunctions;

		public CoreFunctionsManager()
		{
			internalCoreFunctionsHandle = NativeMethods.LoadLibrary(
				NativeMethods.IsUnix()
				? CoreFunctionsModuleUnix
				: CoreFunctionsModuleWindows
			);

			if (internalCoreFunctionsHandle.IsNull())
			{
				throw new Exception();
			}

			internalCoreFunctions = new InternalCoreFunctions(internalCoreFunctionsHandle);

			RegisterFunctions("Default", internalCoreFunctions);

			currentFunctions = internalCoreFunctions;
		}

		#region IDisposable Support

		~CoreFunctionsManager()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!internalCoreFunctionsHandle.IsNull())
			{
				NativeMethods.FreeLibrary(internalCoreFunctionsHandle);

				internalCoreFunctionsHandle = IntPtr.Zero;
			}
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion

		public void RegisterFunctions(string provider, ICoreProcessFunctions functions)
		{
			Contract.Requires(provider != null);
			Contract.Requires(functions != null);

			functionsRegistry.Add(provider, functions);
		}

		public void SetActiveFunctionsProvider(string provider)
		{
			ICoreProcessFunctions functions;
			if (!functionsRegistry.TryGetValue(provider, out functions))
			{
				throw new ArgumentException();
			}

			currentFunctions = functions;
		}

		#region Plugin Functions

		public void EnumerateProcesses(Action<Tuple<IntPtr, string>> callbackProcess)
		{
			currentFunctions.EnumerateProcesses(callbackProcess);
		}

		public void EnumerateRemoteSectionsAndModules(IntPtr process, Action<Section> callbackSection, Action<Module> callbackModule)
		{
			currentFunctions.EnumerateRemoteSectionsAndModules(process, callbackSection, callbackModule);
		}

		public IntPtr OpenRemoteProcess(IntPtr pid, ProcessAccess desiredAccess)
		{
			return currentFunctions.OpenRemoteProcess(pid, desiredAccess);
		}

		public bool IsProcessValid(IntPtr process)
		{
			return currentFunctions.IsProcessValid(process);
		}

		public void CloseRemoteProcess(IntPtr process)
		{
			currentFunctions.CloseRemoteProcess(process);
		}

		public bool ReadRemoteMemory(IntPtr process, IntPtr address, ref byte[] buffer, int offset, int size)
		{
			return currentFunctions.ReadRemoteMemory(process, address, ref buffer, offset, size);
		}

		public bool WriteRemoteMemory(IntPtr process, IntPtr address, ref byte[] buffer, int offset, int size)
		{
			return currentFunctions.WriteRemoteMemory(process, address, ref buffer, offset, size);
		}

		public void ControlRemoteProcess(IntPtr process, ControlRemoteProcessAction action)
		{
			currentFunctions.ControlRemoteProcess(process, action);
		}

		public bool AttachDebuggerToProcess(IntPtr id)
		{
			return currentFunctions.AttachDebuggerToProcess(id);
		}

		public void DetachDebuggerFromProcess(IntPtr id)
		{
			currentFunctions.DetachDebuggerFromProcess(id);
		}

		public void HandleDebugEvent(ref DebugEvent evt)
		{
			currentFunctions.HandleDebugEvent(ref evt);
		}

		public bool AwaitDebugEvent(ref DebugEvent evt, int timeoutInMilliseconds)
		{
			return currentFunctions.AwaitDebugEvent(ref evt, timeoutInMilliseconds);
		}

		public bool SetHardwareBreakpoint(IntPtr id, IntPtr address, HardwareBreakpointRegister register, HardwareBreakpointTrigger trigger, HardwareBreakpointSize size, bool set)
		{
			return currentFunctions.SetHardwareBreakpoint(id, address, register, trigger, size, set);
		}

		#endregion

		#region Internal Core Functions

		public bool DisassembleCode(IntPtr address, int length, IntPtr virtualAddress, out InstructionData instruction)
		{
			return internalCoreFunctions.DisassembleCode(address, length, virtualAddress, out instruction);
		}

		#endregion
	}
}
