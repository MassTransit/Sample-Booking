namespace BookingWeb.Controllers
{
    using System.Web.Http;


    public abstract class EnhancedApiController :
        ApiController
    {
        protected IHttpActionResult Accepted<T>(T value)
        {
            return new AcceptedActionResult<T>(Request, value);
        }
    }
}