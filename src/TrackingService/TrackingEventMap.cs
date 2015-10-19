namespace TrackingService
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Booking.Tracking;


    public class TrackingEventMap :
        EntityTypeConfiguration<TrackingEvent>
    {
        public TrackingEventMap()
        {
            HasKey(x => x.MessageId);
            Property(x => x.MessageId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.Timestamp);
            Property(x => x.Duration);

            Property(x => x.CorrelationId);
            Property(x => x.ConversationId);
            Property(x => x.InitiatorId);

            Property(x => x.BookingRequestId);

            Property(x => x.EventType);
            Property(x => x.Text).IsOptional();
        }
    }
}