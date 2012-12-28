namespace PowerGrid.Component {
    using System;
    using System.Windows.Forms;

    public struct ConditionalFormat {
        //public string Condition { get; set; }
        public Func<DataGridViewRow, bool> Condition { get; set; }
        public Format Format { get; set; }
    }
}