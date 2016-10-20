using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Logger;

namespace ReClassNET.DataExchange
{
	class ReClass2007File : IReClassImport
	{
		public const string FormatName = "ReClass 2007 File";
		public const string FileExtension = ".rdc";

		private Dictionary<int, SchemaClassNode> classes;

		public SchemaBuilder Load(string filePath, ILogger logger)
		{
			Contract.Requires(filePath != null);
			Contract.Requires(logger != null);

			try
			{
				using (var connection = new SQLiteConnection($@"Data Source={filePath}"))
				{
					connection.Open();

					classes = Query(connection, "SELECT tbl_name FROM sqlite_master WHERE tbl_name like 'class%'")
						.AsEnumerable()
						.ToDictionary(
							r => Convert.ToInt32(r["tbl_name"].ToString().Substring(5)),
							r => new SchemaClassNode
							{
								Name = Query(connection, $"SELECT variable FROM {r["tbl_name"]} WHERE type = 2 LIMIT 1").First()["variable"].ToString(),
#if WIN64
								AddressFormula = "0x140000000"
#else
								AddressFormula = "0x400000"
#endif
							}
						);

					var schema = classes
						.Select(kv =>
						{
							kv.Value.Nodes.AddRange(Query(connection, $"SELECT variable, comment, type, length, ref FROM class{kv.Key} WHERE type != 2").Select(r => ReadNode(r, logger)).Where(n => n != null));
							return kv.Value;
						}).ToList();

					return SchemaBuilder.FromSchema(schema);
				}
			}
			catch (Exception ex)
			{
				logger.Log(ex);

				return null;
			}
		}

		private static readonly SchemaType[] TypeMap = new SchemaType[]
		{
			SchemaType.None,
			SchemaType.ClassInstance,
			SchemaType.None,
			SchemaType.None,
			SchemaType.Hex32,
			SchemaType.Hex16,
			SchemaType.Hex8,
			SchemaType.ClassPtr,
			SchemaType.Int32,
			SchemaType.Int16,
			SchemaType.Int8,
			SchemaType.Float,
			SchemaType.UInt32,
			SchemaType.UInt16,
			SchemaType.UInt8,
			SchemaType.UTF8Text,
			SchemaType.FunctionPtr
		};

		private SchemaNode ReadNode(DataRow row, ILogger logger)
		{
			Contract.Requires(row != null);
			Contract.Requires(logger != null);

			var type = SchemaType.None;

			int typeVal = Convert.ToInt32(row["type"]);
			if (typeVal >= 0 && typeVal < TypeMap.Length)
			{
				type = TypeMap[typeVal];
			}

			if (type == SchemaType.None)
			{
				logger.Log(LogLevel.Warning, $"Skipping node with unknown type: {row["type"]}");
				logger.Log(LogLevel.Warning, row.ToString());

				return null;
			}

			SchemaNode sn = null;

			if (type == SchemaType.ClassInstance || type == SchemaType.ClassPtr)
			{
				var reference = Convert.ToInt32(row["ref"]);
				if (!classes.ContainsKey(reference))
				{
					logger.Log(LogLevel.Warning, $"Skipping node with unknown reference: {row["ref"]}");
					logger.Log(LogLevel.Warning, row.ToString());

					return null;
				}

				sn = new SchemaReferenceNode(type, classes[reference]);
			}
			else
			{
				sn = new SchemaNode(type);
			}

			sn.Name = Convert.ToString(row["variable"]);
			sn.Comment = Convert.ToString(row["comment"]);

			switch (type)
			{
				case SchemaType.UTF8Text:
					sn.Count = Convert.ToInt32(row["length"]);
					break;
			}

			return sn;
		}

		private IEnumerable<DataRow> Query(SQLiteConnection connection, string query)
		{
			Contract.Requires(connection != null);
			Contract.Requires(query != null);

			using (var adapter = new SQLiteDataAdapter(query, connection))
			{
				var ds = new DataSet();

				adapter.Fill(ds);

				return ds.Tables[0].AsEnumerable();
			}
		}
	}
}
