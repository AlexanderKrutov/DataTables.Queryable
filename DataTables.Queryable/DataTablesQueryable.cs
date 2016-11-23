using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataTables.Queryable
{
    internal class DataTablesQueryable<T> : IDataTablesQueryable<T>
    {
        private IQueryable<T> sourceQueryable;
        private DataTablesQueryProvider<T> sourceProvider;
        private DataTablesRequest<T> request;

        internal DataTablesQueryable(IQueryable<T> query, DataTablesRequest<T> request)
        {
            this.sourceQueryable = query;
            this.sourceProvider = new DataTablesQueryProvider<T>(query.Provider, request);
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
                return sourceQueryable.Expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return sourceProvider;
            }
        }

        public IQueryable<T> SourceQueryable
        {
            get
            {
                return sourceQueryable;
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return sourceQueryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return sourceQueryable.GetEnumerator();
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
            return sourceQueryable.ToString();
        }
    }

    internal class DataTablesQueryProvider<T> : IQueryProvider
    {
        private IQueryProvider sourceProvider;
        private DataTablesRequest<T> request;

        internal DataTablesQueryProvider(IQueryProvider sourceProvider, DataTablesRequest<T> request)
        {
            this.sourceProvider = sourceProvider;
            this.request = request;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new DataTablesQueryable<T>((IQueryable<T>)sourceProvider.CreateQuery(expression), request);
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return (IQueryable<TResult>)CreateQuery(expression);
        }

        public object Execute(Expression expression)
        {
            return sourceProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)sourceProvider.Execute(expression);
        }
    }
}
