using System.Resources;
using ReClassNET.Gui;

namespace ReClassNET.Plugins
{
	public interface IPluginHost
	{
		MainForm MainWindow { get; }

		ResourceManager Resources { get; }

		RemoteProcess Process { get; }
	}
}
