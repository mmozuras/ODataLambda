namespace ODataLambda
{
    using System;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Linq.Expressions;

    public static class ODataExtensions
    {
        public static DataServiceQuery<T> Expand<T, TProperty>(this DataServiceQuery<T> query, Expression<Func<T, TProperty>> expression)
        {
            string propertyName = expression.ToPropertyPath();
            return query.Expand(propertyName);
        }

        public static string Expand<T, TProperty>(this DataServiceCollection<T> collection, Expression<Func<T, TProperty>> expression)
        {
            return expression.ToPropertyPath();
        }

        private static string ToPropertyPath(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var memberExpression = (MemberExpression) expression;
                    var result = memberExpression.Expression.ToPropertyPath() + "/" + memberExpression.Member.Name;
                    return RemoveLeadingSlash(result);

                case ExpressionType.Lambda:
                    return ((LambdaExpression)expression).Body.ToPropertyPath();

                case ExpressionType.Call:
                    var methodCallExpression = (MethodCallExpression) expression;
                    var result2 = methodCallExpression.Arguments.Aggregate(string.Empty, (current, arg) => current + "/" + arg.ToPropertyPath());
                    return RemoveLeadingSlash(result2);

                case ExpressionType.Quote:
                    var unaryExpression = (UnaryExpression) expression;
                    return unaryExpression.Operand.ToPropertyPath();
            }
            return string.Empty;
        }

        private static string RemoveLeadingSlash(string s)
        {
            return s.StartsWith("/") ? s.Remove(0, 1) : s;
        }
    }
}