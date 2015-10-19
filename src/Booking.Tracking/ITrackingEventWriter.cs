namespace Booking.Tracking
{
    using System.Threading.Tasks;


    public interface ITrackingEventWriter
    {
        Task Write(TrackingEvent trackingEvent);
    }
}