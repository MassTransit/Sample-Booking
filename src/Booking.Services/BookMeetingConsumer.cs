namespace Booking.Services
{
    using System;
    using System.Threading.Tasks;
    using Contracts.Commands;
    using Contracts.Events;
    using MassTransit;


    /// <summary>
    /// Consumes the initial meeting request, and determines how to process it based on the delivery context
    /// </summary>
    public class BookMeetingConsumer :
        IConsumer<BookMeeting>
    {
        readonly IBookingRequestHandler _handler;

        public BookMeetingConsumer(IBookingRequestHandler handler)
        {
            _handler = handler;
        }

        public async Task Consume(ConsumeContext<BookMeeting> context)
        {
            await context.Publish(new Received(context.Message));

            // If the message was previously delivered, follow the slow path
            if (context.ReceiveContext.Redelivered)
            {
                await context.Publish(new Redelivered(context.Message));
            }
            else
            {
                await _handler.BookMeeting(context);
            }
        }


        class Received :
            BookingRequestReceived
        {
            public Received(BookMeeting request)
            {
                Timestamp = DateTime.UtcNow;
                Request = request;
            }

            public BookMeeting Request { get; }

            public DateTime Timestamp { get; }
        }

        class Redelivered :
            BookingRequestRedelivered
        {
            public Redelivered(BookMeeting request)
            {
                Timestamp = DateTime.UtcNow;
                Request = request;
            }

            public DateTime Timestamp { get; }
            public BookMeeting Request { get; }
        }
    }
}