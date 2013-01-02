namespace PowerGrid.Component {
    using System;
    using System.Linq;
    using System.Windows.Forms;
    using System.Drawing;

    public partial class FrmGridContainer : Form {
        private readonly ConditionalFormatEngine _formatEngine;    
        public FrmGridContainer() {            
            InitializeComponent();
            _formatEngine = new ConditionalFormatEngine();

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

            ////The way to go for buil-in formats (or programmer defined formats)
            //grid.AddOrUpdateConditionalFormat(
            //    row => grid.Rows.IndexOf(row) % 2 == 0,                
            //    new Format {
            //        BackgroundColor = Color.Yellow,
            //        Name = "Yellow Highlight"
            //    });


            //This should be used for end users defined formats
            grid.AddOrUpdateConditionalFormat(
                _formatEngine.CreateFunc(
                "id MOD 2 = 0"//User input
                ), 
                new Format {
                    BackgroundColor = Color.YellowGreen,
                    Name = "Green Highlight"
                });
        }
    }
}
