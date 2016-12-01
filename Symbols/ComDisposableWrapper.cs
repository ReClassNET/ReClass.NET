using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace ReClassNET.Symbols
{
	class DisposableWrapper : IDisposable
	{
		protected object obj;

		[ContractInvariantMethod]
		private void ObjectInvariants()
		{
			Contract.Invariant(obj != null);
		}

		public DisposableWrapper(object obj)
		{
			Contract.Requires(obj != null);

			this.obj = obj;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Marshal.ReleaseComObject(obj);
			}
		}

		~DisposableWrapper()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}
	}

	class ComDisposableWrapper<T> : DisposableWrapper
	{
		public T Interface => (T)obj;

		public ComDisposableWrapper(T com)
			: base(com)
		{
			Contract.Requires(com != null);
		}
	}
}
