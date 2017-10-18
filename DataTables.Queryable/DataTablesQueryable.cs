using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataTables.Queryable
{
    /// <inheritdoc />
    /// <summary>
    /// Extended version of standard <see cref="T:System.Linq.IQueryable`1" /> interface with
    /// additional property to access <see cref="T:DataTables.Queryable.DataTablesRequest`1" />.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    public interface IDataTablesQueryable<T> : IQueryable<T>
    {
        /// <summary>
        /// <see cref="DataTablesRequest{T}"/> instance to filter the original <see cref="IQueryable{T}"/>.
        /// </summary>
        DataTablesRequest<T> Request { get; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Internal implementation of <see cref="T:DataTables.Queryable.IDataTablesQueryable`1" /> interface.
    /// In fact, this is a wrapper around an <see cref="T:System.Linq.IQueryable`1" />.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    internal class DataTablesQueryable<T> : IDataTablesQueryable<T>
    {
        private readonly IQueryable<T> _sourceQueryable;
        private readonly DataTablesQueryProvider<T> _sourceProvider;

        internal DataTablesQueryable(IQueryable<T> query, DataTablesRequest<T> request)
        {
            _sourceQueryable = query;
            _sourceProvider = new DataTablesQueryProvider<T>(query.Provider, request);
            Request = request;
        }

        public Type ElementType => typeof(T);

        public Expression Expression => _sourceQueryable.Expression;

        public IQueryProvider Provider => _sourceProvider;

        public IEnumerator<T> GetEnumerator()
        {
            return _sourceQueryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sourceQueryable.GetEnumerator();
        }

        public DataTablesRequest<T> Request { get; }

        public override string ToString()
        {
            return _sourceQueryable.ToString();
        }
    }
}
