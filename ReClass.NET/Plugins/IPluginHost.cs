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
	[ContractClass(typeof(PluginHostContract))]
	public interface IPluginHost
	{
		/// <summary>Gets the main window of ReClass.NET.</summary>
		MainForm MainWindow { get; }

		/// <summary>Gets the resources of ReClass.NET.</summary>
		ResourceManager Resources { get; }

		/// <summary>Gets the process ReClass.NET is attached to.</summary>
		RemoteProcess Process { get; }

		/// <summary>Gets the logger ReClass.NET is using.</summary>
		ILogger Logger { get; }

		/// <summary>Gets the settings ReClass.NET is using.</summary>
		Settings Settings { get; }

		/// <summary>Registers a node information reader to display custom data on nodes.</summary>
		/// <param name="reader">The node information reader.</param>
		void RegisterNodeInfoReader(INodeInfoReader reader);

		/// <summary>Unregisters the node information reader.</summary>
		/// <param name="reader">The node information reader.</param>
		void DeregisterNodeInfoReader(INodeInfoReader reader);

		/// <summary>Registers a new node type.</summary>
		/// <param name="type">The type of the node.</param>
		/// <param name="name">The name of the type.</param>
		/// <param name="icon">The icon of the type (may be null).</param>
		/// <param name="converter">The converter used to serialize the node.</param>
		/// <param name="generator">The generator used to generate code from the node.</param>
		void RegisterNodeType(Type type, string name, Image icon, ICustomNodeConverter converter, ICustomCodeGenerator generator);

		/// <summary>Unregisters a node type.</summary>
		/// <param name="type">The type of the node.</param>
		/// <param name="converter">The converter used to serialize the node.</param>
		/// <param name="generator">The generator used to generate code from the node.</param>
		void DeregisterNodeType(Type type, ICustomNodeConverter converter, ICustomCodeGenerator generator);
	}

	[ContractClassFor(typeof(IPluginHost))]
	internal abstract class PluginHostContract : IPluginHost
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

		public void RegisterNodeType(Type type, string name, Image icon, ICustomNodeConverter converter, ICustomCodeGenerator generator)
		{
			Contract.Requires(type != null);
			Contract.Requires(name != null);
			Contract.Requires(converter != null);
			Contract.Requires(generator != null);

			throw new NotImplementedException();
		}

		public void DeregisterNodeInfoReader(INodeInfoReader reader)
		{
			Contract.Requires(reader != null);

			throw new NotImplementedException();
		}

		public void DeregisterNodeType(Type type, ICustomNodeConverter converter, ICustomCodeGenerator generator)
		{
			Contract.Requires(type != null);
			Contract.Requires(converter != null);
			Contract.Requires(generator != null);

			throw new NotImplementedException();
		}
	}
}
