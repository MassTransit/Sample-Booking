namespace Booking.Contracts
{
    using System;


    public interface BookingRequestNotProcessed
    {
        Guid BookingRequestId { get; }

        /// <summary>
        /// A reason code for the booking not being processed
        /// </summary>
        int ReasonCode { get; }

        // A display reason of why the booking was not processed
        string ReasonText { get; }
    }
}