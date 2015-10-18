namespace Booking.Activities.Tests
{
    using System;
    using System.Threading.Tasks;
    using Automatonymous;
    using Contracts.Events;
    using MassTransit;
    using MassTransit.Saga;
    using MassTransit.TestFramework;
    using NUnit.Framework;
    using Tracking;


    [TestFixture]
    public class BookingRequestTracking_Specs :
        InMemoryTestFixture
    {
        [Test]
        public async Task Should_insert_a_new_tracking_instance()
        {
            Guid sagaId = NewId.NextGuid();

            await Bus.Publish(new RequestReceived(sagaId, DateTime.UtcNow));

            Guid? foundSagaId = await _bookingRequestSagaRepository.ShouldContainSaga(sagaId, TestTimeout);

            Assert.IsTrue(foundSagaId.HasValue);
            Assert.AreEqual(sagaId, foundSagaId.Value);
        }


        public class RequestReceived :
            BookingRequestReceived
        {
            public RequestReceived(Guid bookingRequestId, DateTime timestamp)
            {
                BookingRequestId = bookingRequestId;
                Timestamp = timestamp;
            }

            public Guid BookingRequestId { get; }
            public DateTime Timestamp { get; }
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