using System.Diagnostics.Contracts;
using System.Drawing;

namespace ReClassNET.Plugins
{
	public abstract class Plugin
	{
		public virtual Image Icon => null;

		public virtual bool Initialize(IPluginHost host)
		{
			Contract.Requires(host != null);

			return true;
		}

		public virtual void Terminate()
		{

		}
	}
}
