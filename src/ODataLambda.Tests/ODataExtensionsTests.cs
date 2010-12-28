namespace ODataLambda.Tests
{
    using System.Data.Services.Client;
    using Extensions;
    using Fakes;
    using NUnit.Framework;

    [TestFixture]
    public class ODataExtensionsTests
    {
        private FakeContext fakeContext;
        private string orders;

        [SetUp]
        public void Setup()
        {
            fakeContext = new FakeContext();
            orders = fakeContext.BaseUri + "/Orders()?";
        }

        [Test]
        public void Should_expand_single_property()
        {
            var query = fakeContext.Orders.Expand(x => x.Product);

            query.UriShouldEqual(orders + "$expand=Product");
        }

        [Test]
        public void Should_expand_nested_property()
        {
            var query = fakeContext.Orders.Expand(x => x.Product.Id);

            query.UriShouldEqual(orders + "$expand=Product/Id");
        }

        [Test]
        public void Should_expand_deeply_recursively_nested_property()
        {
            var query = fakeContext.Orders.Expand(x => x.Product.Order.Product.Order.Product.Order.Product.Order.Product.Order);

            query.UriShouldEqual(orders + "$expand=Product/Order/Product/Order/Product/Order/Product/Order/Product/Order");
        }

        [Test]
        public void Should_expand_in_collection_nested_property()
        {
            var query = fakeContext.Orders.Expand(x => x.Products.Expand(y => y.Order));

            query.UriShouldEqual(orders + "$expand=Products/Order");
        }

        [Test]
        public void Should_expand_in_collection_deeply_nested_property()
        {
            var query = fakeContext.Orders.Expand(x => x.Products.Expand(y => y.Order.Product.Order.Product.Id));

            query.UriShouldEqual(orders + "$expand=Products/Order/Product/Order/Product/Id");
        }

        [Test]
        public void Should_expand_all_properties()
        {
            var query = fakeContext.Orders.ExpandAll();

            query.UriShouldEqual(orders + "$expand=Product,Products");
        }
    }
}