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
        public static DataServiceQuery<T> Expand<T, TProperty>(this DataServiceQuery<T> query, Expression<Func<T, TProperty>> expression)
        {
            string propertyName = expression.ToPropertyPath();
            return query.Expand(propertyName);
        }

        private static string ToPropertyPath<T, TProperty>(this Expression<Func<T, TProperty>> expression)
        {
            IList<string> propertyNames = new List<string>();
            Expression currentNode = expression.Body;
            while (currentNode.NodeType != ExpressionType.Parameter)
            {
                if (currentNode.NodeType == ExpressionType.MemberAccess || currentNode.NodeType == ExpressionType.Convert)
                {
                    MemberExpression memberExpression = GetMemberExpression(currentNode);
                    MemberInfo member = memberExpression.Member;

                    if (!IsPropertyOrField(member))
                    {
                        throw new InvalidOperationException(string.Format("The member '{0}' is a {1} but a Property or Field is expected.", member.Name, member.MemberType));
                    }
                    propertyNames.Add(member.Name);
                    currentNode = memberExpression.Expression;
                }
                else
                {
                    throw new InvalidOperationException(string.Format("The expression node type '{0}' is not supported. Expected: MemberAccess or Convert", currentNode.NodeType));
                }
            }
            return string.Join("/", propertyNames.Reverse().ToArray());
        }

        private static bool IsPropertyOrField(MemberInfo member)
        {
            return member.MemberType == MemberTypes.Property || member.MemberType == MemberTypes.Field;
        }

        private static MemberExpression GetMemberExpression(Expression currentNode)
        {
            MemberExpression memberExpression;
            if (currentNode.NodeType == ExpressionType.MemberAccess)
            {
                memberExpression = (MemberExpression)currentNode;
            }
            else
            {
                memberExpression = (MemberExpression)((UnaryExpression)currentNode).Operand;
            }
            return memberExpression;
        }
    }
}