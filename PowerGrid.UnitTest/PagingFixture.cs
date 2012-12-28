namespace PowerGrid.UnitTest {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Component;
    using NUnit.Framework;    

    [TestFixture]
    public class PagingFixture {
        private DataTable _rawData;

        [SetUp]
        public void Setup() {
            _rawData = new DataTable("Test");
            _rawData.Columns.Add("Name");
            _rawData.Columns.Add("Value");

            for (var i = 0; i < 50; i++)
                _rawData.Rows.Add(new object[] { i, "foo" + i });
        }
        
        [Test]
        public void datasource_count_should_match_page_size() {
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;

            grid.Configure(dataSource, () => 50, 10);            
            Assert.AreEqual(10, ((IEnumerable<DataRow>)grid.DataSource).Count());
        }

        [Test]
        public void datasource_count_should_be_less_than_page_size_for_incomplete_pages() {
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;

            grid.Configure(dataSource, () => 55, 10);
            grid.OnGotoLast();

            Assert.IsTrue(((IEnumerable<DataRow>)grid.DataSource).Count() < 10);
        }

        [Test]
        public void when_loads_the_grid_should_go_to_the_first_page() {
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;

            grid.Configure(dataSource,() => 50, 10);
            
            Assert.AreEqual(0, grid.CurrentPage);
            Assert.AreEqual("foo0", ((IEnumerable<DataRow>)grid.DataSource).ToList()[0][1]);
        }

        [Test]
        public void goto_next_page() {
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;
            grid.Configure(dataSource, () => 50, 10);

            grid.OnGotoNext();
            Assert.AreEqual(1, grid.CurrentPage);
            Assert.AreEqual("foo10", ((IEnumerable<DataRow>)grid.DataSource).ToList()[0][1]);
        }

        [Test]
        public void goto_previous_page() {
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;
            
            grid.Configure(dataSource, () => 50, 10);
            grid.OnGotoNext();
            Assert.AreEqual(1, grid.CurrentPage);

            grid.OnGotoPrev();
            Assert.AreEqual(0, grid.CurrentPage);
            Assert.AreEqual("foo0", ((IEnumerable<DataRow>)grid.DataSource).ToList()[0][1]);
        }

        [Test]
        public void goto_first_page() {            
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;
            grid.Configure(dataSource, () => 50, 10);

            grid.OnGotoNext();
            grid.OnGotoNext();
            Assert.AreEqual(2, grid.CurrentPage);

            grid.OnGotoFirst();            
            Assert.AreEqual(0, grid.CurrentPage);
            Assert.AreEqual("foo0", ((IEnumerable<DataRow>)grid.DataSource).ToList()[0][1]);
        }

        [Test]
        public void goto_last_page() {
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;
            
            grid.Configure(dataSource, () => 50, 10);
            grid.OnGotoLast();

            Assert.AreEqual(4, grid.CurrentPage);
            Assert.AreEqual("foo40", ((IEnumerable<DataRow>)grid.DataSource).ToList()[0][1]);
        }

        [Test]
        public void goto_last_incomplete_page() {
            for (var i = 0; i < 5; i++)
                _rawData.Rows.Add(new object[] { i, "foo5" + i });
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;
            
            grid.Configure(dataSource, () => 55, 10);
            grid.OnGotoLast();
           
            Assert.AreEqual(5, grid.CurrentPage);
            Assert.AreEqual(5, ((IEnumerable<DataRow>)grid.DataSource).Count());
            Assert.AreEqual("foo50", ((IEnumerable<DataRow>)grid.DataSource).ToList()[0][1]);
        }
        
        [Test]
        public void verify_total_page_count_when_last_page_is_incomplete() {
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;

            grid.Configure(dataSource, () => 55, 10);

            Assert.AreEqual(6, grid.TotalPages());
        }

        [Test]
        public void verify_total_page_count() {
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;

            grid.Configure(dataSource, () => 50, 10);

            Assert.AreEqual(5, grid.TotalPages());
        }

        [Test, ExpectedException(typeof(ArgumentException), ExpectedMessage = "Page size must be greater than zero.")]
        public void when_configures_page_size_equal_to_zero_throws_ArgumentException() {
            var grid = new PowerGridControl();
            var dataSource = from DataRow row in _rawData.Rows.OfType<DataRow>().AsQueryable() select row;

            grid.Configure(dataSource, () => 50, 0);
        }
    }
}