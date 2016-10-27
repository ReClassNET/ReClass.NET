using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Resources;
using ReClassNET.DataExchange;
using ReClassNET.Forms;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET.Plugins
{
	[ContractClass(typeof(IPluginHostContract))]
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

	[ContractClassFor(typeof(IPluginHost))]
	internal abstract class IPluginHostContract : IPluginHost
	{
		public ILogger Logger
		{
			get
			{
				Contract.Ensures(Logger != null);

				throw new NotImplementedException();
			}
		}

		public MainForm MainWindow
		{
			get
			{
				Contract.Ensures(MainWindow != null);

				throw new NotImplementedException();
			}
		}

		public RemoteProcess Process
		{
			get
			{
				Contract.Ensures(Process != null);

				throw new NotImplementedException();
			}
		}

		public ResourceManager Resources
		{
			get
			{
				Contract.Ensures(Resources != null);

				throw new NotImplementedException();
			}
		}

		public Settings Settings
		{
			get
			{
				Contract.Ensures(Settings != null);

				throw new NotImplementedException();
			}
		}

		public void RegisterNodeInfoReader(INodeInfoReader reader)
		{
			Contract.Requires(reader != null);

			throw new NotImplementedException();
		}

		public void RegisterNodeType(Type type, ICustomSchemaConverter converter, string name, Image icon)
		{
			Contract.Requires(type != null);
			Contract.Requires(converter != null);
			Contract.Requires(name != null);

			throw new NotImplementedException();
		}

		public void UnregisterNodeInfoReader(INodeInfoReader reader)
		{
			Contract.Requires(reader != null);

			throw new NotImplementedException();
		}

		public void UnregisterNodeType(Type type, ICustomSchemaConverter converter)
		{
			Contract.Requires(type != null);
			Contract.Requires(converter != null);

			throw new NotImplementedException();
		}
	}
}
