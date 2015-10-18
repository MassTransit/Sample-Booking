namespace Booking.Contracts.Events
{
    using System;


    public interface BookingRequestNotAccepted
    {
        Guid BookingRequestId { get; }

        /// <summary>
        /// A reason code for the booking not being accepted
        /// </summary>
        int ReasonCode { get; }

        // A display reason of why the booking was not accepted
        string ReasonText { get; }
    }
}