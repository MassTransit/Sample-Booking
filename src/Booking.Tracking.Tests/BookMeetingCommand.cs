namespace Booking.Tracking.Tests
{
    using System;
    using Contracts.Commands;


    public class BookMeetingCommand :
        BookMeeting
    {
        public BookMeetingCommand(Guid bookingRequestId, string emailAddress, DateTime startTime, TimeSpan duration, int roomCapacity)
        {
            BookingRequestId = bookingRequestId;
            EmailAddress = emailAddress;
            StartTime = startTime;
            Duration = duration;
            RoomCapacity = roomCapacity;

            Timestamp = DateTime.UtcNow;
        }

        public Guid BookingRequestId { get; }

        public DateTime Timestamp { get; }

        public string EmailAddress { get; }

        public DateTime StartTime { get; }

        public TimeSpan Duration { get; }

        public int RoomCapacity { get; }
    }
}