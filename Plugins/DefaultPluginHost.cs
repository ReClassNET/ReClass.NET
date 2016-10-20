using System.Diagnostics.Contracts;
using System.Resources;
using ReClassNET.Gui;

namespace ReClassNET.Plugins
{
	internal sealed class DefaultPluginHost : IPluginHost
	{
		public MainForm MainWindow { get; }

		public ResourceManager Resources => Properties.Resources.ResourceManager;

		public RemoteProcess Process { get; }

		public DefaultPluginHost(MainForm form, RemoteProcess process)
		{
			Contract.Requires(form != null);
			Contract.Requires(process != null);

			MainWindow = form;

			Process = process;
		}
	}
}
