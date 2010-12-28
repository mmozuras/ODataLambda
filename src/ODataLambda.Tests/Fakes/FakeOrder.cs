namespace ODataLambda.Tests.Fakes
{
    using System.Data.Services.Client;
    using System.Data.Services.Common;

    [DataServiceKey("Id")]
    public class FakeOrder
    {
        public int Id { get; set; }
        public FakeProduct Product { get; set; }
        public DataServiceCollection<FakeProduct> Products { get; set; }
    }
}