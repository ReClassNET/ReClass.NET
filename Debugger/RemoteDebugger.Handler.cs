using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReClassNET.Native;

namespace ReClassNET.Debugger
{
	public partial class RemoteDebugger
	{
		private bool HandleEvent(ref DebugEvent evt)
		{
			switch (evt.Header.Type)
			{
				case DebugEventType.CreateProcess:
					return HandleCreateProcessEvent(ref evt);
				case DebugEventType.ExitProcess:
					return HandleExitProcessEvent(ref evt);
				case DebugEventType.CreateThread:
					return HandleCreateThreadEvent(ref evt);
				case DebugEventType.ExitThread:
					return HandleExitThreadEvent(ref evt);
				case DebugEventType.LoadDll:
					return HandleLoadDllEvent(ref evt);
				case DebugEventType.UnloadDll:
					return HandleUnloadDllEvent(ref evt);
				case DebugEventType.Exception:
					return HandleExceptionEvent(ref evt);
			}

			return true;
		}

		private bool HandleCreateProcessEvent(ref DebugEvent evt)
		{
			return true;
		}

		private bool HandleExitProcessEvent(ref DebugEvent evt)
		{
			return false;
		}

		private bool HandleCreateThreadEvent(ref DebugEvent evt)
		{
			return true;
		}

		private bool HandleExitThreadEvent(ref DebugEvent evt)
		{
			return true;
		}

		private bool HandleLoadDllEvent(ref DebugEvent evt)
		{
			return true;
		}

		private bool HandleUnloadDllEvent(ref DebugEvent evt)
		{
			return true;
		}

		private bool HandleExceptionEvent(ref DebugEvent evt)
		{
			IBreakpoint current = null;
			lock (syncBreakpoint)
			{
				var causedBy = evt.Data.ExceptionInfo.CausedBy;

				foreach (var bp in breakpoints)
				{
					var hwbp = bp as HardwareBreakpoint;
					if (hwbp != null)
					{
						if (causedBy == hwbp.Register)
						{
							current = bp;
							break;
						}
					}
				}
			}

			if (current != null)
			{
				current.Handler(ref evt);
			}

			return true;
		}
	}
}
