using System;
using ReClassNET.Debugger;
using ReClassNET.Nodes;

namespace ReClassNET.UI
{
	public class LinkedWindowFeatures
	{
		public static ClassNode CreateClassAtAddress(IntPtr address, bool addDefaultBytes)
		{
			var classView = Program.MainForm.ClassView;

			var node = ClassNode.Create();
			node.Address = address;
			if (addDefaultBytes)
			{
				node.AddBytes(64);
			}

			classView.SelectedClass = node;

			return node;
		}

		public static ClassNode CreateDefaultClass()
		{
			var address = ClassNode.DefaultAddress;

			var mainModule = Program.RemoteProcess.GetModuleByName(Program.RemoteProcess.UnderlayingProcess?.Name);
			if (mainModule != null)
			{
				address = mainModule.Start;
			}

			return CreateClassAtAddress(address, true);
		}

		public static void SetCurrentClassAddress(IntPtr address)
		{
			var classNode = Program.MainForm.ClassView.SelectedClass;
			if (classNode == null)
			{
				return;
			}

			classNode.Address = address;
		}

		public static void FindWhatInteractsWithAddress(IntPtr address, int size, bool writeOnly)
		{
			var debugger = Program.RemoteProcess.Debugger;

			if (!debugger.AskUserAndAttachDebugger())
			{
				return;
			}

			if (writeOnly)
			{
				debugger.FindWhatWritesToAddress(address, size);
			}
			else
			{
				debugger.FindWhatAccessesAddress(address, size);
			}
		}
	}
}
