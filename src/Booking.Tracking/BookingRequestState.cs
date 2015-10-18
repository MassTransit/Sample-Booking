namespace Booking.Tracking
{
    using System;
    using Automatonymous;


    public class BookingRequestState :
        SagaStateMachineInstance
    {
        /// <summary>
        /// The state of the saga
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// When the booking request was received
        /// </summary>
        public DateTime? ReceiveTime { get; set; }

        /// <summary>
        /// The correlationId of the saga state instance (matches the booking requestId)
        /// </summary>
        public Guid CorrelationId { get; set; }


        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public string EmailAddress { get; set; }
        public int? RoomCapacity { get; set; }
        public DateTime? StartTime { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}