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
    }

    /// <summary>
    /// Internal implementation of <see cref="IDataTablesColumnsCollection{T}"/>
    /// </summary>
    /// <typeparam name="T">Model entity class</typeparam>
    internal class DataTablesColumnsList<T> : List<DataTablesColumn<T>>, IDataTablesColumnsCollection<T>
    {
        public DataTablesColumn<T> this[Expression<Func<T, object>> propertyExpression] => this[GetPropertyPath(propertyExpression)];

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

        private MemberExpression GetMemberExpression(Expression expression)
        {
            if (expression is MemberExpression expression1)
            {
                return expression1;
            }

            if (!(expression is LambdaExpression lambdaExpression)) return null;

            switch (lambdaExpression.Body)
            {
                case MemberExpression memberExpression:
                    return memberExpression;
                case UnaryExpression _:
                    return ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
            }

            return null;
        }

        private string GetPropertyPath(Expression expr)
        {
            var path = new StringBuilder();
            var memberExpression = GetMemberExpression(expr);
            do
            {
                if (path.Length > 0)
                {
                    path.Insert(0, ".");
                }
                path.Insert(0, memberExpression.Member.Name);
                memberExpression = GetMemberExpression(memberExpression.Expression);
            }
            while (memberExpression != null);
            return path.ToString();
        }
    }
}
