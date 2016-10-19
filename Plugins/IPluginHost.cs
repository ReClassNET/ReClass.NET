using System.Resources;

namespace ReClassNET.Plugins
{
	public interface IPluginHost
	{
		MainForm MainWindow { get; }

		ResourceManager Resources { get; }

		RemoteProcess Process { get; }
	}
}
