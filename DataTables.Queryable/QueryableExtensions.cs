using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataTables.Queryable
{
    /// <summary>
    /// Set of DataTables.Queryable extensions for <see cref="IQueryable{T}"/>
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Creates a <see cref="IPagedList{T}"/> from a <see cref="IQueryable{T}"/>.
        /// Calling this method invokes executing the query and immediate applying the filter defined by <see cref="DataTablesRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="queryable"><see cref="IQueryable{T}"/> to be filtered and paginated immediately.</param>
        /// <param name="request"><see cref="DataTablesRequest{T}"/> instance with filtering parameters.</param>
        /// <returns><see cref="IPagedList{T}"/> intstance.</returns>
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> queryable, DataTablesRequest<T> request)
        {
            return queryable.Filter(request).ToPagedList();
        }

        /// <summary>
        /// Creates a <see cref="IPagedList{T}"/> from a <see cref="IDataTablesQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="queryable"><see cref="IDataTablesQueryable{T}"/> instance.</param>
        /// <returns><see cref="IPagedList{T}"/> instance.</returns>
        public static IPagedList<T> ToPagedList<T>(this IDataTablesQueryable<T> queryable)
        {
            return new PagedList<T>(queryable);
        }

        /// <summary>
        /// Modifies the <see cref="IQueryable{T}"/> by applying <see cref="DataTablesRequest{T}"/> filtering parameters.
        /// </summary>
        /// <typeparam name="T">Data type to be filtered</typeparam>
        /// <param name="queryable"><see cref="IQueryable{T}"/> instance to be filtered.</param>
        /// <param name="request"><see cref="DataTablesRequest{T}"/> instance that stores filterning request parameters</param>
        /// <returns><see cref="IDataTablesQueryable{T}"/> with appied <see cref="DataTablesRequest{T}"/></returns>
        public static IDataTablesQueryable<T> Filter<T>(this IQueryable<T> queryable, DataTablesRequest<T> request)
        {
            // Modify the IQueryable<T> with consecutive steps.
            // If you need to change the order or add extra steps,
            // you should to write own Filter<T> extension method similarly.
            queryable =

                // convert IQueryable<T> to IDataTablesQueryable<T>
                queryable.AsDataTablesQueryable(request)

                // apply custom filter, if specified
                .CustomFilter()

                // perform global search by all searchable columns
                .GlobalSearch()

                // perform individual columns search by all searchable columns
                .ColumnsSearch()

                // order the IDataTablesQueryable<T> by columns listed in the request
                .Order();

#if TRACE
            Trace.WriteLine($"DataTables.Queryable resulting query:\n {queryable}");
#endif
            return (IDataTablesQueryable<T>)queryable;
        }

        /// <summary>
        /// Converts the <see cref="IQueryable{T}"/> to <see cref="IDataTablesQueryable{T}"/>. 
        /// </summary>
        /// <typeparam name="T"><see cref="IQueryable{T}"/> element type.</typeparam>
        /// <param name="queryable"><see cref="IQueryable{T}"/> instance to be converted to <see cref="IDataTablesQueryable{T}"/>.</param>
        /// <param name="request"><see cref="DataTablesRequest{T}"/> instance with request parameters.</param>
        /// <returns><see cref="IDataTablesQueryable{T}"/> instance.</returns>
        public static IDataTablesQueryable<T> AsDataTablesQueryable<T>(this IQueryable<T> queryable, DataTablesRequest<T> request)
        {
            return new DataTablesQueryable<T>(queryable, request);
        }

        /// <summary>
        /// Modifies the <see cref="IDataTablesQueryable{T}"/> by applying custom filter from <see cref="DataTablesRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">Data type to be filtered</typeparam>
        /// <param name="queryable"><see cref="IDataTablesQueryable{T}"/> instance to be filtered.</param>
        /// <returns>Modified <see cref="IDataTablesQueryable{T}"/> with applied custom filter.</returns>
        public static IDataTablesQueryable<T> CustomFilter<T>(this IDataTablesQueryable<T> queryable)
        {
            if (queryable.Request.CustomFilterPredicate != null)
            {
                queryable = (IDataTablesQueryable<T>)queryable.Where(queryable.Request.CustomFilterPredicate);
            }
            return queryable;
        }

        /// <summary>
        /// Modifies the <see cref="IDataTablesQueryable{T}"/> by applying global search from <see cref="DataTablesRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">Item type to be filtered</typeparam>
        /// <param name="queryable"><see cref="IDataTablesQueryable{T}"/> instance to be filtered.</param>
        /// <returns><see cref="IDataTablesQueryable{T}"/> with appied global search from <see cref="DataTablesRequest{T}"/></returns>
        public static IDataTablesQueryable<T> GlobalSearch<T>(this IDataTablesQueryable<T> queryable)
        {
            if (string.IsNullOrEmpty(queryable.Request.GlobalSearchValue)) return queryable;

            // searchable columns
            var columns = queryable.Request.Columns.Where(c => c.IsSearchable);

            if (!columns.Any()) return queryable;

            var predicate = columns
                .Select(c => c.GlobalSearchPredicate ?? BuildStringContainsPredicate<T>(c.PropertyName,
                                 queryable.Request.GlobalSearchValue, c.SearchCaseInsensitive))
                .Aggregate<Expression<Func<T, bool>>, Expression<Func<T, bool>>>(null,
                    (current, expr) => current == null ? PredicateBuilder.Create(expr) : current.Or(expr));
            queryable = (IDataTablesQueryable<T>) queryable.Where(predicate);

            return queryable;
        }

        /// <summary>
        /// Modifies the <see cref="IDataTablesQueryable{T}"/> by applying individual column search from <see cref="DataTablesRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">Data type to be filtered</typeparam>
        /// <param name="queryable"><see cref="IDataTablesQueryable{T}"/> instance to be filtered.</param>
        /// <returns><see cref="IDataTablesQueryable{T}"/> with appied individual column search from <see cref="DataTablesRequest{T}"/></returns>
        public static IDataTablesQueryable<T> ColumnsSearch<T>(this IDataTablesQueryable<T> queryable)
        {
            // searchable columns
            var columns = queryable.Request.Columns.Where(c =>
                c.IsSearchable &&
                !string.IsNullOrEmpty(c.SearchValue));

            if (!columns.Any()) return queryable;

            var predicate = columns
                .Select(c =>
                    c.ColumnSearchPredicate ??
                    BuildStringContainsPredicate<T>(c.PropertyName, c.SearchValue, c.SearchCaseInsensitive))
                .Aggregate<Expression<Func<T, bool>>, Expression<Func<T, bool>>>(null,
                    (current, expr) => current == null ? PredicateBuilder.Create(expr) : current.And(expr));
            queryable = (IDataTablesQueryable<T>) queryable.Where(predicate);

            return queryable;
        }

        /// <summary>
        /// Modifies the <see cref="IDataTablesQueryable{T}"/> by applying ordering operations defined by <see cref="DataTablesRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">Data type to be ordered</typeparam>
        /// <param name="queryable"><see cref="IDataTablesQueryable{T}"/> instance to be ordered.</param>
        /// <returns><see cref="IDataTablesQueryable{T}"/> with appied ordering from <see cref="DataTablesRequest{T}"/></returns>
        public static IDataTablesQueryable<T> Order<T>(this IDataTablesQueryable<T> queryable)
        {
            // orderable columns
            var columns = queryable.Request.Columns.Where(c =>
                c.IsOrderable &&
                c.OrderingIndex != -1)
                .OrderBy(c => c.OrderingIndex);

            var alreadyOrdered = false;

            foreach (var c in columns)
            {
                queryable = (IDataTablesQueryable<T>)queryable.OrderBy(c.PropertyName, c.OrderingDirection, c.OrderingCaseInsensitive, alreadyOrdered);
                alreadyOrdered = true;
            }

            return queryable;
        }

        /// <summary>
        /// <see cref="object.ToString()"/> method info. 
        /// Used for building search predicates when the searchable property has non-string type.
        /// </summary>
        private static readonly MethodInfo ObjectToString = typeof(object).GetMethod(nameof(ToString));

        /// <summary>
        /// <see cref="string.ToLower()"/> method info. 
        /// Used for conversion of string values to lower case.
        /// </summary>
        private static readonly MethodInfo StringToLower = typeof(string).GetMethod(nameof(string.ToLower), new Type[] { });

        /// <summary>
        /// <see cref="string.Contains(string)"/> method info. 
        /// Used for building default search predicates.
        /// </summary>
        private static readonly MethodInfo StringContains = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });

        /// <summary>
        /// Builds the property expression from the full property name.
        /// </summary>
        /// <param name="param">Parameter expression, like <code>e =></code></param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>MemberExpression instance</returns>
        private static MemberExpression BuildPropertyExpression(Expression param, string propertyName) =>
            (MemberExpression)propertyName.Split('.').Aggregate(param, Expression.Property);

        /// <summary>
        /// Creates predicate expression like 
        /// <code>(T t) => t.SomeProperty.Contains("Constant")</code> 
        /// where "SomeProperty" name is defined by <paramref name="stringConstant"/> parameter, and "Constant" is the <paramref name="stringConstant"/>.
        /// If property has non-string type, it is converted to string with <see cref="object.ToString()"/> method.
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="stringConstant">String constant to construnt the <see cref="string.Contains(string)"/> expression.</param>
        /// <param name="caseInsensitive">Case insensitive search</param>
        /// <returns>Predicate instance</returns>
        private static Expression<Func<T, bool>> BuildStringContainsPredicate<T>(string propertyName, string stringConstant, bool caseInsensitive)
        {
            var type = typeof(T);
            var parameterExp = Expression.Parameter(type, "e");
            var propertyExp = BuildPropertyExpression(parameterExp, propertyName);

            Expression exp = propertyExp;

            // if the property value type is not string, it needs to be casted at first
            if (propertyExp.Type != typeof(string))
            {
                exp = Expression.Call(propertyExp, ObjectToString);
            }

            // call ToLower if case insensitive search
            if (caseInsensitive)
            {
                exp = Expression.Call(exp, StringToLower);
                stringConstant = stringConstant.ToLower();
            }

            var someValue = Expression.Constant(stringConstant, typeof(string));
            var containsMethodExp = Expression.Call(exp, StringContains, someValue);
            return Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
        }

        /// <summary>
        /// Orders the <see cref="IQueryable{T}"/> by property with specified name. 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">Data type</param>
        /// <param name="propertyName">Property name</param>
        /// <param name="direction">Sorting direction</param>
        /// <param name="caseInsensitive">If true, case insensitive ordering will be performed (with forced <see cref="String.ToLower()"/> conversion).</param>
        /// <param name="alreadyOrdered">Flag indicating the <see cref="IQueryable{T}"/> is already ordered.</param>
        /// <returns>Ordered <see cref="IQueryable{T}"/>.</returns>
        private static IQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyName, ListSortDirection direction, bool caseInsensitive, bool alreadyOrdered)
        {
            string methodName;

            switch (direction)
            {
                case ListSortDirection.Ascending when alreadyOrdered:
                    methodName = nameof(System.Linq.Queryable.ThenBy);
                    break;
                case ListSortDirection.Descending when alreadyOrdered:
                    methodName = nameof(System.Linq.Queryable.ThenByDescending);
                    break;

                case ListSortDirection.Ascending:
                    methodName = nameof(System.Linq.Queryable.OrderBy);
                    break;
                case ListSortDirection.Descending:
                    methodName = nameof(System.Linq.Queryable.OrderByDescending);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), $"{direction} is not valid a sorting direction", null);
            }

            var type = typeof(T);
            var parameterExp = Expression.Parameter(type, "e");
            var propertyExp = BuildPropertyExpression(parameterExp, propertyName);

            Expression exp = propertyExp;

            // call ToLower if case insensitive search
            if (caseInsensitive && propertyExp.Type == typeof(string))
            {
                exp = Expression.Call(exp, StringToLower);
            }

            var orderByExp = Expression.Lambda(exp, parameterExp);
            var typeArguments = new[] { type, propertyExp.Type };

            var resultExpr = Expression.Call(typeof(System.Linq.Queryable), methodName, typeArguments, query.Expression, Expression.Quote(orderByExp));

            return query.Provider.CreateQuery<T>(resultExpr);
        }
    }
}
