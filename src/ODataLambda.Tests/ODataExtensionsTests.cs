namespace ODataLambda.Tests
{
    using System;
    using System.Data.Services.Client;
    using Fakes;
    using NUnit.Framework;
    using Should;

    public class ODataExtensionsTests
    {
        private FakeContext fakeContext;

        [SetUp]
        public void Setup()
        {
            fakeContext = new FakeContext();
        }

        [Test]
        public void Should_expand_single_property()
        {
            var query = fakeContext.Orders.Expand(x => x.Product);

            UriShouldContain(query, "$expand=Product");
        }

        [Test]
        public void Should_expand_nested_property()
        {
            var query = fakeContext.Orders.Expand(x => x.Product.Id);

            UriShouldContain(query, "$expand=Product/Id");
        }

        [Test]
        public void Should_expand_deeply_recursively_nested_property()
        {
            var query = fakeContext.Orders.Expand(x => x.Product.Order.Product.Order.Product.Order.Product.Order.Product.Order);

            UriShouldContain(query, "$expand=Product/Order/Product/Order/Product/Order/Product/Order/Product/Order");
        }

        [Test]
        public void Should_expand_in_collection_nested_property()
        {
            var query = fakeContext.Orders.Expand(x => x.Products.Expand(y => y.Order));

            UriShouldContain(query, "$expand=Products/Order");
        }

        private static void UriShouldContain(DataServiceQuery query, string expected)
        {
            string requestUri = query.RequestUri.ToString();
            Console.WriteLine(requestUri);
            requestUri.ShouldContain(expected);
        }
    }
}