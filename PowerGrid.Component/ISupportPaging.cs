namespace PowerGrid.Component {
    using System;

    public interface ISupportPaging {
        Action OnGotoFirst { get; set; }
        Action OnGotoPrev { get; set; }
        Action OnGotoNext { get; set; }
        Action OnGotoLast { get; set; }

        //Records per page.
        int PageSize { get; set; }

        //Current page records only.
        object DataSource { get; set; }

        int CurrentPage { get; set; }
        Func<int> TotalPages { get; set; }
        Action OnPageChange { get; set; }
        Action CleanUp { get; set; }
        Action EndConfigure { get; set; }
        bool Configured { get; set; }
    }
}