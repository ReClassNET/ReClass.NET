using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class AboutForm : IconForm
	{
		public AboutForm()
		{
			InitializeComponent();

			bannerBox.Icon = Properties.Resources.ReClassNet.ToBitmap();
			bannerBox.Title = AssemblyInfo.Title;
			bannerBox.Text = $"Version: {AssemblyInfo.Version.ToString(2)}";
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			GlobalWindowManager.RemoveWindow(this);
		}
	}
}
