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

        internal PagedList(IQueryable<T> query, DataTablesRequest<T> request) : base()
        {
            int skipCount = (request.PageNumber - 1) * request.PageSize;
            int takeCount = request.PageSize;

            TotalCount = query.Count();
            PageNumber = request.PageNumber;
            PageSize = request.PageSize;
            PagesCount = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;

            AddRange(query.Skip(skipCount).Take(takeCount));
        }
    }
}
