namespace PowerGrid.Component {
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public static class DataGridViewRowExtensions {
        
        public static Dictionary<string, object> ToDictionary(this DataGridViewRow row, bool uppercaseKeys = true) {
            if (row == null)
                return null;

            return row.DataGridView.Columns.Cast<DataGridViewColumn>()
                .ToDictionary(
                    column => uppercaseKeys ? column.Name.ToUpper() : column.Name,
                    column => row.Cells[column.Name].Value);
        }
    }
}