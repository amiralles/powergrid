

namespace PowerGrid.DemoApp {    
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using Component;

    public partial class FrmMain : Form {
        private readonly FrmGridContainer frmGrid;

        public FrmMain() {            
            frmGrid = new FrmGridContainer();
            IsMdiContainer = true;
            frmGrid.MdiParent = this;

            InitializeComponent(); 
            WindowState = FormWindowState.Maximized;

            frmGrid.RegisterConditionalFormat();

            frmGrid.Configure( 
                query: GetData(),
                //Please don't do 
                //totalRowCount: () => GetData().ToList().Count ;)
                totalRowCount: () => 50000,
                pageSize: 1000);
            
            frmGrid.Show();            
        }

        private static IQueryable<Person> GetData() {
            //in a real app, we will get the list of persons
            //from a DbContext or something like that.
            var persons = new List<Person>();
            for (var i = 0; i < 50000; i++)
                persons.Add(new Person { Id = i, Name = "person " + i });

            return (from d in persons
                    orderby d.Id
                    select d).AsQueryable();
        }        
    }

    internal class Person {
        public int Id { get; set; }
        public string Name { get; set; }        
    }
}
