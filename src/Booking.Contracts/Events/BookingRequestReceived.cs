namespace Booking.Contracts.Events
{
    using System;


    public interface BookingRequestReceived
    {
        Guid BookingRequestId { get; }

        DateTime Timestamp { get; }
    }
}