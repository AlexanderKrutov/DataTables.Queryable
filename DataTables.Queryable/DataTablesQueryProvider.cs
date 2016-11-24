using System.Linq;
using System.Linq.Expressions;

namespace DataTables.Queryable
{
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
