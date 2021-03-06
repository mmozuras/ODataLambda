namespace ODataLambda.Tests
{
    using System.Data.Services.Client;
    using System.Linq;
    using Extensions;
    using Fakes;
    using NUnit.Framework;
    using Should;

    [TestFixture]
    public class ODataExtensionsTests
    {
        private FakeContext fakeContext;
        private string orders;
        private FakeOrder fakeOrder;
        private FakeProduct fakeProduct;

        [SetUp]
        public void Setup()
        {
            fakeContext = new FakeContext();
            fakeOrder = new FakeOrder();
            fakeProduct = new FakeProduct();

            fakeContext.AddObject("Orders", fakeOrder);
            fakeContext.AddObject("Product", fakeProduct);

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

        [Test]
        public void Should_add_link()
        {
            fakeContext.AddLink(fakeOrder, x => x.Products, fakeProduct);

            LinkDescriptor link = fakeContext.Links.Single();
            link.Source.ShouldEqual(fakeOrder);
            link.SourceProperty.ShouldEqual("Products");
            link.Target.ShouldEqual(fakeProduct);
        }

        [Test]
        public void Should_set_link()
        {
            fakeContext.SetLink(fakeOrder, x => x.Product, fakeProduct);

            LinkDescriptor link = fakeContext.Links.Single();
            link.Source.ShouldEqual(fakeOrder);
            link.SourceProperty.ShouldEqual("Product");
            link.Target.ShouldEqual(fakeProduct);
        }

        [Test]
        public void Should_delete_link()
        {
            fakeContext.AddLink(fakeOrder, x => x.Products, fakeProduct);
            fakeContext.DeleteLink(fakeOrder, x => x.Products, fakeProduct);

            fakeContext.Links.ShouldBeEmpty();
        }

        [Test]
        public void Should_add_object_without_providing_set_name()
        {
            var order = new FakeOrder();
            fakeContext.AddObject(order);
            fakeContext.Entities.SingleOrDefault(x => x.Entity == order).State.ShouldEqual(EntityStates.Added);
        }

        [Test]
        public void Should_attach_object_without_providing_set_name()
        {
            var order = new FakeOrder();
            fakeContext.Attach(order);
            fakeContext.Entities.SingleOrDefault(x => x.Entity == order).State.ShouldEqual(EntityStates.Unchanged);
        }

        [Test]
        public void Should_create_query_without_providing_set_name()
        {
            DataServiceQuery<FakeOrder> query = fakeContext.CreateQuery<FakeContext, FakeOrder>();
            query.UriShouldEqual("http://localhost/Fake/Orders");
        }
    }
}