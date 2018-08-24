using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace DataTables.Queryable
{
    /// <summary>
    /// Represents DataTables column filtering/sorting info
    /// </summary>
    /// <typeparam name="T">Model type</typeparam>
    public class DataTablesColumn<T>
    {
        /// <summary>
        /// Column's index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Name of model type property associated with this column.
        /// By default, "columns[i][data]" parameter value is used from the incoming DataTables request.
        /// If "columns[i][name]" is specified, it is used instead.
        /// If model type has no public property matching this value, exception is thrown.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Flag to indicate if this column is searchable (true) or not (false).
        /// </summary>
        public bool IsSearchable { get; set; }

        /// <summary>
        /// Flag to indicate if this column is orderable (true) or not (false).
        /// </summary>
        public bool IsOrderable { get; set; }

        /// <summary>
        /// Search value to apply to this specific column.
        /// </summary>
        public string SearchValue { get; set; }

        /// <summary>
        /// Flag to indicate if the search term for this column should be treated as regular expression (true) or not (false).
        /// </summary>
        public bool SearchRegex { get; set; }

        /// <summary>
        /// Column's ordering index. The resulting enumerable should be sorted first by column with lowest ordering index.
        /// </summary>
        public int OrderingIndex { get; set; } = -1;

        /// <summary>
        /// Column's ordering direction, ascending or descending.
        /// </summary>
        public ListSortDirection OrderingDirection { get; set; }

        /// <summary>
        /// If true, the resulting enumerable will be sorted by the specified column with case-insensitive collation.
        /// </summary>
        public bool OrderingCaseInsensitive { get; set; }

        /// <summary>
        /// If true, the resulting enumerable will be searched by the specified column with case-insensitive collation.
        /// </summary>
        public bool SearchCaseInsensitive { get; set; }

        /// <summary>
        /// Optional predicate expression that will be used to search by the searchable column when <see cref="SearchValue"/> is specified. 
        /// If no predicate provided, <see cref="string.Contains(string)"/> method is used by default.
        /// </summary>
        public Expression<Func<T, bool>> ColumnSearchPredicate { get; set; }

        /// <summary>
        /// Optional expression that specifies the different property which should be used if ordering by the column is required. 
        /// If no expression provided, the same property will be used for sorting as specified by <see cref="PropertyName"/> value.
        /// </summary>
        public Expression<Func<T, object>> ColumnOrderingProperty { get; set; } 

        /// <summary>
        /// Optional predicate expression that will be used to search by the searchable column when <see cref="DataTablesRequest{T}.GlobalSearchValue"/> is specified. 
        /// If no predicate provided, <see cref="string.Contains(string)"/> method is used by default.
        /// </summary>
        public Expression<Func<T, bool>> GlobalSearchPredicate { get; set; }
    }
}
