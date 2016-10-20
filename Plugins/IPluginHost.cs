using System.Resources;
using ReClassNET.Gui;
using ReClassNET.Nodes;

namespace ReClassNET.Plugins
{
	public interface IPluginHost
	{
		MainForm MainWindow { get; }

		ResourceManager Resources { get; }

		RemoteProcess Process { get; }

		void RegisterGetNodeInfoCallback(GetNodeInfoCallback callback);
		void UnregisterGetNodeInfoCallback(GetNodeInfoCallback callback);
	}
}
