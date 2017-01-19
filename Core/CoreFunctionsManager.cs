using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using ReClassNET.Debugger;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.Util;

namespace ReClassNET.Core
{
	public class CoreFunctionsManager : IDisposable
	{
		private const string CoreFunctionsModuleWindows = "NativeCore.dll";
		private const string CoreFunctionsModuleUnix = "NativeCore.so";

		private readonly Dictionary<string, ICoreProcessFunctions> functionsRegistry = new Dictionary<string, ICoreProcessFunctions>();

		public IEnumerable<string> FunctionProviders => functionsRegistry.Keys;

		private IntPtr internalCoreFunctionsHandle;

		private readonly InternalCoreFunctions internalCoreFunctions;

		private ICoreProcessFunctions currentFunctions;

		public CoreFunctionsManager()
		{
			var libraryName = NativeMethods.IsUnix() ? CoreFunctionsModuleUnix : CoreFunctionsModuleWindows;

			internalCoreFunctionsHandle = NativeMethods.LoadLibrary("./" + libraryName);

			if (internalCoreFunctionsHandle.IsNull())
			{
				throw new FileNotFoundException(libraryName);
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

		public void EnumerateProcesses(Action<ProcessInfo> callbackProcess)
		{
			var c = callbackProcess == null ? null : (EnumerateProcessCallback)delegate (ref EnumerateProcessData data)
			{
				callbackProcess(new ProcessInfo(data.Id, data.Path));
			};

			currentFunctions.EnumerateProcesses(c);
		}

		public void EnumerateRemoteSectionsAndModules(IntPtr process, Action<Section> callbackSection, Action<Module> callbackModule)
		{
			var c1 = callbackSection == null ? null : (EnumerateRemoteSectionCallback)delegate (ref EnumerateRemoteSectionData data)
			{
				callbackSection(new Section
				{
					Start = data.BaseAddress,
					End = data.BaseAddress.Add(data.Size),
					Size = data.Size,
					Name = data.Name,
					Protection = data.Protection,
					Type = data.Type,
					ModulePath = data.ModulePath,
					ModuleName = Path.GetFileName(data.ModulePath),
					Category = data.Category
				});
			};

			var c2 = callbackModule == null ? null : (EnumerateRemoteModuleCallback)delegate (ref EnumerateRemoteModuleData data)
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

			currentFunctions.EnumerateRemoteSectionsAndModules(process, c1, c2);
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
