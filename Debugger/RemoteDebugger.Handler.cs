namespace ReClassNET.Debugger
{
	public partial class RemoteDebugger
	{
		private void HandleExceptionEvent(ref DebugEvent evt)
		{
			IBreakpoint current = null;
			lock (syncBreakpoint)
			{
				var causedBy = evt.ExceptionInfo.CausedBy;

				foreach (var bp in breakpoints)
				{
					var hwbp = bp as HardwareBreakpoint;
					if (hwbp != null)
					{
						if (causedBy == hwbp.Register)
						{
							current.Handler(ref evt);

							break;
						}
					}
				}
			}
		}
	}
}
