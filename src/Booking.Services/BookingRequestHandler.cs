namespace Booking.Services
{
    using System.Threading.Tasks;
    using Contracts.Commands;
    using MassTransit;
    using MassTransit.Courier;


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

            builder.AddActivity(_settings.ReserveRoomActivityName,
                _settings.ReserveRoomExecuteAddress, new
                {
                    ReservationApiKey = "secret"
                });

            builder.AddActivity(_settings.FetchAvatarActivityName, _settings.FetchAvatarExecuteAddress);

            builder.SetVariables(new
            {
                context.Message.EmailAddress,
                context.Message.StartTime,
                context.Message.Duration,
                context.Message.RoomCapacity
            });

            var routingSlip = builder.Build();

            await context.Execute(routingSlip);
        }
    }
}