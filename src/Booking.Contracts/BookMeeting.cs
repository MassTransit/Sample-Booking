namespace Booking.Contracts
{
    using System;


    public interface BookMeeting
    {
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