using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Resources;
using ReClassNET.CodeGenerator;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Memory;
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
			BaseNode.NodeInfoReader.Add(reader);
		}

		public void DeregisterNodeInfoReader(INodeInfoReader reader)
		{
			BaseNode.NodeInfoReader.Remove(reader);
		}

		public void RegisterNodeType(Type type, string name, Image icon, ICustomNodeConverter converter, ICustomCodeGenerator generator)
		{
			CustomNodeConvert.RegisterCustomType(converter);
			CustomCodeGenerator.RegisterCustomType(generator);

			MainWindow.RegisterNodeType(type, name, icon ?? Properties.Resources.B16x16_Plugin);
		}

		public void DeregisterNodeType(Type type, ICustomNodeConverter converter, ICustomCodeGenerator generator)
		{
			CustomNodeConvert.DeregisterCustomType(converter);
			CustomCodeGenerator.DeregisterCustomType(generator);

			MainWindow.DeregisterNodeType(type);
		}
	}
}
