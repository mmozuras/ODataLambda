# ODataLambda

Strongly typed helpers to work with OData

Instead of littering your .NET OData queries with strings (`context.AddLink("Products") context.Orders.Expand("Product")`), you can use strongly typed helpers to simplify your development (this is not a full list of provided helpers):
    
    context.AddLink(order, x => x.Products, product);
    context.SetLink(order, x => x.Product, product);    
    context.Orders.Expand(x => x.Products.Expand(y => y.Order));
    context.Orders.ExpandAll();
	context.LoadProperty(order, x => x.Product);
	context.LoadAllProperties(order);
