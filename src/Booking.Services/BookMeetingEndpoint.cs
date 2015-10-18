namespace Booking.Services
{
    using System;
    using MassTransit;
    using MassTransit.Hosting;


    /// <summary>
    /// Configures an endpoint for the assembly
    /// </summary>
    public class BookMeetingEndpoint :
        IEndpointSpecification
    {
        readonly IConsumerFactory<BookMeetingConsumer> _consumerFactory;

        public BookMeetingEndpoint(IConsumerFactory<BookMeetingConsumer> consumerFactory)
        {
            _consumerFactory = consumerFactory;
        }

        /// <summary>
        /// The default queue name for the endpoint, which can be overridden in the .config 
        /// file for the assembly
        /// </summary>
        public string QueueName => "book-meeting";

        /// <summary>
        /// The default concurrent consumer limit for the endpoint, which can be overridden in the .config 
        /// file for the assembly
        /// </summary>
        public int ConsumerLimit => Environment.ProcessorCount;

        /// <summary>
        /// Configures the endpoint, with consumers, handlers, sagas, etc.
        /// </summary>
        public void Configure(IReceiveEndpointConfigurator configurator)
        {
            configurator.Consumer(_consumerFactory);
        }
    }
}