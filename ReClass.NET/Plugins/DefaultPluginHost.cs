using System.Diagnostics.Contracts;
using System.Resources;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Memory;

namespace ReClassNET.Plugins
{
	internal sealed class DefaultPluginHost : IPluginHost
	{
		public MainForm MainWindow { get; }

		public ResourceManager Resources => Properties.Resources.ResourceManager;

		public RemoteProcess Process { get; }

		public ILogger Logger { get; }

		public Settings Settings => Program.Settings;

		public DefaultPluginHost(MainForm form, RemoteProcess process, ILogger logger)
		{
			Contract.Requires(form != null);
			Contract.Requires(process != null);
			Contract.Requires(logger != null);

			MainWindow = form;
			Process = process;
			Logger = logger;
		}
	}
}
