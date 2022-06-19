using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET.Forms
{
	public partial class NetworkingForm : IconForm
	{
		public bool mConnected = false;

		public NetworkingForm()
		{
			InitializeComponent();
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void label2_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			string ipStr = tbIp.Text;
			string portStr = tbPort.Text;

			if (!string.IsNullOrEmpty(ipStr) && !string.IsNullOrEmpty(portStr))
			{
				short port = 0;

				if (short.TryParse(portStr, out port))
				{
					switch(Program.CoreFunctions.ConnectServer(ipStr, port))//[MarshalAs(UnmanagedType.LPStr)]
					{
						case 0: mConnected = true; Close(); return; // Sucessfully Connected
						case 1: MessageBox.Show("Alredy Connected"); Close(); return;
						case 2: mConnected = false;  MessageBox.Show("Connection Failed"); return;
					}

				} else
				{
					MessageBox.Show("Invalid Port");
				}

			} else
			{
				MessageBox.Show("Fields Empty");
			}
		}

	}
}
