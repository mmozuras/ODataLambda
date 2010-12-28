namespace ODataLambda.Tests.Fakes
{
    using System.Data.Services.Common;

    [DataServiceKey("Id")]
    public class FakeProduct
    {
        public int Id { get; set; }
        public FakeOrder Order { get; set; }
    }
}