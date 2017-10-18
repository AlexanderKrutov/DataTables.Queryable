using System.Linq;
using System.Linq.Expressions;

namespace DataTables.Queryable
{
    internal class DataTablesQueryProvider<T> : IQueryProvider
    {
        private readonly IQueryProvider _sourceProvider;
        private readonly DataTablesRequest<T> _request;

        internal DataTablesQueryProvider(IQueryProvider sourceProvider, DataTablesRequest<T> request)
        {
            _sourceProvider = sourceProvider;
            _request = request;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new DataTablesQueryable<T>((IQueryable<T>)_sourceProvider.CreateQuery(expression), _request);
        }

        public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
        {
            return (IQueryable<TResult>)CreateQuery(expression);
        }

        public object Execute(Expression expression)
        {
            return _sourceProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)_sourceProvider.Execute(expression);
        }
    }
}
