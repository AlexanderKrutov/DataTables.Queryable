using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataTables.Queryable
{
    /// <summary>
    /// Defines interface for collection of <see cref="DataTablesColumn{T}"/> items with additional indexer methods
    /// to access an item by column name or model property name.
    /// </summary>
    /// <typeparam name="T">Model entity class</typeparam>
    public interface IDataTablesColumnsCollection<T> : ICollection<DataTablesColumn<T>>
    {
        /// <summary>
        /// Gets column by specified column name
        /// </summary>
        /// <param name="columnName">Column name that matches with the model property name</param>
        /// <returns><see cref="DataTablesColumn{T}"/> instance correspoding to specified model property</returns>
        DataTablesColumn<T> this[string columnName] { get; }

        /// <summary>
        /// Gets column by specified model property
        /// </summary>
        /// <param name="propertyExpression">Expression to locate the desired property</param>
        /// <returns><see cref="DataTablesColumn{T}"/> instance correspoding to specified model property</returns>
        DataTablesColumn<T> this[Expression<Func<T, object>> propertyExpression] { get; }

        /// <summary>
        /// Gets column by its index
        /// </summary>
        /// <param name="columnIndex">Column index</param>
        /// <returns><see cref="DataTablesColumn{T}"/> instance correspoding to specified model property</returns>
        DataTablesColumn<T> this[int columnIndex] { get; }
    }

    /// <summary>
    /// Internal implementation of <see cref="IDataTablesColumnsCollection{T}"/>
    /// </summary>
    /// <typeparam name="T">Model entity class</typeparam>
    internal class DataTablesColumnsList<T> : List<DataTablesColumn<T>>, IDataTablesColumnsCollection<T>
    {
        public DataTablesColumn<T> this[Expression<Func<T, object>> propertyExpression]
        {
            get
            {
                return this[propertyExpression.GetPropertyPath()];
            }
        }

        public DataTablesColumn<T> this[string columnName]
        {
            get
            {                
                var column = this.FirstOrDefault(c => c.PropertyName == columnName);
                if (column == null)
                    throw new ArgumentException($"Column \"{columnName}\" not found", nameof(columnName));

                return column;
            }
        }

        public new DataTablesColumn<T> this[int columnIndex]
        {
            get
            {
                if (columnIndex < 0 || columnIndex > base.Count)
                    throw new ArgumentException($"Column index \"{columnIndex}\" is out of range", nameof(columnIndex));

                return base[columnIndex];
            }
        }
    }
}
