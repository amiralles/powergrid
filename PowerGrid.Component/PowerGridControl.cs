namespace PowerGrid.Component {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    public class PowerGridControl : DataGridView, ISupportPaging {
        private readonly ConditionalFormatEngine _formatEngine;
        private readonly List<ConditionalFormat> _conditionalFormats;
        private readonly List<ConditionalFormat> _userSelectedFormats;

        public PowerGridControl() {
            _formatEngine = new ConditionalFormatEngine();
            _conditionalFormats = new List<ConditionalFormat>();
            _userSelectedFormats = new List<ConditionalFormat>();

            RegisterDefaultConditionalFormats();

            ReadOnly = true;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            AllowUserToOrderColumns = false;
            MultiSelect = false;
            UpperBound = 100;
            SuspendLayout();

            RowPrePaint += (s, e) => {
                if (e.RowIndex < 0 || Rows[e.RowIndex].Tag != null)
                    return;

                //User defined formats take precedence over built in formats.
                if (!ApplyUserDefinedFormat(Rows[e.RowIndex])) {

                    foreach (var cf in _conditionalFormats) {
                        Rows[e.RowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 9f);

                        if (!_formatEngine.Eval(cf.Condition, Rows[e.RowIndex]))
                            continue;

                        Rows[e.RowIndex].DefaultCellStyle.BackColor = cf.Format.BackgroundColor;
                        Rows[e.RowIndex].DefaultCellStyle.ForeColor = cf.Format.ForeColor;
                    }
                }
                //Avoid painting the same row twice.
                Rows[e.RowIndex].Tag = "already painted";
                ResumeLayout(true);
            };
        }

        public Action OnPageChange { get; set; }
        public Action OnGotoFirst { get; set; }
        public Action OnGotoPrev { get; set; }
        public Action OnGotoNext { get; set; }
        public Action OnGotoLast { get; set; }

        public int UpperBound { get; set; }
        public bool Configured { get; set; }

        public Action CleanUp { get; set; }
        public Action EndConfigure { get; set; }

        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public Func<int> TotalPages { get; set; }

        
        public void DisableConditionalFormat() {
            _conditionalFormats.Clear();
            _userSelectedFormats.Clear();
        }

        private void RegisterDefaultConditionalFormats() {

            //Irrelevant to this example
            //Func<DataGridViewRow, bool> retro = r => r != null && Convert.ToInt32(r.Cells["Foo"].Value) == 2;
            //Func<DataGridViewRow, bool> av    = r => r != null && Convert.ToInt32(r.Cells["Foo"].Value) == 1;

            //_conditionalFormats.AddRange(new[] {
            //    new ConditionalFormat {Condition = retro, Format = BuiltinFormats.RowHighlightRed},
            //    new ConditionalFormat {Condition = av, Format    = BuiltinFormats.RowHighlightGreen},
            //});
        }

        public void AddOrUpdateConditionalFormat(Func<DataGridViewRow, bool> condition, Format f) {
            var condFormat = new ConditionalFormat { Condition = condition, Format = f };
            if (_conditionalFormats.Contains(condFormat))
                _conditionalFormats.Remove(condFormat);
        }

        protected override bool DoubleBuffered {
            get {
                return true;//Avoid flickering
            }
            set { }
        }

        protected override void Dispose(bool disposing) {
            CleanUp();
            base.Dispose(disposing);
        }

        public void AddAndApplyFormat(string expression, string formatName) {
            AddAndApplyFormat(_formatEngine.CreateFunc(expression), formatName);
        }

        public void AddAndApplyFormat(Func<DataGridViewRow, bool> expression, string formatName) {
            var format = BuiltinFormats.All.FirstOrDefault(f => f.Name == formatName);
            if (format == null) {
                return;
            }

            _userSelectedFormats.Add(new ConditionalFormat {
                Condition = expression,
                Format = format
            });

            foreach (DataGridViewRow row in Rows)
                ApplyUserDefinedFormat(row);

            if (format == BuiltinFormats.Unformatted) {                
                DisableConditionalFormat();
            }
        }

        private bool ApplyUserDefinedFormat(DataGridViewRow row) {
            var result = false;
            foreach (var cf in _userSelectedFormats.Where(cf => cf.Condition(row))) {
                row.DefaultCellStyle.BackColor = cf.Format.BackgroundColor;
                row.DefaultCellStyle.ForeColor = cf.Format.ForeColor;
                result = true;
            }

            return result;
        }
    }
}