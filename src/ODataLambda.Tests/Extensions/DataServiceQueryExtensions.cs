namespace ODataLambda.Tests.Extensions
{
    using System.Data.Services.Client;
    using Should;

    public static class DataServiceQueryExtensions
    {
        public static void UriShouldEqual(this DataServiceQuery query, string expected)
        {
            query.RequestUri.ToString().ShouldEqual(expected);
        }
    }
}