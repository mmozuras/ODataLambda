namespace ODataLambda
{
    using System;
    using System.Data.Services.Client;
    using System.Linq.Expressions;

    /// <summary>
    /// Just a Facade over DataServiceContext, hides methods, that are not strongly typed.
    /// </summary>
    public class ODataLambdaContext<TContext> where TContext : DataServiceContext
    {
        /// <summary>
        /// Initializes a new ODataLambdaContext
        /// </summary>
        /// <param name="context"></param>
        public ODataLambdaContext(TContext context)
        {
            InnerContext = context;
        }

        /// <summary>
        /// The DataServiceContext, over which this class is a Facade.
        /// </summary>
        public TContext InnerContext { get; private set; }

        /// <summary>
        /// Creates a data service query for data of a specified generic type.
        /// </summary>
        public DataServiceQuery<T> Query<T>()
        {
            return InnerContext.CreateQuery<TContext, T>();
        }

        /// <summary>
        /// Adds the specified object to the set of objects that DataServiceContext is tracking.
        /// </summary>
        /// <param name="entity">The resource to be tracked by DataServiceContext in the added state.</param>
        public void Add<T>(T entity)
        {
            InnerContext.AddObject(entity);
        }

        /// <summary>
        /// Notifies DataServiceContext to start tracking the specified resource and supplies the location of the resource within the specified resource set.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Attach<T>(T entity)
        {
            InnerContext.Attach(entity);
        }

        /// <summary>
        /// Removes the entity from the list of entities that DataServiceContext is tracking.
        /// </summary>
        /// <param name="entity">The tracked entity to be detached from DataServiceContext.</param>
        public void Detach<T>(T entity)
        {
            InnerContext.Detach(entity);
        }

        /// <summary>
        /// Changes the state of the specified object to Deleted in DataServiceContext.
        /// </summary>
        /// <param name="entity">The tracked entity to be changed to the Deleted state.</param>
        public void Delete<T>(T entity)
        {
            InnerContext.DeleteObject(entity);
        }

        /// <summary>
        /// Changes the state of the specified object in DataServiceContext to Modified.
        /// </summary>
        /// <param name="entity">The tracked entity to be assigned to the Modified state.</param>
        public void Update<T>(T entity)
        {
            InnerContext.UpdateObject(entity);
        }

        /// <summary>
        /// Saves the changes DataServiceContext is tracking to storage.
        /// </summary>
        public void Save()
        {
            InnerContext.SaveChanges();
        }

        /// <summary>
        /// Notifies DataServiceContext that a new link exists between the objects specified and that the link is represented by the property specified by the sourceProperty parameter.
        /// </summary>
        /// <param name="source">The source object for the new link.</param>
        /// <param name="sourceProperty">The property on the source object that identifies the target object of the new link.</param>
        /// <param name="target">The child object involved in the new link that is to be initialized by calling this method. The target object must be a subtype of the type identified by the sourceProperty parameter. If target is set to null, the call represents a delete link operation.</param>
        public void SetLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            InnerContext.SetLink(source, sourceProperty, target);
        }

        /// <summary>
        /// Adds the specified link to the set of objects DataServiceContext is tracking.
        /// </summary>
        /// <param name="source">The source object for the new link.</param>
        /// <param name="sourceProperty">The property on the source object that returns the related object.</param>
        /// <param name="target">The object related to the source object by the new link. </param>
        public void AddLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            InnerContext.AddLink(source, sourceProperty, target);
        }

        /// <summary>
        /// Changes the state of the link to deleted in the list of links being tracked by DataServiceContext.
        /// </summary>
        /// <param name="source">The source object in the link to be marked for deletion.</param>
        /// <param name="sourceProperty">The property on the source object that is used to access the target object.</param>
        /// <param name="target">The target object involved in the link that is bound to the source object. The target object must be of the type identified by the source property or a subtype.</param>
        public void DeleteLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            InnerContext.DeleteLink(source, sourceProperty, target);
        }

        /// <summary>
        /// Notifies DataServiceContext to start tracking the specified link that defines a relationship between entity objects.
        /// </summary>
        /// <param name="source">The source object in the new link.</param>
        /// <param name="sourceProperty">The property on the source object that represents the link between the source and target object.</param>
        /// <param name="target">The target object in the link that is bound to the source object specified in this call. The target object must be of the type identified by the source property or a subtype.</param>
        public void AttachLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            InnerContext.AttachLink(source, sourceProperty, target);
        }

        /// <summary>
        /// Removes the specified link from the list of links being tracked by DataServiceContext.
        /// </summary>
        /// <param name="source">The source object participating in the link to be marked for deletion.</param>
        /// <param name="sourceProperty">The property on the source object that represents the source in the link between the source and the target.</param>
        /// <param name="target">The target object involved in the link that is bound to the source object. The target object must be of the type identified by the source property or a subtype.</param>
        public void DetachLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
        {
            InnerContext.DetachLink(source, sourceProperty, target);
        }

        /// <summary>
        /// Loads deferred content for a specified property from the data service.
        /// </summary>
        /// <param name="entity">The entity that contains the property to load.</param>
        /// <param name="property">The property to load.</param>
        public void LoadProperty<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
        {
            InnerContext.LoadProperty(entity, property);
        }

        /// <summary>
        /// Loads deferred content for a specified property from the data service.
        /// </summary>
        /// <param name="entity">The entity that contains the property to load.</param>
        /// <param name="property">The property to load.</param>
        /// <param name="continuation">A DataServiceQueryContinuation object that represents the next page of related entities to load from the data service.</param>
        public void LoadProperty<T, TProperty>(T entity, Expression<Func<T, TProperty>> property,
                                               DataServiceQueryContinuation<T> continuation)
        {
            InnerContext.LoadProperty(entity, property, continuation);
        }

        /// <summary>
        /// Loads deferred content for all properties.
        /// </summary>
        /// <param name="entity">The entity that contains the properties to load.</param>
        public void LoadAllProperties<T>(T entity)
        {
            InnerContext.LoadAllProperties(entity);
        }
    }
}