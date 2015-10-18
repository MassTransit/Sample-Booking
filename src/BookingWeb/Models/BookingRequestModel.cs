namespace BookingWeb.Models
{
    using System;


    public class BookingRequestModel
    {
        public Guid BookingRequestId { get; set; }

        public string EmailAddress { get; set; }

        public DateTime StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public int RoomCapacity { get; set; }
    }
}