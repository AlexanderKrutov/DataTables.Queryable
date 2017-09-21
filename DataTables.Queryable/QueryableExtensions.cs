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
	public static IEnumerable<string> ExtractPrimitiveProperties(this IEnumerable<PropertyInfo> props)
        {
            var result = new List<string>();
            props
                .Where(t => !t.PropertyType.IsSimpleType())
                .All(p => {
                    var pps = p.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    result.AddRange(pps.Where(t => t.PropertyType.IsSimpleType()).Select(r => p.Name + "." + r.Name));
                    result.AddRange(pps.Where(t => !t.PropertyType.IsSimpleType()).ExtractPrimitiveProperties().Select(r => p.Name + "." + r));
                    return true;
                });
            return result.AsEnumerable();
        }

        public static bool IsSimpleType(this Type type) =>
                type.IsValueType ||
                type.IsPrimitive ||
                new Type[] {
                typeof(String),
                typeof(Decimal),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
	
	
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
            if (!String.IsNullOrEmpty(queryable.Request.GlobalSearchValue))
            {
                // all public properties names 
                var propertyNames = queryable.ElementType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => p.Name);

                // searchable columns
                var columns = queryable.Request.Columns.Where(c =>
                    c.IsSearchable &&
                    propertyNames.Contains(c.PropertyName));

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
            // all public property names 
            var propertyNames = queryable.ElementType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name);

            // searchable columns
            var columns = queryable.Request.Columns.Where(c =>
                c.IsSearchable &&
                !String.IsNullOrEmpty(c.SearchValue) &&
                propertyNames.Contains(c.PropertyName));

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
            // all public property names 
            var propertyNames = queryable.ElementType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name);

            // orderable columns
            var columns = queryable.Request.Columns.Where(c =>
                c.IsOrderable &&
                c.OrderingIndex != -1 &&
                propertyNames.Contains(c.PropertyName))
                .OrderBy(c => c.OrderingIndex);

            bool alreadyOrdered = false;

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
        /// Creates predicate expression like 
        /// <code>(T t) => t.SomeProperty.Contains("Constant")</code> 
        /// where "SomeProperty" name is defined by <paramref name="stringConstant"/> parameter, and "Constant" is the <paramref name="stringConstant"/>.
        /// If property has non-string type, it is converted to string with <see cref="object.ToString()"/> method.
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="propertyName">Property name</param>
        /// <param name="stringConstant">String constant to construnt the <see cref="string.Contains(string)"/> expression.</param>
        /// <returns>Predicate instance</returns>
        private static Expression<Func<T, bool>> BuildStringContainsPredicate<T>(string propertyName, string stringConstant, bool caseInsensitive)
        {
            var parameterExp = Expression.Parameter(typeof(T), "e");
            var propertyExp = Expression.Property(parameterExp, propertyName);

            Expression exp = propertyExp;

            // if the property value type is not string, it needs to be casted at first
            if (typeof(T).GetProperty(propertyName).PropertyType != typeof(string))
            {
                exp = Expression.Call(propertyExp, Object_ToString);
            }

            // call ToLower if case insensitive search
            if (caseInsensitive)
            {
                exp = Expression.Call(exp, String_ToLower);
                stringConstant = stringConstant.ToLower();
            }

            var someValue = Expression.Constant(stringConstant, typeof(string));
            var containsMethodExp = Expression.Call(exp, String_Contains, someValue);
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
            var property = type.GetProperty(propertyName);
            var parameter = Expression.Parameter(type, "e");

            Expression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            if (caseInsensitive && property.PropertyType == typeof(string))
            {
                propertyAccess = Expression.Call(propertyAccess, String_ToLower);
            }

            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };

            var resultExpr = Expression.Call(typeof(System.Linq.Queryable), methodName, typeArguments, query.Expression, Expression.Quote(orderByExp));

            return query.Provider.CreateQuery<T>(resultExpr);
        }
    }
}
