using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DataTables.Queryable
{
    internal class DataTablesQueryProvider<T> : IAsyncQueryProvider
    {
        private IAsyncQueryProvider sourceProvider;
        private DataTablesRequest<T> request;

        internal DataTablesQueryProvider(IAsyncQueryProvider sourceProvider, DataTablesRequest<T> request)
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

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return sourceProvider.ExecuteAsync<TResult>(expression);
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return sourceProvider.ExecuteAsync<TResult>(expression, cancellationToken);
        }
    }
}
