using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTables.Queryable
{
    /// <summary>
    /// Contains datatables parameters sent from client side when POST method is used.
    /// </summary>
    /// <remarks>
    /// Original class code is taken from
    /// https://datatables.net/forums/discussion/40690/sample-implementation-of-serverside-processing-in-c-mvc-ef-with-paging-sorting-searching
    /// </remarks>
    public class DataTablesAjaxPostModel
    {
        /// <summary>
        /// Draw counter. This is used by DataTables to ensure that the Ajax returns from 
        /// server-side processing requests are drawn in sequence by DataTables 
        /// (Ajax requests are asynchronous and thus can return out of sequence). 
        /// This is used as part of the `draw` return parameter.
        /// </summary>
        public int Draw { get; set; }

        /// <summary>
        /// Paging first record indicator. This is the start point in the current data set (0 index based - i.e. 0 is the first record).
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Number of records that the table can display in the current draw. 
        /// It is expected that the number of records returned will be equal to this number, 
        /// unless the server has fewer records to return. 
        /// Note that this can be -1 to indicate that all records should be returned (although that negates any benefits of server-side processing!)
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Global search value. To be applied to all columns which have `Searchable` as `true`.
        /// </summary>
        public SearchData Search { get; set; }

        /// <summary>
        /// Columns parameters.
        /// </summary>
        public List<ColumnData> Columns { get; set; }

        /// <summary>
        /// Order parameters
        /// </summary>
        public List<OrderData> Order { get; set; }

        /// <summary>
        /// Column parameters
        /// </summary>
        public class ColumnData
        {
            /// <summary>
            /// Column's data source
            /// </summary>
            public string Data { get; set; }

            /// <summary>
            /// Column's name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Flag to indicate if this column is searchable (true) or not (false)
            /// </summary>
            public bool Searchable { get; set; }

            /// <summary>
            /// Flag to indicate if this column is orderable (true) or not (false).
            /// </summary>
            public bool Orderable { get; set; }

            /// <summary>
            /// Search value to apply to this specific column.
            /// </summary>
            public SearchData Search { get; set; }
        }

        /// <summary>
        /// Search parameters
        /// </summary>
        public class SearchData
        {
            /// <summary>
            /// Search term
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// Flag to indicate if the search term should be treated as regular expression (true) or not (false).
            /// Not implemented in current version of DataTables.Queryable.
            /// </summary>
            public bool Regex { get; set; }
        }

        /// <summary>
        /// Ordering parameters
        /// </summary>
        public class OrderData
        {
            /// <summary>
            /// Column index to which ordering should be applied.
            /// </summary>
            public int Column { get; set; }

            /// <summary>
            /// Ordering direction for this column.
            /// It will be `asc` or `desc` to indicate ascending ordering or descending ordering, respectively.
            /// </summary>
            public string Dir { get; set; }
        }

        /// <summary>
        /// Converts POST model to <see cref="NameValueCollection"/>.
        /// </summary>
        /// <returns>
        /// New instance of <see cref="NameValueCollection"/>.
        /// </returns>
        internal NameValueCollection ToNameValueCollection()
        {
            NameValueCollection model = new NameValueCollection();

            model["draw"] = Draw.ToString();
            model["start"] = Start.ToString();
            model["length"] = Length.ToString();
            model[$"search[value]"] = Search.Value;
            model[$"search[regex]"] = Search.Regex.ToString();

            for (int i = 0; i< Columns.Count; i++)
            {
                model[$"columns[{i}][data]"] = Columns[i].Data;
                model[$"columns[{i}][name]"] = Columns[i].Name;
                model[$"columns[{i}][searchable]"] = Columns[i].Searchable.ToString();
                model[$"columns[{i}][orderable]"] = Columns[i].Orderable.ToString();
                model[$"columns[{i}][search][value]"] = Columns[i].Search?.Value;
                model[$"columns[{i}][search][regex]"] = Columns[i].Search?.Regex.ToString();
            }

            for (int i = 0; i < Order.Count; i++)
            {
                model[$"order[{i}][column]"] = Order[i].Column.ToString();
                model[$"order[{i}][dir]"] = Order[i].Dir;
            }

            return model;
        }
    }
}
