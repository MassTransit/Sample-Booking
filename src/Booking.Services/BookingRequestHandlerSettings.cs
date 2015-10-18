namespace Booking.Services
{
    using System;
    using MassTransit.Hosting;


    public interface BookingRequestHandlerSettings :
        ISettings
    {
        /// <summary>
        /// The display name of the activity
        /// </summary>
        string FetchAvatarActivityName { get; }

        /// <summary>
        /// The execute address of the activity for the routing slip
        /// </summary>
        Uri FetchAvatarExecuteAddress { get; }

        /// <summary>
        /// The address where routing slip events should be sent
        /// </summary>
        Uri RoutingSlipEventSubscriptionAddress { get; }
    }
}