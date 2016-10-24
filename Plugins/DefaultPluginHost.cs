using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Resources;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.DataExchange;

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
			if (reader == null) throw new ArgumentNullException(nameof(reader));

			BaseNode.NodeInfoReader.Add(reader);
		}

		public void UnregisterNodeInfoReader(INodeInfoReader reader)
		{
			if (reader == null) throw new ArgumentNullException(nameof(reader));
			
			BaseNode.NodeInfoReader.Remove(reader);
		}

		public void RegisterNodeType(Type type, ICustomSchemaConverter converter, string name, Image icon)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (converter == null) throw new ArgumentNullException(nameof(converter));

			CustomSchemaConvert.RegisterCustomType(converter);
			MainWindow.AddNodeType(type, name, icon);
		}

		public void UnregisterNodeType(Type type, ICustomSchemaConverter converter)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (converter == null) throw new ArgumentNullException(nameof(converter));

			CustomSchemaConvert.UnregisterCustomType(converter);
			MainWindow.RemoveNodeType(type);
		}
	}
}
