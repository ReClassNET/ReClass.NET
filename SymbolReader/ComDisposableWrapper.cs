using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET.SymbolReader
{
	class DisposableWrapper : IDisposable
	{
		protected object obj;

		public DisposableWrapper(object obj)
		{
			Contract.Requires(obj != null);

			this.obj = obj;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (obj != null)
			{
				Marshal.ReleaseComObject(obj);

				obj = null;
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

		}
	}
}
