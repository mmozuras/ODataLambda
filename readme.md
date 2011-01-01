# ODataLambda

Strongly typed helpers to work with [OData][1]

Instead of littering your .NET OData queries with strings (`context.AddLink("Products") context.Orders.Expand("Product")`), you can use strongly typed helpers to simplify your development.

A [Facade][2] over [DataServiceContext][3] is also provided, called ODataLambdaContext. ODataLambdaContext has these methods and properties:
    
    TContext InnerContext { get; private set; }
    DataServiceQuery<T> Query<T>()
    void Add<T>(T entity)
    void Attach<T>(T entity)
    void Detach<T>(T entity)
    void Delete<T>(T entity)
    void Update<T>(T entity)
    void Save()
    void SetLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
    void AddLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
    void DeleteLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
    void AttachLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
    void DetachLink<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> sourceProperty, object target)
    void LoadProperty<T, TProperty>(T entity, Expression<Func<T, TProperty>> property)
    void LoadProperty<T, TProperty>(T entity, Expression<Func<T, TProperty>> property, DataServiceQueryContinuation<T> continuation)
    void LoadAllProperties<T>(T entity)
                                           


  [1]: http://www.odata.org/
  [2]: http://en.wikipedia.org/wiki/Facade_pattern
  [3]: http://msdn.microsoft.com/en-us/library/cc679618.aspx