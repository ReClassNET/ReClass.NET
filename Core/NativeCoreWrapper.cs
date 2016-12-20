using System;
using System.IO;
using System.Runtime.InteropServices;
using ReClassNET.Debugger;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.Util;

namespace ReClassNET.Core
{
	public class NativeCoreWrapper : ICoreProcessFunctions
	{
		#region Native Delegates

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		private struct EnumerateProcessData
		{
			public IntPtr Id;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string Path;
		};

		private delegate void EnumerateProcessCallback(ref EnumerateProcessData data);
		private delegate void EnumerateProcessesDelegate([MarshalAs(UnmanagedType.FunctionPtr)] EnumerateProcessCallback callbackProcess);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		private struct EnumerateRemoteSectionData
		{
			public IntPtr BaseAddress;

			public IntPtr Size;

			public SectionType Type;

			public SectionProtection Protection;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
			public string Name;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string ModulePath;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
		private struct EnumerateRemoteModuleData
		{
			public IntPtr BaseAddress;

			public IntPtr Size;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string Path;
		}

		private delegate void EnumerateRemoteSectionCallback(ref EnumerateRemoteSectionData data);
		private delegate void EnumerateRemoteModuleCallback(ref EnumerateRemoteModuleData data);
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

		private EnumerateProcessesDelegate enumerateProcessesDelegate;
		private EnumerateRemoteSectionsAndModulesDelegate enumerateRemoteSectionsAndModulesDelegate;
		private OpenRemoteProcessDelegate openRemoteProcessDelegate;
		private IsProcessValidDelegate isProcessValidDelegate;
		private CloseRemoteProcessDelegate closeRemoteProcessDelegate;
		private ReadRemoteMemoryDelegate readRemoteMemoryDelegate;
		private WriteRemoteMemoryDelegate writeRemoteMemoryDelegate;
		private ControlRemoteProcessDelegate controlRemoteProcessDelegate;
		private AttachDebuggerToProcessDelegate attachDebuggerToProcessDelegate;
		private DetachDebuggerFromProcessDelegate detachDebuggerFromProcessDelegate;
		private AwaitDebugEventDelegate awaitDebugEventDelegate;
		private HandleDebugEventDelegate handleDebugEventDelegate;
		private SetHardwareBreakpointDelegate setHardwareBreakpointDelegate;

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
				throw new Exception();
			}
			return Marshal.GetDelegateForFunctionPointer<TDelegate>(address);
		}

		public void EnumerateProcesses(Action<Tuple<IntPtr, string>> callbackProcess)
		{
			var c = callbackProcess == null ? null : (EnumerateProcessCallback)delegate (ref EnumerateProcessData data)
			{
				callbackProcess(Tuple.Create(data.Id, data.Path));
			};

			enumerateProcessesDelegate(c);
		}

		public void EnumerateRemoteSectionsAndModules(IntPtr process, Action<Section> callbackSection, Action<Module> callbackModule)
		{
			var c1 = callbackSection == null ? null : (EnumerateRemoteSectionCallback)delegate (ref EnumerateRemoteSectionData data)
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

			enumerateRemoteSectionsAndModulesDelegate(process, c1, c2);
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
