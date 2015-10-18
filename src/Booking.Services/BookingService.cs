namespace Booking.Services
{
    using MassTransit;
    using MassTransit.Hosting;


    /// <summary>
    /// Configures the bus settings for the service and all endpoints in the same assembly.
    /// </summary>
    public class BookingService :
        IServiceSpecification
    {
        public void Configure(IServiceConfigurator configurator)
        {
            configurator.UseRetry(Retry.None);
        }
    }
}