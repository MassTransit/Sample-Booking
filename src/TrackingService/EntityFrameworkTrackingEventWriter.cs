namespace TrackingService
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Booking.Tracking;


    public class EntityFrameworkTrackingEventWriter :
        ITrackingEventWriter
    {
        readonly Func<DbContext> _contextFactory;

        public EntityFrameworkTrackingEventWriter(Func<DbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task Write(TrackingEvent trackingEvent)
        {
            using (var context = _contextFactory())
            {
                context.Set<TrackingEvent>().Add(trackingEvent);

                await context.SaveChangesAsync();
            }
        }
    }
}