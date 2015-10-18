namespace BookingWeb.Controllers
{
    using System;
    using System.Configuration;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Booking.Contracts.Commands;
    using MassTransit;
    using Models;


    public class BookController :
        EnhancedApiController
    {
        // POST api/book
        public async Task<IHttpActionResult> Post(BookingRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (model.BookingRequestId == Guid.Empty)
                model.BookingRequestId = NewId.NextGuid();

            var address = ConfigurationManager.AppSettings["BookMeetingAddress"];

            var endpoint = await WebApiApplication.Bus.GetSendEndpoint(new Uri(address));

            var request = new BookMeetingCommand(model);

            await endpoint.Send(request);

            return Accepted(new BookingRequestResult
            {
                BookingRequestId = model.BookingRequestId,
                Timestamp = request.Timestamp
            });
        }


        class BookMeetingCommand :
            BookMeeting
        {
            readonly BookingRequestModel _model;

            public BookMeetingCommand(BookingRequestModel model)
            {
                _model = model;

                Timestamp = DateTime.UtcNow;
            }

            public Guid BookingRequestId => _model.BookingRequestId;

            public DateTime Timestamp { get; }

            public string EmailAddress => _model.EmailAddress;
            public DateTime StartTime => _model.StartTime;
            public TimeSpan Duration => _model.Duration;
            public int RoomCapacity => _model.RoomCapacity;
        }
    }
}