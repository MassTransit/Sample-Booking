namespace Booking.Contracts.Events
{
    using System;
    using Commands;


    /// <summary>
    /// This event is published when a booking request is redelivered, either by the transport
    /// or due to an exception handling case
    /// </summary>
    public interface BookingRequestRedelivered
    {
        /// <summary>
        /// The time the request was redelivered
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// The actual meeting request
        /// </summary>
        BookMeeting Request { get; }
    }
}