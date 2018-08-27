using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataTables.Queryable
{
    /// <summary>
    /// Utility extension methods for <see cref="Expression" />
    /// </summary>
    internal static class ExpressionExtensions
    {
        /// <summary>
        /// Gets property path from an expression
        /// </summary>
        /// <param name="expr">Expression instance, for example 'p => p.Office.Street.Address'.</param>
        /// <returns>Full property path like "Office.Street.Address"</returns>
        public static string GetPropertyPath(this Expression expr)
        {
            var path = new StringBuilder();
            MemberExpression memberExpression = GetMemberExpression(expr);
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

        private static MemberExpression GetMemberExpression(Expression expression)
        {
            if (expression is MemberExpression)
            {
                return (MemberExpression)expression;
            }
            else if (expression is LambdaExpression)
            {
                var lambdaExpression = expression as LambdaExpression;
                if (lambdaExpression.Body is MemberExpression)
                {
                    return (MemberExpression)lambdaExpression.Body;
                }
                else if (lambdaExpression.Body is UnaryExpression)
                {
                    return ((MemberExpression)((UnaryExpression)lambdaExpression.Body).Operand);
                }
            }
            return null;
        }
    }
}
