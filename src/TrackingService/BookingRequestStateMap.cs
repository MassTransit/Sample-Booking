namespace TrackingService
{
    using Booking.Tracking;
    using MassTransit.EntityFrameworkIntegration;


    class BookingRequestStateMap :
        SagaClassMapping<BookingRequestState>
    {
        public BookingRequestStateMap()
        {
            Property(x => x.State);

            Property(x => x.CreateTime);
            Property(x => x.UpdateTime);

            Property(x => x.ReceiveTime);

            Property(x => x.StartTime);
            Property(x => x.Duration);
            Property(x => x.RoomCapacity);
            Property(x => x.EmailAddress);
        }
    }
}