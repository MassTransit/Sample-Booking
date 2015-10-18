namespace Booking.Tracking
{
    using System;
    using Automatonymous;
    using Contracts.Commands;
    using Contracts.Events;


    public class BookingRequestStateMachine :
        MassTransitStateMachine<BookingRequestState>
    {
        public BookingRequestStateMachine()
        {
            InstanceState(state => state.State, Received);

            Event(() => RequestReceived, x =>
            {
                x.CorrelateById(context => context.Message.Request.BookingRequestId);
                x.InsertOnInitial = true;
                x.SetSagaFactory(context => CreateBookingRequestState(context.Message.Timestamp, context.Message.Request));
            });

            Initially(
                When(RequestReceived)
                    .Then(context => context.Instance.ReceiveTime = context.Data.Timestamp)
                    .TransitionTo(Received),
                When(RequestRedelivered)
                    .Then(context => context.Instance.ReceiveTime = context.Data.Timestamp)
                    .Then(context =>
                    {
                        // send the command to execute the routing slip, because it is new
                    })
                    .TransitionTo(Executing));
        }

        public State Received { get; private set; }
        public State Executing { get; private set; }

        public Event<BookingRequestReceived> RequestReceived { get; private set; }
        public Event<BookingRequestRedelivered> RequestRedelivered { get; private set; }

        static BookingRequestState CreateBookingRequestState(DateTime timestamp, BookMeeting request)
        {
            return new BookingRequestState
            {
                CreateTime = timestamp,
                UpdateTime = timestamp,
                EmailAddress = request.EmailAddress,
                StartTime = request.StartTime,
                Duration = request.Duration,
                RoomCapacity = request.RoomCapacity
            };
        }
    }
}