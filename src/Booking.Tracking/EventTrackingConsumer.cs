namespace Booking.Tracking
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts.Events;
    using MassTransit;
    using MassTransit.Courier.Contracts;
    using Newtonsoft.Json;


    public class EventTrackingConsumer :
        IConsumer<RoutingSlipCompleted>,
        IConsumer<RoutingSlipActivityCompleted>,
        IConsumer<RoutingSlipActivityFaulted>,
        IConsumer<RoutingSlipFaulted>,
        IConsumer<BookingRequestReceived>

    {
        readonly ITrackingEventWriter _writer;

        public EventTrackingConsumer(ITrackingEventWriter writer)
        {
            _writer = writer;
        }

        public Task Consume(ConsumeContext<BookingRequestReceived> context)
        {
            return WriteEvent(context, context.Message.Timestamp,
                JsonConvert.SerializeObject(new
                {
                    context.Message.Request.EmailAddress,
                    context.Message.Request.StartTime,
                    context.Message.Request.Duration,
                    context.Message.Request.RoomCapacity
                }), context.Message.Request.BookingRequestId);
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityCompleted> context)
        {
            return WriteEvent(context, context.Message.Timestamp, context.Message.ActivityName,
                duration: context.Message.Duration);
        }

        public Task Consume(ConsumeContext<RoutingSlipActivityFaulted> context)
        {
            return WriteEvent(context, context.Message.Timestamp, context.Message.ActivityName,
                duration: context.Message.Duration);
        }

        public Task Consume(ConsumeContext<RoutingSlipCompleted> context)
        {
            return WriteEvent(context, context.Message.Timestamp, duration: context.Message.Duration);
        }

        public Task Consume(ConsumeContext<RoutingSlipFaulted> context)
        {
            return WriteEvent(context, context.Message.Timestamp, duration: context.Message.Duration,
                text: JsonConvert.SerializeObject(new
                {
                    Exceptions = context.Message.ActivityExceptions.Select(x => new { x.ExceptionInfo.ExceptionType, x.ExceptionInfo.Message}).ToList(),
                    context.Message.TrackingNumber,
                }));
        }

        Task WriteEvent<T>(ConsumeContext<T> context, DateTime timestamp, string text = default(string), Guid? bookingRequestId = default(Guid?),
            TimeSpan? duration = default(TimeSpan?))
            where T : class
        {
            var trackingEvent = new TrackingEvent
            {
                MessageId = context.MessageId ?? NewId.NextGuid(),
                Timestamp = timestamp,
                Duration = duration,
                Text = text,
                CorrelationId = context.CorrelationId,
                ConversationId = context.ConversationId,
                InitiatorId = context.InitiatorId,
                BookingRequestId = bookingRequestId,
                EventType = typeof(T).Name
            };

            return _writer.Write(trackingEvent);
        }
    }
}