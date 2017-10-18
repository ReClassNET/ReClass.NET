using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;

namespace ReClassNET.DataExchange.ReClass
{
	public partial class ReClassNetFile
	{
		/// <summary>Reads a platform from the ReClass.NET file.</summary>
		/// <exception cref="FormatException">Thrown if the format of the file is incorrect.</exception>
		/// <param name="path">Full path of the file.</param>
		/// <returns>The platform as string.</returns>
		public static string ReadPlatform(string path)
		{
			Contract.Requires(path != null);

			using (var fs = new FileStream(path, FileMode.Open))
			{
				using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
				{
					var dataEntry = archive.GetEntry(DataFileName);
					if (dataEntry == null)
					{
						throw new FormatException();
					}
					using (var entryStream = dataEntry.Open())
					{
						var document = XDocument.Load(entryStream);

						//var version = document.Root.Attribute(XmlVersionAttribute)?.Value;
						var platform = document.Root?.Attribute(XmlPlatformAttribute)?.Value;

						return platform;
					}
				}
			}
		}
	}
}
