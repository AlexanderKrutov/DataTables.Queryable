using DataTables.Queryable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                if (propertyExpression == null)
                    throw new ArgumentNullException(nameof(propertyExpression));

                MemberExpression memberExpr = null;
                if (propertyExpression.Body.NodeType == ExpressionType.Convert)
                {
                    memberExpr = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
                }
                else if (propertyExpression.Body.NodeType == ExpressionType.MemberAccess)
                {
                    memberExpr = propertyExpression.Body as MemberExpression;
                }

                if (memberExpr == null)
                    throw new ArgumentException($"Expression \"{propertyExpression}\"' does not refer to a property.");

                return this[memberExpr.Member.Name];
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
    }
}
