using System.Diagnostics.Contracts;
using System.Resources;

namespace ReClassNET.Plugins
{
	internal sealed class DefaultPluginHost : IPluginHost
	{
		public MainForm MainWindow { get; }

		public ResourceManager Resources => Properties.Resources.ResourceManager;

		public DefaultPluginHost(MainForm form)
		{
			Contract.Requires(form != null);

			MainWindow = form;
		}
	}
}
