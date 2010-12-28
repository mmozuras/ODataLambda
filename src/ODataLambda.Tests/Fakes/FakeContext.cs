namespace ODataLambda.Tests.Fakes
{
    using System;
    using System.Data.Services.Client;

    public class FakeContext : DataServiceContext
    {
        public FakeContext() : base(new Uri("http://localhost/Fake"))
        {
            Orders = CreateQuery<FakeOrder>("Orders");
        }

        public DataServiceQuery<FakeOrder> Orders { get; private set; }
    }
}