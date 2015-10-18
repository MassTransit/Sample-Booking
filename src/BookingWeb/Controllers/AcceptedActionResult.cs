namespace BookingWeb.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;


    public class AcceptedActionResult<T> :
        IHttpActionResult
    {
        readonly HttpRequestMessage _request;
        readonly T _value;

        public AcceptedActionResult(HttpRequestMessage request, T value)
        {
            _request = request;
            _value = value;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.Accepted, _value);

            return Task.FromResult(response);
        }
    }
}