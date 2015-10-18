namespace Booking.Services
{
    using System.Threading.Tasks;
    using Contracts.Commands;
    using MassTransit;
    using MassTransit.Courier;
    using MassTransit.Courier.Contracts;


    public class BookingRequestHandler :
        IBookingRequestHandler
    {
        readonly BookingRequestHandlerSettings _settings;

        public BookingRequestHandler(BookingRequestHandlerSettings settings)
        {
            _settings = settings;
        }

        public async Task BookMeeting(ConsumeContext<BookMeeting> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            builder.AddActivity(_settings.FetchAvatarActivityName, _settings.FetchAvatarExecuteAddress);

            builder.SetVariables(new
            {
                context.Message.EmailAddress,
                context.Message.StartTime,
                context.Message.Duration,
                context.Message.RoomCapacity
            });

            builder.AddSubscription(_settings.RoutingSlipEventSubscriptionAddress, RoutingSlipEvents.Completed | RoutingSlipEvents.Faulted);

            var routingSlip = builder.Build();

            await context.Execute(routingSlip);
        }
    }
}