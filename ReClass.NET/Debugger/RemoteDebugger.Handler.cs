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
					if (bp is HardwareBreakpoint hwbp && hwbp.Register == causedBy)
					{
						hwbp.Handler(ref evt);

						break;
					}
				}
			}
		}
	}
}
