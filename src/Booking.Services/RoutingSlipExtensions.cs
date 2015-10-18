namespace Booking.Services
{
    using System.Threading.Tasks;
    using MassTransit;
    using MassTransit.Courier;
    using MassTransit.Courier.Contracts;


    public static class RoutingSlipExtensions
    {
        /// <summary>
        ///  This is a temp workaround until I update MT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bus"></param>
        /// <param name="routingSlip"></param>
        /// <returns></returns>
        public static async Task Execute<T>(this T bus, RoutingSlip routingSlip)
            where T : IPublishEndpoint, ISendEndpointProvider
        {
            var endpoint = await bus.GetSendEndpoint(routingSlip.GetNextExecuteAddress());

            await endpoint.Send(routingSlip);
        }
    }
}