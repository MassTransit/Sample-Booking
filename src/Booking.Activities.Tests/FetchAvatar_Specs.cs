namespace Booking.Activities.Tests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using FetchAvatar;
    using MassTransit;
    using MassTransit.Courier;
    using MassTransit.Courier.Contracts;
    using MassTransit.TestFramework;
    using NUnit.Framework;


    [TestFixture]
    public class FetchAvatar_Specs :
        InMemoryActivityTestFixture
    {
        [Test]
        public async Task Should_write_avatar_address_to_routing_slip_variables()
        {
            var completed = await _completed;

            var avatarFileName = completed.Message.GetVariable<string>("AvatarFileName");

            Assert.IsNotNullOrEmpty(avatarFileName);

            Console.WriteLine($"Avatar path: {avatarFileName}");
        }

        [TestFixtureSetUp]
        public void Setup()
        {
            var builder = new RoutingSlipBuilder(Guid.NewGuid());

            var fetchAvatarActivity = GetActivityContext<FetchAvatarActivity>();
            builder.AddActivity(fetchAvatarActivity.Name, fetchAvatarActivity.ExecuteUri);

            builder.AddVariable("EmailAddress", "chris@phatboyg.com");
            builder.AddVariable("BookingRequestId", NewId.NextGuid());

            _routingSlip = builder.Build();

            Await(() => Bus.Execute(_routingSlip));
        }

        Task<ConsumeContext<RoutingSlipCompleted>> _completed;
        Task<ConsumeContext<RoutingSlipActivityCompleted>> _activityCompleted;
        RoutingSlip _routingSlip;

        protected override void ConfigureInputQueueEndpoint(IReceiveEndpointConfigurator configurator)
        {
            _completed = Handled<RoutingSlipCompleted>(configurator);

            var fetchAvatarActivity = GetActivityContext<FetchAvatarActivity>();

            _activityCompleted = Handled<RoutingSlipActivityCompleted>(configurator, context => context.Message.ActivityName.Equals(fetchAvatarActivity.Name));
        }

        protected override void SetupActivities()
        {
            SetupFetchAvatarActivity();
        }

        void SetupFetchAvatarActivity()
        {
            var avatarPath = CreateAvatarPath();

            FetchAvatarActivitySettings fetchAvatarActivitySettings = new TestFetchAvatarActivitySettings(avatarPath);

            AddActivityContext<FetchAvatarActivity, FetchAvatarArguments>(() => new FetchAvatarActivity(fetchAvatarActivitySettings));
        }

        static string CreateAvatarPath()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var avatarPath = Path.Combine(baseDirectory, "avatars");

            Directory.CreateDirectory(avatarPath);

            return avatarPath;
        }


        class TestFetchAvatarActivitySettings :
            FetchAvatarActivitySettings
        {
            public TestFetchAvatarActivitySettings(string cacheFolderPath)
            {
                CacheFolderPath = cacheFolderPath;
            }

            public string CacheFolderPath { get; }
        }
    }
}