using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;

namespace ReClassNET
{
	static class Program
	{
		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		[STAThread]
		static void Main()
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

			using (var nativeHelper = new NativeHelper())
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm(nativeHelper));
			}
		}
	}
}
