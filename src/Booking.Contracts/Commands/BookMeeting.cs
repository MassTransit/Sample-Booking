namespace Booking.Contracts.Commands
{
    using System;


    public interface BookMeeting
    {
        /// <summary>
        /// Uniquely identifies the booking request
        /// </summary>
        Guid BookingRequestId { get; }

        /// <summary>
        /// The timestamp of the booking request
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// The email address of the booking party
        /// </summary>
        string EmailAddress { get; }

        /// <summary>
        /// The start time of the meeting to book
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// The meeting duration
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// The room capacity required for the meeting
        /// </summary>
        int RoomCapacity { get; }
    }
}