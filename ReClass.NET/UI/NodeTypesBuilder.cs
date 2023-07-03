using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Controls;
using ReClassNET.Nodes;
using ReClassNET.Plugins;

namespace ReClassNET.UI
{
	internal static class NodeTypesBuilder
	{
		private static readonly List<Type[]> defaultNodeTypeGroupList = new List<Type[]>();
		private static readonly Dictionary<Plugin, IReadOnlyList<Type>> pluginNodeTypes = new Dictionary<Plugin, IReadOnlyList<Type>>();

		static NodeTypesBuilder()
		{
			defaultNodeTypeGroupList.Add(new[] { typeof(Hex64Node), typeof(Hex32Node), typeof(Hex16Node), typeof(Hex8Node) });
			defaultNodeTypeGroupList.Add(new[] { typeof(NIntNode), typeof(Int64Node), typeof(Int32Node), typeof(Int16Node), typeof(Int8Node) });
			defaultNodeTypeGroupList.Add(new[] { typeof(NUIntNode), typeof(UInt64Node), typeof(UInt32Node), typeof(UInt16Node), typeof(UInt8Node) });
			defaultNodeTypeGroupList.Add(new[] { typeof(BoolNode), typeof(BitFieldNode), typeof(EnumNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(FloatNode), typeof(DoubleNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(Vector4Node), typeof(Vector3Node), typeof(Vector2Node), typeof(Matrix4x4Node), typeof(Matrix3x4Node), typeof(Matrix3x3Node) });
			defaultNodeTypeGroupList.Add(new[] { typeof(Utf8TextNode), typeof(Utf8TextPtrNode), typeof(Utf16TextNode), typeof(Utf16TextPtrNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(PointerNode), typeof(ArrayNode), typeof(UnionNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(ClassInstanceNode) });
			defaultNodeTypeGroupList.Add(new[] { typeof(VirtualMethodTableNode), typeof(FunctionNode), typeof(FunctionPtrNode) });
		}

		public static void AddPluginNodeGroup(Plugin plugin, IReadOnlyList<Type> nodeTypes)
		{
			Contract.Requires(plugin != null);
			Contract.Requires(nodeTypes != null);

			if (pluginNodeTypes.ContainsKey(plugin))
			{
				throw new InvalidOperationException(); // TODO
			}

			pluginNodeTypes.Add(plugin, nodeTypes);
		}

		public static void RemovePluginNodeGroup(Plugin plugin)
		{
			Contract.Requires(plugin != null);

			pluginNodeTypes.Remove(plugin);
		}

		public static IEnumerable<ToolStripItem> CreateToolStripButtons(Action<Type> handler)
		{
			Contract.Requires(handler != null);

			var clickHandler = new EventHandler((sender, e) => handler((sender as TypeToolStripButton)?.Value ?? ((TypeToolStripMenuItem)sender).Value));

			return CreateToolStripItems(t =>
			{
				GetNodeInfoFromType(t, out var label, out var icon, out var shortcutKeys);

				var item = new TypeToolStripMenuItem
				{
					Value = t,
					ToolTipText = label,
					DisplayStyle = ToolStripItemDisplayStyle.Image,
					Image = icon,
					ShortcutKeys = shortcutKeys,
				};
				item.Click += clickHandler;
				return item;
			}, p => new ToolStripDropDownButton
			{
				ToolTipText = "",
				Image = p.Icon
			}, t =>
			{
				GetNodeInfoFromType(t, out var label, out var icon, out var shortcutKeys);

				var item = new TypeToolStripMenuItem
				{
					Value = t,
					Text = label,
					Image = icon
				};
				item.Click += clickHandler;
				return item;
			});
		}

		public static IEnumerable<ToolStripItem> CreateToolStripMenuItems(Action<Type> handler, bool addNoneType)
		{
			Contract.Requires(handler != null);

			var clickHandler = new EventHandler((sender, e) => handler(((TypeToolStripMenuItem)sender).Value));

			var items = CreateToolStripItems(t =>
			{
				GetNodeInfoFromType(t, out var label, out var icon, out var shortcutKeys);

				var item = new TypeToolStripMenuItem
				{
					Value = t,
					Text = label,
					Image = icon,
					ShortcutKeys = shortcutKeys,
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

				items = items.Prepend(new ToolStripSeparator()).Prepend(noneItem);
			}

			return items;
		}

		private static IEnumerable<ToolStripItem> CreateToolStripItems(Func<Type, ToolStripItem> createItem, Func<Plugin, ToolStripDropDownItem> createPluginContainerItem)
		{
			Contract.Requires(createItem != null);
			Contract.Requires(createPluginContainerItem != null);

			return CreateToolStripItems(createItem, createPluginContainerItem, createItem);
		}

		private static IEnumerable<ToolStripItem> CreateToolStripItems(Func<Type, ToolStripItem> createItem, Func<Plugin, ToolStripDropDownItem> createPluginContainerItem, Func<Type, ToolStripItem> createPluginItem)
		{
			Contract.Requires(createItem != null);
			Contract.Requires(createPluginContainerItem != null);
			Contract.Requires(createPluginItem != null);

			if (!defaultNodeTypeGroupList.Any())
			{
				return Enumerable.Empty<ToolStripItem>();
			}

			var items = defaultNodeTypeGroupList
				.Select(t => t.Select(createItem))
				.Aggregate((l1, l2) => l1.Append(new ToolStripSeparator()).Concat(l2));

			if (pluginNodeTypes.Any())
			{
				foreach (var kv in pluginNodeTypes)
				{
					var pluginContainerItem = createPluginContainerItem(kv.Key);
					pluginContainerItem.Tag = kv.Key;
					pluginContainerItem.DropDownItems.AddRange(
						kv.Value
							.Select(createPluginItem)
							.ToArray()
					);
					items = items.Append(new ToolStripSeparator()).Append(pluginContainerItem);
				}
			}

			return items;
		}

		private static void GetNodeInfoFromType(Type nodeType, out string label, out Image icon, out Keys shortcutKeys)
		{
			Contract.Requires(nodeType != null);

			shortcutKeys = Program.Settings.GetShortcutKeyForNodeType(nodeType);

			var node = BaseNode.CreateInstanceFromType(nodeType, false);
			if (node == null)
			{
				throw new InvalidOperationException($"'{nodeType}' is not a valid node type.");
			}

			node.GetUserInterfaceInfo(out label, out icon);
		}
	}
}
