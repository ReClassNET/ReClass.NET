using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Debugger;
using ReClassNET.Extensions;
using ReClassNET.Memory;

namespace ReClassNET.Core
{
	public class CoreFunctionsManager : IDisposable
	{
		private readonly Dictionary<string, ICoreProcessFunctions> functionsRegistry = new Dictionary<string, ICoreProcessFunctions>();

		private readonly InternalCoreFunctions internalCoreFunctions;

		private ICoreProcessFunctions currentFunctions;

		public IEnumerable<string> FunctionProviders => functionsRegistry.Keys;

		public ICoreProcessFunctions CurrentFunctions => currentFunctions;

		public string CurrentFunctionsProvider => functionsRegistry
			.Where(kv => kv.Value == currentFunctions)
			.Select(kv => kv.Key)
			.FirstOrDefault();

		public CoreFunctionsManager()
		{
			internalCoreFunctions = InternalCoreFunctions.Create();

			RegisterFunctions("Default", internalCoreFunctions);

			currentFunctions = internalCoreFunctions;
		}

		#region IDisposable Support

		public void Dispose()
		{
			internalCoreFunctions.Dispose();
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
			if (!functionsRegistry.TryGetValue(provider, out var functions))
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
				callbackProcess(new ProcessInfo(data.Id, data.Name, data.Path));
			};

			currentFunctions.EnumerateProcesses(c);
		}

		public IList<ProcessInfo> EnumerateProcesses()
		{
			var processes = new List<ProcessInfo>();
			EnumerateProcesses(processes.Add);
			return processes;
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

		public void EnumerateRemoteSectionsAndModules(IntPtr process, out List<Section> sections, out List<Module> modules)
		{
			sections = new List<Section>();
			modules = new List<Module>();

			EnumerateRemoteSectionsAndModules(process, sections.Add, modules.Add);
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

		public bool DisassembleCode(IntPtr address, int length, IntPtr virtualAddress, bool determineStaticInstructionBytes, EnumerateInstructionCallback callback)
		{
			return internalCoreFunctions.DisassembleCode(address, length, virtualAddress, determineStaticInstructionBytes, callback);
		}

		public IntPtr InitializeInput()
		{
			return internalCoreFunctions.InitializeInput();
		}

		public Keys[] GetPressedKeys(IntPtr handle)
		{
			return internalCoreFunctions.GetPressedKeys(handle);
		}

		public void ReleaseInput(IntPtr handle)
		{
			internalCoreFunctions.ReleaseInput(handle);
		}

		#endregion
	}
}
