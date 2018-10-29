using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataTables.Queryable
{
    /// <summary>
    /// Describes single page of data extracted from the <see cref="IDataTablesQueryable{T}"/> 
    /// </summary>
    public interface IPagedList : IList
    {
        /// <summary>
        /// Total items count in the whole collection 
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Count of items per page
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 1-bazed page number
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Total number of pages in the whole collection
        /// </summary>
        int PagesCount { get; }
    }

    /// <summary>
    /// Collection of items that represents a single page of data extracted from the <see cref="IDataTablesQueryable{T}"/>
    /// after applying <see cref="DataTablesRequest{T}"/> filter.
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public interface IPagedList<T> : IPagedList, IList<T> { }

    /// <summary>
    /// Internal implementation of <see cref="IPagedList{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    internal class PagedList<T> : List<T>, IPagedList<T>
    {
        public int TotalCount { get; protected set; }
        public int PageNumber { get; protected set; }
        public int PageSize { get; protected set; }
        public int PagesCount { get; protected set; }

        internal PagedList(IPagedList other) : base()
        {
            TotalCount = other.TotalCount;
            PageNumber = other.PageNumber;
            PageSize = other.PageSize;
            PagesCount = other.PagesCount;
        }

        /// <summary>
        /// Use <see cref="CreateAsync"/>
        /// </summary>
        private PagedList() : base() {}

        /// <summary>
        /// Used by <see cref="CreateAsync"/> to create a <see cref="PagedList{T}"/> aynschronously.
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken"/> to observe while waiting for the task to complete.</param>
        /// <returns></returns>
        private async Task InitializeAsync(IDataTablesQueryable<T> queryable, CancellationToken cancellationToken)
        {
            // pagination is on
            if (queryable.Request.PageSize > 0)
            {
                int skipCount = (queryable.Request.PageNumber - 1) * queryable.Request.PageSize;
                int takeCount = queryable.Request.PageSize;

                TotalCount = await GetCount(queryable.SourceQueryable, cancellationToken);
                PageNumber = queryable.Request.PageNumber;
                PageSize = queryable.Request.PageSize;
                PagesCount = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;

                var pagedQueryable = queryable.SourceQueryable.Skip(skipCount).Take(takeCount);

                AddRange(await ToListAsync(pagedQueryable, cancellationToken));
            }
            // no pagination
            else
            {
                TotalCount = await GetCount(queryable.SourceQueryable, cancellationToken);
                PageNumber = 1;
                PageSize = -1;
                PagesCount = 1;

                AddRange(await queryable.SourceQueryable.ToListAsync(cancellationToken));
            }

        }

        /// <summary>
        /// Get the count asynchronously using a <see cref="CancellationToken" /> if the query is an EF Core queryable./>
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<int> GetCount(IQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Check if the query is an EF Core EntityQueryable for async support.
            if (queryable is EntityQueryable<T>)
            {
                var efQueryable = (EntityQueryable<T>)queryable;
                return await efQueryable.CountAsync(cancellationToken);
            }
            else
            {
                return queryable.Count();
            }
        }

        /// <summary>
        /// Get the list asynchronously using a <see cref="CancellationToken" /> if the query is an EF Core queryable./>
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<List<T>> ToListAsync(IQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            // Check if the query is an EF Core EntityQueryable for async support.
            if (queryable is EntityQueryable<T>)
            {
                var efQueryable = (EntityQueryable<T>)queryable;
                return await efQueryable.ToListAsync(cancellationToken);
            }
            else
            {
                return queryable.ToList();
            }
        }

        /// <summary>
        /// Creates new instance of <see cref="PagedList{T}"/> collection.
        /// </summary>
        /// <param name="queryable"><see cref="IDataTablesQueryable{T}"/>instance to be paginated</param>
        /// <param name="cancellationToken">A <see cref="System.Threading.CancellationToken"/> to observe while waiting for the task to complete.</param>
        internal static async Task<IPagedList<T>> CreateAsync(IDataTablesQueryable<T> queryable, CancellationToken cancellationToken = default(CancellationToken))
        {
            PagedList<T> pagedList = new PagedList<T>();
            await pagedList.InitializeAsync(queryable, cancellationToken);
            return pagedList;
        }


    }
}
