using System.Collections.Generic;
using System.Windows.Forms;

namespace ReClassNET.Extensions
{
	public static class DataGridViewExtension
	{
		public static IEnumerable<DataGridViewRow> GetVisibleRows(this DataGridView dgv)
		{
			var visibleRowsCount = dgv.DisplayedRowCount(true);
			var firstVisibleRowIndex = dgv.FirstDisplayedCell?.RowIndex ?? 0;
			var lastVisibleRowIndex = firstVisibleRowIndex + visibleRowsCount - 1;
			for (var i = firstVisibleRowIndex; i <= lastVisibleRowIndex; i++)
			{
				yield return dgv.Rows[i];
			}
		}
	}
}
