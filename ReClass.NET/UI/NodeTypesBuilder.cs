using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Extensions;
using ReClassNET.Nodes;
using ReClassNET.Plugins;

namespace ReClassNET.UI
{
	public static class NodeTypesBuilder
	{
		private static readonly List<Type[]> defaultNodeTypeGroupList = new List<Type[]>();
		private static readonly Dictionary<Plugin, List<Type>> pluginNodeTypes = new Dictionary<Plugin, List<Type>>();

		static NodeTypesBuilder()
		{
			defaultNodeTypeGroupList.Add(new[] { typeof(Hex64Node), typeof(Hex32Node), typeof(Hex16Node), typeof(Hex8Node) });
			defaultNodeTypeGroupList.Add(new[] { typeof(Int64Node), typeof(Int32Node), typeof(Int16Node), typeof(Int8Node) });
			defaultNodeTypeGroupList.Add(new[] { typeof(UInt64Node), typeof(UInt32Node), typeof(UInt16Node), typeof(UInt8Node) });
			defaultNodeTypeGroupList.Add(new[] { typeof(BoolNode), typeof(BitFieldNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(FloatNode), typeof(DoubleNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(Vector4Node), typeof(Vector3Node), typeof(Vector2Node), typeof(Matrix4x4Node), typeof(Matrix3x4Node), typeof(Matrix3x3Node) });
			defaultNodeTypeGroupList.Add(new[] { typeof(Utf8TextNode), typeof(Utf8TextPtrNode), typeof(Utf16TextNode), typeof(Utf16TextPtrNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(PointerNode), typeof(ArrayNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(ClassInstanceNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(VTableNode), typeof(FunctionNode), typeof(FunctionPtrNode) });
		}

		public static List<Type> RegisterPluginNodeGroup(Plugin plugin)
		{
			Contract.Requires(plugin != null);

			if (!pluginNodeTypes.TryGetValue(plugin, out var types))
			{
				types = new List<Type>();

				pluginNodeTypes.Add(plugin, types);
			}

			return types;
		}

		public static IEnumerable<ToolStripItem> CreateToolStripButtons(Action<Type> handler)
		{
			Contract.Requires(handler != null);

			var clickHandler = new EventHandler((sender, e) => handler(((TypeToolStripButton)sender).Value));

			return CreateToolStripItems(t =>
			{
				GetNodeInfoFromType(t, out var label, out var icon);

				var item = new TypeToolStripButton
				{
					Value = t,
					ToolTipText = label,
					DisplayStyle = ToolStripItemDisplayStyle.Image,
					Image = icon
				};
				item.Click += clickHandler;
				return item;
			}, p => new ToolStripDropDownButton
			{
				ToolTipText = "",
				Image = p.Icon
			});
		}

		public static IEnumerable<ToolStripItem> CreateToolStripMenuItems(Action<Type> handler)
		{
			Contract.Requires(handler != null);

			return CreateToolStripMenuItems(handler, true);
		}

		public static IEnumerable<ToolStripItem> CreateToolStripMenuItems(Action<Type> handler, bool addNoneType)
		{
			Contract.Requires(handler != null);

			var clickHandler = new EventHandler((sender, e) => handler(((TypeToolStripMenuItem)sender).Value));

			var items = CreateToolStripItems(t =>
			{
				GetNodeInfoFromType(t, out var label, out var icon);

				var item = new TypeToolStripMenuItem
				{
					Value = t,
					Text = label,
					Image = icon
				};
				item.Click += clickHandler;
				return item;
			}, p => new ToolStripMenuItem
			{
				Text = p.GetType().ToString(),
				Image = p.Icon
			});

			if (addNoneType)
			{
				ToolStripItem noneItem = new TypeToolStripMenuItem
				{
					Value = null,
					Text = "None"
				};

				items = noneItem.Yield().Append(new ToolStripSeparator()).Concat(items);
			}

			return items;
		}

		private static IEnumerable<ToolStripItem> CreateToolStripItems(Func<Type, ToolStripItem> createItem, Func<Plugin, ToolStripDropDownItem> createPluginContainerItem)
		{
			Contract.Requires(createItem != null);

			if (!defaultNodeTypeGroupList.Any())
			{
				return new ToolStripItem[0];
			}

			var items = defaultNodeTypeGroupList
				.Select(t => t.Select(createItem))
				.Aggregate((l1, l2) => l1.Append(new ToolStripSeparator()).Concat(l2));

			if (pluginNodeTypes.Any())
			{
				foreach (var kv in pluginNodeTypes)
				{
					var pluginContainerItem = createPluginContainerItem(kv.Key);
					pluginContainerItem.DropDownItems.AddRange(
						kv.Value
							.Select(createItem)
							.ToArray()
					);
					items = items.Append(new ToolStripSeparator()).Append(pluginContainerItem);
				}
			}

			return items;
		}

		private static void GetNodeInfoFromType(Type nodeType, out string label, out Image icon)
		{
			Contract.Requires(nodeType != null);

			var node = BaseNode.CreateInstanceFromType(nodeType);
			if (node == null)
			{
				throw new InvalidOperationException($"'{nodeType}' is not a valid node type.");
			}

			node.GetUserInterfaceInfo(out label, out icon);
		}
	}
}
