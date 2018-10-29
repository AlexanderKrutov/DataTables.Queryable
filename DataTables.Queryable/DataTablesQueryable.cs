using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataTables.Queryable
{
    /// <summary>
    /// Extended version of standard <see cref="IQueryable{T}"/> interface with
    /// additional property to access <see cref="DataTablesRequest{T}"/>.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    public interface IDataTablesQueryable<T> : IQueryable<T>
    {
        IQueryable<T> SourceQueryable { get; set; }

        /// <summary>
        /// <see cref="DataTablesRequest{T}"/> instance to filter the original <see cref="IQueryable{T}"/>.
        /// </summary>
        DataTablesRequest<T> Request { get; }
    }

    /// <summary>
    /// Internal implementation of <see cref="IDataTablesQueryable{T}"/> interface.
    /// In fact, this is a wrapper around an <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <typeparam name="T">Data type.</typeparam>
    internal class DataTablesQueryable<T> : IDataTablesQueryable<T>
    {
        public IQueryable<T> SourceQueryable { get; set; }

        private DataTablesQueryProvider<T> sourceProvider;
        private DataTablesRequest<T> request;

        internal DataTablesQueryable(IQueryable<T> query, DataTablesRequest<T> request)
        {
            this.SourceQueryable = query;
            this.sourceProvider = new DataTablesQueryProvider<T>((IAsyncQueryProvider)query.Provider, request);
            this.request = request;
        }

        public Type ElementType
        {
            get
            {
                return typeof(T);
            }
        }

        public Expression Expression
        {
            get
            {
                return SourceQueryable.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return sourceProvider;
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return SourceQueryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return SourceQueryable.GetEnumerator();
        }

        public DataTablesRequest<T> Request
        {
            get
            {
                return request;
            }
        }

        public override string ToString()
        {
            return SourceQueryable.ToString();
        }
    }
}
