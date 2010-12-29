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
        /// <summary>
        /// Notifies the DataServiceContext that a new link exists between the objects specified and that the link is represented by the property specified by the sourceProperty parameter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="source">The source object for the new link.</param>
        /// <param name="sourceProperty">The property on the source object that identifies the target object of the new link.</param>
        /// <param name="target">The child object involved in the new link that is to be initialized by calling this method. The target object must be a subtype of the type identified by the sourceProperty parameter. If target is set to null, the call represents a delete link operation.</param>
        public static void SetLink<TSource, TProperty>(this DataServiceContext context, TSource source,
                                                       Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            context.SetLink(source, sourceProperty.ToPropertyPath(), target);
        }

        /// <summary>
        /// Adds the specified link to the set of objects the DataServiceContext is tracking.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="source">The source object for the new link.</param>
        /// <param name="sourceProperty">The property on the source object that returns the related object.</param>
        /// <param name="target">The object related to the source object by the new link. </param>
        public static void AddLink<TSource, TProperty>(this DataServiceContext context, TSource source,
                                                       Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            context.AddLink(source, sourceProperty.ToPropertyPath(), target);
        }

        /// <summary>
        /// Changes the state of the link to deleted in the list of links being tracked by the DataServiceContext.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="source">The source object in the link to be marked for deletion.</param>
        /// <param name="sourceProperty">The property on the source object that is used to access the target object.</param>
        /// <param name="target">The target object involved in the link that is bound to the source object. The target object must be of the type identified by the source property or a subtype.</param>
        public static void DeleteLink<TSource, TProperty>(this DataServiceContext context, TSource source,
                                                          Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            context.DeleteLink(source, sourceProperty.ToPropertyPath(), target);
        }

        /// <summary>
        /// Notifies the DataServiceContext to start tracking the specified link that defines a relationship between entity objects.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="source">The source object in the new link.</param>
        /// <param name="sourceProperty">The property on the source object that represents the link between the source and target object.</param>
        /// <param name="target">The target object in the link that is bound to the source object specified in this call. The target object must be of the type identified by the source property or a subtype.</param>
        public static void AttachLink<TSource, TProperty>(this DataServiceContext context, TSource source,
                                                          Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            context.AttachLink(source, sourceProperty.ToPropertyPath(), target);
        }

        /// <summary>
        /// Removes the specified link from the list of links being tracked by the DataServiceContext.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="source">The source object participating in the link to be marked for deletion.</param>
        /// <param name="sourceProperty">The property on the source object that represents the source in the link between the source and the target.</param>
        /// <param name="target">The target object involved in the link that is bound to the source object. The target object must be of the type identified by the source property or a subtype.</param>
        public static void DetachLink<TSource, TProperty>(this DataServiceContext context, TSource source,
                                                          Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            context.DetachLink(source, sourceProperty.ToPropertyPath(), target);
        }

        /// <summary>
        /// Loads deferred content for a specified property from the data service.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity">The entity that contains the property to load.</param>
        /// <param name="property">The property to load.</param>
        public static void LoadProperty<T, TProperty>(this DataServiceContext context, T entity,
                                                      Expression<Func<T, TProperty>> property)
        {
            context.LoadProperty(entity, property.ToPropertyPath());
        }

        /// <summary>
        /// Loads deferred content for a specified property from the data service.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity">The entity that contains the property to load.</param>
        /// <param name="property">The property to load.</param>
        /// <param name="continuation">A DataServiceQueryContinuation object that represents the next page of related entities to load from the data service.</param>
        public static void LoadProperty<T, TProperty>(this DataServiceContext context, T entity,
                                                      Expression<Func<T, TProperty>> property, DataServiceQueryContinuation<T> continuation)
        {
            context.LoadProperty(entity, property.ToPropertyPath(), continuation);
        }

        /// <summary>
        /// Loads deferred content for all properties.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity">The entity that contains the properties to load.</param>
        public static void LoadAllProperties<T>(this DataServiceContext context, T entity)
        {
            ForEachProperty<T>(p => context.LoadProperty(entity, p.Name));
        }

        /// <summary>
        /// Creates a new DataServiceQuery with the expand option set in the URI generated by the returned query.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="property">The property to expand.</param>
        public static DataServiceQuery<T> Expand<T, TProperty>(this DataServiceQuery<T> query, Expression<Func<T, TProperty>> property)
        {
            return query.Expand(property.ToPropertyPath());
        }

        /// <summary>
        /// Creates a new DataServiceQuery with the expand option set for all properties of T in the URI generated by the returned query.
        /// </summary>
        public static DataServiceQuery<T> ExpandAll<T>(this DataServiceQuery<T> query)
        {
            IEnumerable<PropertyInfo> properties = GetReferenceTypeProperties<T>();
            return properties.Aggregate(query, (current, property) => current.Expand(property.Name));
        }

        /// <summary>
        /// Creates a new DataServiceQuery with the expand option set in the URI generated by the returned query.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="property">The property to expand.</param>
        public static string Expand<T, TProperty>(this DataServiceCollection<T> collection, Expression<Func<T, TProperty>> property)
        {
            return property.ToPropertyPath();
        }

        private static void ForEachProperty<T>(Action<PropertyInfo> action)
        {
            IEnumerable<PropertyInfo> properties = GetReferenceTypeProperties<T>();
            foreach (PropertyInfo property in properties)
            {
                action(property);
            }
        }

        private static IEnumerable<PropertyInfo> GetReferenceTypeProperties<T>()
        {
            return typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !x.PropertyType.IsValueType);
        }

        private static string ToPropertyPath(this Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Call:
                    var methodCallExpression = (MethodCallExpression) expression;
                    string result = methodCallExpression.Arguments.Aggregate(string.Empty,
                                                                             (current, arg) => current + "/" + arg.ToPropertyPath());
                    return RemoveLeadingSlash(result);

                case ExpressionType.Lambda:
                    return ((LambdaExpression) expression).Body.ToPropertyPath();

                case ExpressionType.MemberAccess:
                case ExpressionType.Convert:
                    var memberExpression = (MemberExpression) expression;
                    string result2 = memberExpression.Expression.ToPropertyPath() + "/" + memberExpression.Member.Name;
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