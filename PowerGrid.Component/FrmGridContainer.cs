using System;
using System.Linq;
using System.Windows.Forms;

namespace PowerGrid.Component {
    using System.Drawing;

    public partial class FrmGridContainer : Form {
        
        public FrmGridContainer() {            
            InitializeComponent();

            lblShowing.Enabled = false;

            grid.OnPageChange = () =>
                lblShowing.Text = string.Format("Shoing page {0} out of {1}",
                grid.CurrentPage + 1,
                grid.TotalPages());

            btnFirst.Click += delegate {
                grid.OnGotoFirst();
            };

            btnPrev.Click += delegate {
                grid.OnGotoPrev();
            };

            btnNext.Click += delegate {
                grid.OnGotoNext();
            };

            btnLast.Click += delegate {
                grid.OnGotoLast();
            };                       
        }

        public void Configure<T>(IQueryable<T> query, Func<int> totalRowCount, int pageSize) {            
            grid.Configure(query, totalRowCount,pageSize);
        }

        public void RegisterConditionalFormat() {


            grid.AddOrUpdateConditionalFormat(

                row => grid.Rows.IndexOf(row) % 2 == 0,
                
                new Format {
                    BackgroundColor = Color.Yellow,
                    Name = "Yellow Highlight"
                });
        }
    }
}
