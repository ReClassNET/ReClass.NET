using System.Diagnostics.Contracts;
using System.Resources;
using ReClassNET.Gui;
using ReClassNET.Nodes;

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

		public void RegisterGetNodeInfoCallback(GetNodeInfoCallback callback)
		{
			if (callback != null)
			{
				BaseNode.GetNodeInfoCallbacks.Add(callback);
			}
		}

		public void UnregisterGetNodeInfoCallback(GetNodeInfoCallback callback)
		{
			if (callback != null)
			{
				BaseNode.GetNodeInfoCallbacks.Remove(callback);
			}
		}
	}
}
