namespace ODataLambda.Tests
{
    using System;
    using System.Linq;
    using MicrosoftPdcData;
    using NUnit.Framework;
    using Should;

    [TestFixture]
    public class ODataExtensionsIntegrationTests
    {
        private ScheduleModel context;

        [SetUp]
        public void Setup()
        {
            context = new ScheduleModel(new Uri("http://odata.microsoftpdc.com/ODataSchedule.svc/"));
        }

        [Test]
        public void Should_load_single_property()
        {
            var session = context.Sessions.First();

            session.Manifests.ShouldBeEmpty();

            context.LoadProperty(session, x => x.Manifests);

            session.Manifests.ShouldNotBeEmpty();
        }

        [Test]
        public void Should_load_all_properties()
        {
            var session = context.Sessions.First();

            session.Manifests.ShouldBeEmpty();
            session.DownloadableContent.ShouldBeEmpty();

            context.LoadAllProperties(session);

            session.Manifests.ShouldNotBeEmpty();
            session.DownloadableContent.ShouldNotBeEmpty();
        }

        [Test]
        public void Should_detach_and_attach_link()
        {
            var session = context.Sessions.Expand(x => x.Manifests).First();
            Manifest firstManifest = session.Manifests.First();

            context.DetachLink(session, x => x.Manifests, firstManifest);
            context.AttachLink(session, x => x.Manifests, firstManifest);
        }
    }
}