namespace ReClassNET.Debugger
{
	public partial class RemoteDebugger
	{
		private void HandleExceptionEvent(ref DebugEvent evt)
		{
			lock (syncBreakpoint)
			{
				var causedBy = evt.ExceptionInfo.CausedBy;

				foreach (var bp in breakpoints)
				{
					var hwbp = bp as HardwareBreakpoint;
					if (hwbp?.Register == causedBy)
					{
						hwbp.Handler(ref evt);

						break;
					}
				}
			}
		}
	}
}
