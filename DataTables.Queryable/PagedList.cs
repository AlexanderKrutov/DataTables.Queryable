using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTables.Queryable
{
    public interface IPagedList<T> : IList<T>
    {
        int TotalCount { get; }
        int PageSize { get; }
        int PageNumber { get; }
        int PagesCount { get; }
    }

    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public int TotalCount { get; protected set; }
        public int PageNumber { get; protected set; }
        public int PageSize { get; protected set; }
        public int PagesCount { get; protected set; }

        internal PagedList(IDataTablesQueryable<T> query) : base()
        {


            //int skipCount = (query.Request.PageNumber - 1) * query.Request.PageSize;
            //int takeCount = query.Request.PageSize;

            TotalCount = query.SourceQueryable.Count();
            //PageNumber = query.Request.PageNumber;
            //PageSize = query.Request.PageSize;
            //PagesCount = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;

            AddRange(query.ToList());
        }
    }
}
