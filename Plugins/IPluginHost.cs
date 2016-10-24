using System;
using System.Drawing;
using System.Resources;
using ReClassNET.DataExchange;
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

		Settings Settings { get; }

		void RegisterNodeInfoReader(INodeInfoReader reader);
		void UnregisterNodeInfoReader(INodeInfoReader reader);

		void RegisterNodeType(Type type, ICustomSchemaConverter converter, string name, Image icon);
		void UnregisterNodeType(Type type, ICustomSchemaConverter converter);
	}
}
