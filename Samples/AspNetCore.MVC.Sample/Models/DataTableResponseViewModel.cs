using DataTables.Queryable;

namespace AspNetCore.MVC.Sample.Models
{
    public class DataTableResponseViewModel<T>
    {
        public int draw { get; set; }

        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }

        public IPagedList<T> data { get; set; }
    }
}
