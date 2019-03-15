using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.CodeGenerator;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Nodes;

namespace ReClassNET.Plugins
{
	public class Plugin
	{
		public class CustomNodeTypes
		{
			/// <summary>
			/// A list with custom node types.
			/// </summary>
			public IReadOnlyList<Type> NodeTypes { get; set; }

			/// <summary>
			/// An instance of a serializer which can process the custom node types.
			/// </summary>
			public ICustomNodeSerializer Serializer { get; set; }

			/// <summary>
			/// An instance of a code generator which can process the custom node types.
			/// </summary>
			public CustomCppCodeGenerator CodeGenerator { get; set; }
		}

		/// <summary>
		/// The icon of the plugin.
		/// </summary>
		public virtual Image Icon => null;

		public virtual bool Initialize(IPluginHost host)
		{
			Contract.Requires(host != null);

			return true;
		}

		public virtual void Terminate()
		{

		}

		/// <summary>
		/// Gets called once to receive all node info readers the plugin provides.
		/// </summary>
		/// <returns>A list with node info readers or <c>null</c> if the plugin provides none.</returns>
		public virtual IReadOnlyList<INodeInfoReader> GetNodeInfoReaders()
		{
			return null;
		}

		/// <summary>
		/// Gets called once to receive all custom node types the plugin provides.
		/// </summary>
		/// <returns>Informations about the custom nodes or <c>null</c> if the plugin provides none.</returns>
		public virtual CustomNodeTypes GetCustomNodeTypes()
		{
			return null;
		}
	}
}
