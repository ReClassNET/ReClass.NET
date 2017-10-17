using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Util.Rtf
{
	partial class RtfBuilder
	{
		private class RtfFormatLock : IDisposable
		{
			private readonly RtfBuilder builder;
			private readonly RtfFormatWrapper wrapped;

			public RtfFormatLock(RtfBuilder builder)
			{
				Contract.Requires(builder != null);

				this.builder = builder;

				wrapped = new RtfFormatWrapper(builder);

				builder.isLocked = true;
			}

			public void Dispose()
			{
				wrapped.Dispose();

				builder.isLocked = false;
			}
		}
	}
}