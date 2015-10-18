namespace Booking.Services
{
    using System.Threading.Tasks;
    using Contracts.Commands;
    using MassTransit;


    public interface IBookingRequestHandler
    {
        Task BookMeeting(ConsumeContext<BookMeeting> context);
    }
}