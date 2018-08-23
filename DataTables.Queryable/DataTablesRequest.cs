using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace DataTables.Queryable
{
    /// <summary>
    /// Represents object that stores request parameters sent by <a href="https://datatables.net/">DataTables.net</a> library.
    /// See <a href="https://datatables.net/manual/server-side">https://datatables.net/manual/server-side</a> for more details.
    /// </summary>
    public class DataTablesRequest<T>
    {
        /// <summary>
        /// 1-based page number (used for pagination of results).
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Count of records per page. Negative value means pagination is off.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Draw counter. This is used by DataTables to ensure that the Ajax returns from server-side processing requests are drawn in sequence by DataTables.
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Global search value. To be applied to all columns which have searchable as true.
        /// Null if no search criteria provided.
        /// </summary>
        public string GlobalSearchValue { get; set; }

        /// <summary>
        /// True if the <see cref="GlobalSearchValue"/> should be treated as a regular expression for advanced searching, false otherwise.
        /// This feature is not implemented yet.
        /// </summary>
        public bool GlobalSearchRegex { get; private set; }

        /// <summary>
        /// Collection of DataTables column info.
        /// Each column can be acessed via indexer by corresponding property name or by property selector. 
        /// </summary>
        /// <example>
        /// Example for an entity Student that has public property FirstName.
        /// <code>
        /// // Get DataTables request from Http query parameters
        /// var request = new DataTablesRequest&lt;Student&gt;(url); 
        /// 
        /// // Access by property name
        /// var column = request.Columns["FirstName"];
        /// 
        /// // Access by property selector
        /// var column = request.Columns[s => s.FirstName];
        /// </code>
        /// </example>
        public IDataTablesColumnsCollection<T> Columns { get; private set; } = new DataTablesColumnsList<T>();

        /// <summary>
        /// Custom predicate to filter the queryable even when the <see cref="GlobalSearchValue"/> not specified.
        /// If custom filter predicate is specified, it is appended in the first place to the resulting queryable.
        /// </summary>
        public Expression<Func<T, bool>> CustomFilterPredicate { get; set; } = null;

        /// <summary>
        /// Original request parameters collection
        /// </summary>
        protected NameValueCollection mOrginalRequest { get; set; } = null;

        /// <summary>
        /// Gets original request parameter value by its name
        /// </summary>
        /// <param name="parameterName">Name of original request parameter</param>
        /// <returns>String value of original request parameter</returns>
        public string this[string parameterName]
        {
            get
            {
                return mOrginalRequest[parameterName];
            }
        }

        /// <summary>
        /// Creates new <see cref="DataTablesRequest{T}"/> from <see cref="IDictionary{String, Object}"/>.
        /// This constructor is useful with it's needed to create <see cref="DataTablesRequest{T}"/> from the Nancy's <a href="https://github.com/NancyFx/Nancy/blob/master/src/Nancy/Request.cs">Request.Form</a> data.
        /// </summary>
        /// <param name="form">Request form data</param>
        public DataTablesRequest(IDictionary<string, object> form) 
            : this(form.Aggregate(new NameValueCollection(), (k, v) => { k.Add(v.Key, v.Value.ToString()); return k; })) { }

        /// <summary>
        /// Creates new <see cref="DataTablesRequest{T}"/> from <see cref="Uri"/> instance.
        /// </summary>
        /// <param name="uri"><see cref="Uri"/> instance</param>
        public DataTablesRequest(Uri uri) 
            : this(uri.Query) { }

        /// <summary>
        /// Creates new <see cref="DataTablesRequest{T}"/> from <see cref="DataTablesAjaxPostModel"/>.
        /// </summary>
        /// <param name="ajaxPostModel">Contains datatables parameters sent from client side when POST method is used.</param>
        public DataTablesRequest(DataTablesAjaxPostModel ajaxPostModel)
            :this(ajaxPostModel.ToNameValueCollection()) {  }

        /// <summary>
        /// Creates new <see cref="DataTablesRequest{T}"/> from http query string.
        /// </summary>
        /// <param name="queryString"></param>
        public DataTablesRequest(string queryString) 
            : this(HttpUtility.ParseQueryString(queryString)) { }

        /// <summary>
        /// Creates new <see cref="DataTablesRequest{T}"/> from <see cref="NameValueCollection"/> instance.
        /// </summary>
        /// <param name="query"></param>
        public DataTablesRequest(NameValueCollection query) 
        {
            if (query == null)
                throw new ArgumentNullException("Datatables query parameters collection is null.");

            if (!query.HasKeys())
                throw new ArgumentException("Datatables query has no keys.");

            mOrginalRequest = new NameValueCollection(query);

#if TRACE
            Trace.WriteLine($"DataTables.Queryable incoming request parameters:");
            query.AllKeys.ToList().ForEach(k => Trace.WriteLine($"{k} = {query[k]}"));
#endif

            int start = Int32.TryParse(query["start"], out start) ? start : 0;
            int length = Int32.TryParse(query["length"], out length) ? length : 15;
            int draw = Int32.TryParse(query["draw"], out draw) ? draw : 0;

            string globalSearch = query["search[value]"];
            bool searchRegex = Boolean.TryParse(query["search[regex]"], out searchRegex) ? searchRegex : false;

            int pageNumber = start / length + 1;

            GlobalSearchValue = globalSearch;
            GlobalSearchRegex = searchRegex;
            PageNumber = pageNumber;
            PageSize = length;
            Draw = draw;

            // extract columns info
            string columnPattern = "columns\\[(\\d+)\\]\\[data\\]";
            var columnKeys = query.AllKeys.Where(k => k != null && Regex.IsMatch(k, columnPattern));
            foreach (var key in columnKeys)
            {
                var colIndex = Regex.Match(key, columnPattern).Groups[1].Value;
                bool orderable = Boolean.TryParse(query[$"columns[{colIndex}][orderable]"], out orderable) ? orderable : true;
                bool searchable = Boolean.TryParse(query[$"columns[{colIndex}][searchable]"], out searchable) ? searchable : true;
                bool colSearchRegex = Boolean.TryParse(query["search[regex]"], out colSearchRegex) ? colSearchRegex : false;
                bool colCISearch = Boolean.TryParse(query[$"columns[{colIndex}][cisearch]"], out colCISearch) ? colCISearch : false;
                bool colCIOrder = Boolean.TryParse(query[$"columns[{colIndex}][ciorder]"], out colCIOrder) ? colCIOrder : false;

                string data = query[$"columns[{colIndex}][data]"];
                string name = query[$"columns[{colIndex}][name]"];
                string searchValue = query[$"columns[{colIndex}][search][value]"];
                string propertyName = null;
                PropertyInfo propertyInfo = null;
                Type type = typeof(T);

                // take property name from `data`
                if (colIndex.ToString() != data)
                {
                    propertyInfo = GetPropertyByName(type, data);
                    if (propertyInfo != null)
                    {
                        propertyName = data;
                    }
                    else
                    {
                        throw new ArgumentException($"Could not find a property called \"{data}\" on type \"{type}\". Make sure you have specified correct value of \"columnDefs.data\" parameter in datatables options.");
                    }
                }

                // take property name from `name`
                if (propertyInfo == null && !string.IsNullOrWhiteSpace(name))
                {
                    propertyInfo = GetPropertyByName(type, name);
                    if (propertyInfo != null)
                    {
                        propertyName = name;
                    }
                    else
                    {
                        throw new ArgumentException($"Could not find a property called \"{name}\" on type \"{type}\". Make sure you have specified correct value of \"columnDefs.name\" parameter in datatables options.");
                    }
                }

                if (propertyName == null)
                {
                    throw new ArgumentException($"Unable to associate datatables column \"{colIndex}\" with model type \"{typeof(T)}\". There are no matching public property found. Make sure you specified valid identifiers for \"columnDefs.data\" and/or \"columnDefs.name\" parameters in datatables options for the column \"{colIndex}\".");
                }

                var column = new DataTablesColumn<T>()
                {
                    Index = Int32.Parse(colIndex),
                    PropertyName = propertyName,
                    SearchValue = searchValue,
                    SearchRegex = colSearchRegex,
                    IsSearchable = searchable,
                    IsOrderable = orderable,
                    SearchCaseInsensitive = colCISearch,
                    OrderingCaseInsensitive = colCIOrder
                };

                Columns.Add(column);
            }

            // extract sorting info
            string orderPattern = "order\\[(\\d)\\]\\[column\\]";
            var orderKeys = query.AllKeys.Where(k => k != null && Regex.IsMatch(k, orderPattern));
            foreach (var key in orderKeys)
            {
                var index = Regex.Match(key, orderPattern).Groups[1].Value;

                int columnIndex = 0;
                int sortingIndex = 0;
                if (Int32.TryParse(index, out sortingIndex) &&
                    Int32.TryParse(query[$"order[{index}][column]"], out columnIndex))
                {
                    var column = Columns.FirstOrDefault(c => c.Index == columnIndex);
                    if (column != null)
                    {
                        column.OrderingIndex = sortingIndex;
                        column.OrderingDirection = query[$"order[{index}][dir]"] == "desc" ? 
                            ListSortDirection.Descending : ListSortDirection.Ascending;
                    }
                }
            }
        }

        /// <summary>
        /// Gets <see cref="PropertyInfo"/> from full property name.
        /// </summary>
        /// <param name="type">Type to get the property from</param>
        /// <param name="propertyName">Full property name. Can contain dots, like "SomeProperty.NestedProperty" to access to nested comlplex types.</param>
        /// <returns><see cref="PropertyInfo"/> instance.</returns>
        private PropertyInfo GetPropertyByName(Type type, string propertyName)
        {
            string[] parts = propertyName.Split('.');
            return (parts.Length > 1)
                ? GetPropertyByName(type.GetProperty(parts[0]).PropertyType, parts.Skip(1).Aggregate((a, i) => $"{a}.{i}"))
                : type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
        }
    }
}
