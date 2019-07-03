using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataTables.Queryable
{
    /// <summary>
    /// Set of DataTables.Queryable extensions for <see cref="IQueryable{T}"/>
    /// </summary>
    public static class QueryableExtensions
    {
        #region Synchronous methods

        /// <summary>
        /// Creates a <see cref="IPagedList{T}"/> from a <see cref="IEnumerable{T}"/>.
        /// Calling this method invokes executing the query and immediate applying the filter defined by <see cref="DataTablesRequest{T}"/>.
        /// <param name="source"><see cref="IEnumerable{T}"/> to be filtered and paginated immediately.</param>
        /// <param name="request"><see cref="DataTablesRequest{T}"/> instance with filtering parameters.</param>
        /// </summary>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, DataTablesRequest<T> request)
        {
            return source.AsQueryable().ToPagedList(request);
        }

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

        #endregion Synchronous methods

        #region Asynchronous methods

        /// <summary>
        /// Asynchronously creates a <see cref="IPagedList{T}"/> from a <see cref="IEnumerable{T}"/>.
        /// Calling this method invokes executing the query and applying the filter defined by <see cref="DataTablesRequest{T}"/>.
        /// <param name="source"><see cref="IEnumerable{T}"/> to be filtered and paginated.</param>
        /// <param name="request"><see cref="DataTablesRequest{T}"/> instance with filtering parameters.</param>
        /// </summary>
        public static Task<IPagedList<T>> ToPagedListAsync<T>(this IEnumerable<T> source, DataTablesRequest<T> request)
        {
            return Task.Factory.StartNew(() => source.AsQueryable().ToPagedList(request));
        }

        /// <summary>
        /// Asynchronously creates a <see cref="IPagedList{T}"/> from a <see cref="IQueryable{T}"/>.
        /// Calling this method invokes executing the query and applying the filter defined by <see cref="DataTablesRequest{T}"/>.
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="queryable"><see cref="IQueryable{T}"/> to be filtered and paginated.</param>
        /// <param name="request"><see cref="DataTablesRequest{T}"/> instance with filtering parameters.</param>
        /// <returns><see cref="IPagedList{T}"/> intstance.</returns>
        public static Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> queryable, DataTablesRequest<T> request)
        {
            return Task.Factory.StartNew(() => queryable.Filter(request).ToPagedList());
        }

        /// <summary>
        /// Asynchronously creates a <see cref="IPagedList{T}"/> from a <see cref="IDataTablesQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="queryable"><see cref="IDataTablesQueryable{T}"/> instance.</param>
        /// <returns><see cref="IPagedList{T}"/> instance.</returns>
        public static Task<IPagedList<T>> ToPagedListAsync<T>(this IDataTablesQueryable<T> queryable)
        {
            return Task.Factory.StartNew<IPagedList<T>>(() => new PagedList<T>(queryable));
        }
        
        #endregion Asynchronous methods

        /// <summary>
        /// Applies specified action to the each item of paged list.
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="list">Paged list instance.</param>
        /// <param name="action">Action to be applied.</param>
        /// <returns><see cref="IPagedList{T}"/> instance.</returns>
        public static IPagedList<T> Apply<T>(this IPagedList<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
            return list;
        }

        /// <summary>
        /// Converts each item from the source list by applying specified converter function. 
        /// </summary>
        /// <typeparam name="T">Source data type</typeparam>
        /// <typeparam name="TResult">Result data type</typeparam>
        /// <param name="source">Paged list instance.</param>
        /// <param name="converter">Converter to be applied.</param>
        /// <returns><see cref="IPagedList{TResult}"/> instance.</returns>
        public static IPagedList<TResult> Convert<T, TResult>(this IPagedList<T> source, Func<T, TResult> converter)
        {
            var list = new PagedList<TResult>(source);
            foreach (var item in source)
            {
                list.Add(converter(item));
            }
            return list;
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

            if (request.Log != null)
            {
                StringBuilder sb = new StringBuilder("DataTables.Queryable -> Incoming request:\n");
                foreach (string key in request.OriginalRequest.AllKeys)
                {
                    string value = request.OriginalRequest[key];
                    sb.AppendLine($"{key} = {$"\"{value}\""}");
                }
                sb.AppendLine();
                sb.AppendLine($"DataTables.Queryable -> Resulting queryable:\n{queryable}\n");

                request.Log.BeginInvoke(sb.ToString(), null, null);
            }

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
            if (!String.IsNullOrEmpty(queryable.Request.GlobalSearchValue))
            {
                // searchable columns
                var columns = queryable.Request.Columns.Where(c => c.IsSearchable);

                if (columns.Any())
                {
                    Expression<Func<T, bool>> predicate = null;
                    foreach (var c in columns)
                    {
                        var expr = c.GlobalSearchPredicate ?? BuildStringContainsPredicate<T>(c.PropertyName, queryable.Request.GlobalSearchValue, c.SearchCaseInsensitive);
                        predicate = predicate == null ?
                            PredicateBuilder.Create(expr) :
                            predicate.Or(expr);
                    }
                    queryable = (IDataTablesQueryable<T>)queryable.Where(predicate);
                }
            }
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
                !String.IsNullOrEmpty(c.SearchValue));

            if (columns.Any())
            {
                Expression<Func<T, bool>> predicate = null;
                foreach (var c in columns)
                {
                    var expr = c.ColumnSearchPredicate ?? BuildStringContainsPredicate<T>(c.PropertyName, c.SearchValue, c.SearchCaseInsensitive);
                    predicate = predicate == null ?
                        PredicateBuilder.Create(expr) :
                        predicate.And(expr);
                }
                queryable = (IDataTablesQueryable<T>)queryable.Where(predicate);
            }
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

            bool alreadyOrdered = false;

            foreach (var c in columns)
            {
                var propertyName = c.ColumnOrderingProperty != null ? c.ColumnOrderingProperty.GetPropertyPath() : c.PropertyName;
                queryable = (IDataTablesQueryable<T>)queryable.OrderBy(propertyName, c.OrderingDirection, c.OrderingCaseInsensitive, alreadyOrdered);
                alreadyOrdered = true;
            }

            return queryable;
        }

        /// <summary>
        /// <see cref="object.ToString()"/> method info. 
        /// Used for building search predicates when the searchable property has non-string type.
        /// </summary>
        private static readonly MethodInfo Object_ToString = typeof(object).GetMethod(nameof(object.ToString));

        /// <summary>
        /// <see cref="string.ToLower()"/> method info. 
        /// Used for conversion of string values to lower case.
        /// </summary>
        private static readonly MethodInfo String_ToLower = typeof(string).GetMethod(nameof(String.ToLower), new Type[] { });

        /// <summary>
        /// <see cref="string.Contains(string)"/> method info. 
        /// Used for building default search predicates.
        /// </summary>
        private static readonly MethodInfo String_Contains = typeof(string).GetMethod(nameof(String.Contains), new[] { typeof(string) });

        /// <summary>
        /// Builds the property expression from the full property name.
        /// </summary>
        /// <param name="param">Parameter expression, like <code>e =></code></param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>MemberExpression instance</returns>
        private static MemberExpression BuildPropertyExpression(ParameterExpression param, string propertyName)
        {
            string[] parts = propertyName.Split('.');
            Expression body = param;
            foreach (var member in parts)
            {
                body = Expression.Property(body, member);
            }
            return (MemberExpression)body;
        }

        /// <summary>
        /// Creates predicate expression like 
        /// <code>(T t) => t.SomeProperty.Contains("Constant")</code> 
        /// where "SomeProperty" name is defined by <paramref name="stringConstant"/> parameter, and "Constant" is the <paramref name="stringConstant"/>.
        /// If property has non-string type, it is converted to string with <see cref="object.ToString()"/> method.
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="stringConstant">String constant to construct the <see cref="string.Contains(string)"/> expression.</param>
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
                exp = Expression.Call(propertyExp, Object_ToString);
            }

            var someValue = Expression.Constant(caseInsensitive ? stringConstant.ToLower() : stringConstant, typeof(string));
            var containsMethodExp = Expression.Call(exp, String_Contains, someValue);
            var notNullExp = Expression.NotEqual(exp, Expression.Constant(null, typeof(object)));

            // call ToLower if case insensitive search
            if (caseInsensitive)
            {
                var toLowerExp = Expression.Call(exp, String_ToLower);
                containsMethodExp = Expression.Call(toLowerExp, String_Contains, someValue);
            }

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(notNullExp, containsMethodExp), parameterExp);
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
            string methodName = null;

            if (direction == ListSortDirection.Ascending && !alreadyOrdered)
                methodName = nameof(System.Linq.Queryable.OrderBy);
            else if (direction == ListSortDirection.Descending && !alreadyOrdered)
                methodName = nameof(System.Linq.Queryable.OrderByDescending);
            if (direction == ListSortDirection.Ascending && alreadyOrdered)
                methodName = nameof(System.Linq.Queryable.ThenBy);
            else if (direction == ListSortDirection.Descending && alreadyOrdered)
                methodName = nameof(System.Linq.Queryable.ThenByDescending);

            var type = typeof(T);
            var parameterExp = Expression.Parameter(type, "e");
            var propertyExp = BuildPropertyExpression(parameterExp, propertyName);

            Expression exp = propertyExp;

            // call ToLower if case insensitive search
            if (caseInsensitive && propertyExp.Type == typeof(string))
            {
                exp = Expression.Call(exp, String_ToLower);
            }

            var orderByExp = Expression.Lambda(exp, parameterExp);
            var typeArguments = new Type[] { type, propertyExp.Type };

            var resultExpr = Expression.Call(typeof(System.Linq.Queryable), methodName, typeArguments, query.Expression, Expression.Quote(orderByExp));

            return query.Provider.CreateQuery<T>(resultExpr);
        }
    }
}
