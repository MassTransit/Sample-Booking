namespace Booking.Contracts.Events
{
    using System;
    using Commands;


    public interface BookingRequestReceived
    {
        DateTime Timestamp { get; }

        /// <summary>
        /// The actual meeting request
        /// </summary>
        BookMeeting Request { get; }
    }
}