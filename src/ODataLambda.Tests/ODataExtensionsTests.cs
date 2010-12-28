namespace ODataLambda.Tests
{
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
            var dataServiceQuery = fakeContext.Orders.Expand(x => x.Product);

            UriShouldContain(dataServiceQuery, "$expand=Product");
        }

        public static void UriShouldContain(DataServiceQuery query, string expected)
        {
            query.RequestUri.ToString().ShouldContain(expected);
        }
    }
}