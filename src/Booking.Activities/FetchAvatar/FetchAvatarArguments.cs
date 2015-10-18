namespace Booking.Activities.FetchAvatar
{
    using System;


    public interface FetchAvatarArguments
    {
        /// <summary>
        /// The unique identifier for the booking request
        /// </summary>
        Guid BookingRequestId { get; }

        /// <summary>
        /// Use the email address to fetch the avatar
        /// </summary>
        string EmailAddress { get; }
    }
}