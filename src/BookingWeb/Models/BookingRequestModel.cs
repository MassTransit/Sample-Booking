namespace BookingWeb.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;


    public class BookingRequestModel
    {
        public Guid BookingRequestId { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public int RoomCapacity { get; set; }
    }
}