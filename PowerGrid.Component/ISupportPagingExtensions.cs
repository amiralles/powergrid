namespace PowerGrid.Component {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    public static class ISupportPagingExtensions {

        public static void Search<T>(this ISupportPaging grid,
            IQueryable<T> source,
            Func<T, bool> criteria) {

            if (!grid.Configured)
                throw new InvalidOperationException(
                    "You have to configure the grid first. Take a look at the configure method");
            
            PagingCache.Invalidate();
            grid.SetDataSource(First, source.Where(criteria).AsQueryable());
        }
        
        public static void Configure<T>(this ISupportPaging grid, 
            IQueryable<T> query, 
            Func<int> totalRowCount, 
            int pageSize = 100) {
            ValidateInput(grid, query, totalRowCount, pageSize);
            
            grid.PageSize = pageSize;

            grid.OnGotoFirst = () => grid.SetDataSource(First, query);
            
            grid.OnGotoPrev = () => grid.SetDataSource(Prev, query);
            grid.OnGotoNext = () => grid.SetDataSource(Next, query);
            grid.OnGotoLast = () => grid.SetDataSource(Last, query);
            
            if (grid.TotalPages == null) {
                //local copy to avoid possible multiple enumerations.
                var total = totalRowCount();
                //
                grid.TotalPages = () => CalculatePages(grid, total);
            }

            //Goes to the first page.
            grid.SetDataSource(First, query);
            grid.CleanUp = () => 
                PagingCache.Remove(grid);//<= to avoid memory  leaks.

            if (grid.EndConfigure != null)
                grid.EndConfigure();
            grid.Configured = true;
        }

        private static int CalculatePages(ISupportPaging grid, int total) {
            var result = total / grid.PageSize;
            if (total % grid.PageSize != 0)
                result++;
            return result;
        }

        private static void ValidateInput<T>(ISupportPaging grid, IQueryable<T> query, Func<int> totalRowCount, int pageSize) {
            if (grid == null)
                throw new ArgumentNullException("grid");

            if (query == null)
                throw new ArgumentNullException("query");

            if (totalRowCount == null)
                throw new ArgumentNullException("totalRowCount");

            if (pageSize < 1)
                throw new ArgumentException("Page size must be greater than zero.");
        }

        static void SetDataSource<T>(this ISupportPaging grid, Func<ISupportPaging, IQueryable<T>, IEnumerable<T>> gotoPage, IQueryable<T> source) {
            grid.DataSource = null;            
            grid.DataSource = QueryToSortableDataSource(gotoPage(grid, source));
            
            if (grid.OnPageChange != null)
                grid.OnPageChange();
        }

        private static SortableBindingList<T> QueryToSortableDataSource<T>(IEnumerable<T> queryResult) {
            return new SortableBindingList<T>(queryResult);
        }

        //paging strategies.
        static IEnumerable<T> First<T>(this ISupportPaging grid, 
            IQueryable<T> dataSource) {
            grid.CurrentPage = 0;
            return grid.GetPage(dataSource);
        }

        static IEnumerable<T> Prev<T>(this ISupportPaging grid, IQueryable<T> dataSource) {
            if (grid.CurrentPage != 0)
                grid.CurrentPage--;
            return grid.GetPage(dataSource);
        }

        static IEnumerable<T> Next<T>(this ISupportPaging grid, IQueryable<T> dataSource) {
            if (grid.CurrentPage < grid.TotalPages() - 1)
                grid.CurrentPage++;
            return grid.GetPage(dataSource);
        }

        static IEnumerable<T> Last<T>(this ISupportPaging grid, IQueryable<T> dataSource) {
            if (grid.TotalPages() > 0)
                grid.CurrentPage = grid.TotalPages() - 1;
            return grid.GetPage(dataSource);
        }
        //

        private static IEnumerable<T> InternalSearch<T>(this ISupportPaging grid, IQueryable<T> dataSource) {
            grid.CurrentPage = 0;            
            PagingCache.Invalidate();//maybe this is not the best way to go....
            return grid.GetPage(dataSource);
        }

        static IEnumerable<T> GetPage<T>(this ISupportPaging grid, 
            IQueryable<T> dataSource) {
            return PagingCache.CacheAndReturn(grid, dataSource);
        }

        #region Caching

        /// <summary>
        /// This class takes care of datasource caching.
        /// </summary>
        static class PagingCache {
            private static readonly Dictionary<CacheKey, object> _storage;

            static PagingCache() {
                _storage = new Dictionary<CacheKey, object>();
            }

            private static void Cache<T>(ISupportPaging grid, IEnumerable<T> queryResult) {
                var key = new CacheKey(grid);
                if (_storage.ContainsKey(key))
                    _storage.Remove(key);
                _storage.Add(key, queryResult);
            }

            internal static void Remove(ISupportPaging grid) {
                var key = new CacheKey(grid);
                if (_storage.ContainsKey(key))
                    _storage.Remove(key);
            }

            private static IEnumerable<T> GetCached<T>(ISupportPaging grid) {
                var key = new CacheKey(grid);
                if (!_storage.ContainsKey(key))
                    return null;
                return (IEnumerable<T>)_storage[key];
            }

            private struct CacheKey {
                private readonly ISupportPaging _grid;
                private readonly int _gridPage;

                public CacheKey(ISupportPaging grid) {
                    _grid = grid;
                    _gridPage = grid.CurrentPage;
                }

                private bool Equals(CacheKey other) {
                    return Equals(_grid, other._grid) && _gridPage == other._gridPage;
                }

                public override bool Equals(object obj) {
                    if (ReferenceEquals(null, obj)) return false;
                    return obj is CacheKey && Equals((CacheKey)obj);
                }

                public override int GetHashCode() {
                    unchecked {
                        return ((_grid != null ? _grid.GetHashCode() : 0) * 397) ^ _gridPage;
                    }
                }
            }

            public static IEnumerable<T> CacheAndReturn<T>(ISupportPaging grid, 
                IQueryable<T> dataSource) {

                var result = GetCached<T>(grid);
                if (result == null)
                    Cache(grid,
                        //paging in action
                          (from p in dataSource select p)
                              .Skip( /*start page*/grid.CurrentPage * grid.PageSize)
                              .Take(grid.PageSize)
                              .ToList()); //only at this point we hit the database
                        //***********************************************************
                return GetCached<T>(grid);
            }

            public static void Invalidate() {
                _storage.Clear();
            }
        }

        #endregion
    }
}