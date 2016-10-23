using System.Resources;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.Plugins
{
	public interface IPluginHost
	{
		MainForm MainWindow { get; }

		ResourceManager Resources { get; }

		RemoteProcess Process { get; }

		ILogger Logger { get; }

		void RegisterNodeInfoReader(INodeInfoReader reader);
		void UnregisterNodeInfoReader(INodeInfoReader reader);
	}
}
