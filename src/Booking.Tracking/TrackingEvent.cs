namespace Booking.Tracking
{
    using System;


    public class TrackingEvent
    {
        public Guid MessageId { get; set; }
        public DateTime Timestamp { get; set; }

        public Guid? CorrelationId { get; set; }
        public Guid? ConversationId { get; set; }
        public Guid? InitiatorId { get; set; }

        public Guid? BookingRequestId { get; set; }

        public string EventType { get; set; }

        public string Text { get; set; }

        public TimeSpan? Duration { get; set; }
    }
}