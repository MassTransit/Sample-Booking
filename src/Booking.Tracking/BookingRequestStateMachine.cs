namespace Booking.Tracking
{
    using Automatonymous;
    using Contracts.Events;


    public class BookingRequestStateMachine :
        MassTransitStateMachine<BookingRequestState>
    {
        public BookingRequestStateMachine()
        {
            InstanceState(state => state.State, Received);

            Event(() => RequestReceived, x =>
            {
                x.CorrelateById(context => context.Message.BookingRequestId);
                x.InsertOnInitial = true;
            });

            Initially(
                When(RequestReceived)
                    .Then(context => context.Instance.ReceiveTime = context.Data.Timestamp)
                    .TransitionTo(Received));
        }

        public State Received { get; private set; }

        public Event<BookingRequestReceived> RequestReceived { get; private set; }
    }
}