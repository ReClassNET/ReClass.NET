using ReClassNET.Gui;
using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET
{
	partial class MainForm : Form
	{
		private readonly NativeHelper nativeHelper;

		RemoteProcess remoteProcess;
		Memory memory = new Memory();
		Settings settings = new Settings();

		public MainForm(NativeHelper nativeHelper)
		{
			Contract.Requires(nativeHelper != null);

			this.nativeHelper = nativeHelper;

			InitializeComponent();

			mainMenuStrip.Renderer = new CustomToolStripProfessionalRenderer(true);
			toolStrip.Renderer = new CustomToolStripProfessionalRenderer(false);

			remoteProcess = new RemoteProcess(nativeHelper);
			remoteProcess.ProcessChanged += delegate (RemoteProcess sender)
			{
				if (sender.Process == null)
				{
					processToolStripStatusLabel.Text = "No process selected";
				}
				else
				{
					processToolStripStatusLabel.Text = $"{sender.Process.Name} (PID: {sender.Process.Id})";
				}
			};

			memory.Process = remoteProcess;

			ClassNode.NewClassCreated += delegate (ClassNode node)
			{
				classesView.Add(node);
			};

			Graphics graphics = this.CreateGraphics();

			memoryViewControl.Settings = settings;
			memoryViewControl.Memory = memory;
		}

		private void selectProcessToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var pb = new ProcessBrowser(nativeHelper, "notepad++.exe"))
			{
				if (pb.ShowDialog() == DialogResult.OK)
				{
					if (remoteProcess.Process != null)
					{
						nativeHelper.CloseRemoteProcess(remoteProcess.Process.Handle);
					}

					remoteProcess.Process = pb.SelectedProcess;
				}
			}
		}

		private void classesView_ClassSelected(object sender, ClassNode node)
		{
			memoryViewControl.ClassNode = node;

			memoryViewControl.Invalidate();
		}

		private void cleanUnusedClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			classesView.RemoveUnusedClasses();
		}

		private void addBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var item = sender as IntegerToolStripMenuItem;
			if (item == null)
			{
				return;
			}

			memoryViewControl.AddBytes(item.Value);
		}

		private void insertBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var item = sender as IntegerToolStripMenuItem;
			if (item == null)
			{
				return;
			}

			memoryViewControl.InsertBytes(item.Value);
		}

		private void memoryTypeToolStripButton_Click(object sender, EventArgs e)
		{
			var item = sender as TypeToolStripButton;
			if (item == null)
			{
				return;
			}

			memoryViewControl.ReplaceSelectedNodesWithType(item.Value);
		}

		private void newClassToolStripButton_Click(object sender, EventArgs e)
		{
			var node = new ClassNode();
			node.AddBytes(64);

			classesView.SelectedClass = node;
		}
	}
}
