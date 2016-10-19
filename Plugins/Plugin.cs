using System.Drawing;

namespace ReClassNET.Plugins
{
	public abstract class Plugin
	{
		public virtual Image Icon => null;

		public virtual bool Initialize(IPluginHost host)
		{
			return (host != null);
		}

		public virtual void Terminate()
		{

		}
	}
}
