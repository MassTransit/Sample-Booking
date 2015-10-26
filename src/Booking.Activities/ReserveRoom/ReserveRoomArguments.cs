namespace Booking.Activities.ReserveRoom
{
    public interface ReserveRoomArguments
    {
        /// <summary>
        /// Key used with room reservation service
        /// </summary>
        string ReservationApiKey { get; }

        /// <summary>
        /// Capacity of room, pulled from variables
        /// </summary>
        int RoomCapacity { get; }
    }
}