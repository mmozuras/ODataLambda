namespace ODataLambda
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ODataExtensions
    {
        public static void LoadProperty<T, TProperty>(this DataServiceContext context, T entity,
            Expression<Func<T, TProperty>> expression)
        {
            string propertyPath = expression.ToPropertyPath();
            context.LoadProperty(entity, propertyPath);
        }

        public static void LoadAllProperties<T>(this DataServiceContext context, T entity)
        {
            ForEachProperty<T>(p => context.LoadProperty(entity, p.Name));
        }

        public static DataServiceQuery<T> Expand<T, TProperty>(this DataServiceQuery<T> query, Expression<Func<T, TProperty>> expression)
        {
            string propertyPath = expression.ToPropertyPath();
            return query.Expand(propertyPath);
        }

        public static DataServiceQuery<T> ExpandAll<T>(this DataServiceQuery<T> query)
        {
            var properties = GetReferenceTypeProperties<T>();
            return properties.Aggregate(query, (current, property) => current.Expand(property.Name));
        }

        public static string Expand<T, TProperty>(this DataServiceCollection<T> collection, Expression<Func<T, TProperty>> expression)
        {
            return expression.ToPropertyPath();
        }

        private static void ForEachProperty<T>(Action<PropertyInfo> action)
        {
            var properties = GetReferenceTypeProperties<T>();
            foreach (var property in properties)
            {
                action(property);
            }
        }

        private static IEnumerable<PropertyInfo> GetReferenceTypeProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !x.PropertyType.IsValueType);
        }

        private static string ToPropertyPath(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Call:
                    var methodCallExpression = (MethodCallExpression) expression;
                    var result = methodCallExpression.Arguments.Aggregate(string.Empty, (current, arg) => current + "/" + arg.ToPropertyPath());
                    return RemoveLeadingSlash(result);

                case ExpressionType.Lambda:
                    return ((LambdaExpression)expression).Body.ToPropertyPath();

                case ExpressionType.MemberAccess:
                case ExpressionType.Convert:
                    var memberExpression = (MemberExpression) expression;
                    var result2 = memberExpression.Expression.ToPropertyPath() + "/" + memberExpression.Member.Name;
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