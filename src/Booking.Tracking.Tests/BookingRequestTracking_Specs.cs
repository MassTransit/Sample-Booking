namespace Booking.Activities.Tests
{
    using System;
    using System.Threading.Tasks;
    using Automatonymous;
    using Contracts.Commands;
    using Contracts.Events;
    using MassTransit;
    using MassTransit.Saga;
    using MassTransit.TestFramework;
    using NUnit.Framework;
    using Tracking;
    using Tracking.Tests;


    [TestFixture]
    public class BookingRequestTracking_Specs :
        InMemoryTestFixture
    {
        [Test]
        public async Task Should_insert_a_new_tracking_instance()
        {
            Guid sagaId = NewId.NextGuid();

            var command = new BookMeetingCommand(sagaId, "chris@phatboyg.com", new DateTime(2016, 10, 1, 14, 0, 0), TimeSpan.FromHours(1), 8);
            await Bus.Publish(new RequestReceived(sagaId, command));

            Guid? foundSagaId = await _bookingRequestSagaRepository.ShouldContainSaga(sagaId, TestTimeout);

            Assert.IsTrue(foundSagaId.HasValue);
            Assert.AreEqual(sagaId, foundSagaId.Value);
        }


        public class RequestReceived :
            BookingRequestReceived
        {
            public RequestReceived(Guid bookingRequestId, BookMeeting request)
            {
                BookingRequestId = bookingRequestId;
                Timestamp = DateTime.UtcNow;
                Request = request;
            }

            public Guid BookingRequestId { get; }
            public DateTime Timestamp { get; }

            public BookMeeting Request { get; }
        }


        InMemorySagaRepository<BookingRequestState> _bookingRequestSagaRepository;
        BookingRequestStateMachine _bookingRequestStateMachine;

        protected override void ConfigureInputQueueEndpoint(IReceiveEndpointConfigurator configurator)
        {
            _bookingRequestSagaRepository = new InMemorySagaRepository<BookingRequestState>();
            _bookingRequestStateMachine = new BookingRequestStateMachine();

            configurator.StateMachineSaga(_bookingRequestStateMachine, _bookingRequestSagaRepository);
        }
    }
}