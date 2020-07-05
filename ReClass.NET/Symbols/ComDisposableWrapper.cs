using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace ReClassNET.Symbols
{
	class DisposableWrapper : IDisposable
	{
		protected object Object;

		[ContractInvariantMethod]
		private void ObjectInvariants()
		{
			Contract.Invariant(Object != null);
		}

		public DisposableWrapper(object obj)
		{
			Contract.Requires(obj != null);

			this.Object = obj;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				Marshal.ReleaseComObject(Object);
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
		public T Interface => (T)Object;

		public ComDisposableWrapper(T com)
			: base(com)
		{
			Contract.Requires(com != null);
		}
	}
}
