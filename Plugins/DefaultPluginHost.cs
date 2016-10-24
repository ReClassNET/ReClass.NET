using System.Diagnostics.Contracts;
using System.Resources;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Nodes;

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

		public void RegisterNodeInfoReader(INodeInfoReader reader)
		{
			if (reader != null)
			{
				BaseNode.NodeInfoReader.Add(reader);
			}
		}

		public void UnregisterNodeInfoReader(INodeInfoReader reader)
		{
			if (reader != null)
			{
				BaseNode.NodeInfoReader.Remove(reader);
			}
		}
	}
}
