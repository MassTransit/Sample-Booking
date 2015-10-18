namespace TrackingService
{
    using System;
    using System.Configuration;
    using Automatonymous;
    using Booking.Tracking;
    using MassTransit;
    using MassTransit.EntityFrameworkIntegration;
    using MassTransit.EntityFrameworkIntegration.Saga;
    using MassTransit.Saga;
    using MassTransit.Util;
    using Topshelf;
    using Topshelf.Logging;


    class TrackingService :
        ServiceControl
    {
        readonly LogWriter _log = HostLogger.Get<TrackingService>();
        readonly BookingRequestStateMachine _stateMachine;

        IBusControl _busControl;
        BusHandle _busHandle;
        SagaDbContextFactory _sagaDbContextFactory;
        ISagaRepository<BookingRequestState> _sagaRepository;

        public TrackingService()
        {
            _stateMachine = new BookingRequestStateMachine();
        }

        public bool Start(HostControl hostControl)
        {
            _log.Info("Creating bus...");

            _sagaRepository = GetSagaRepository();

            using (var context = _sagaDbContextFactory())
            {
                context.Database.CreateIfNotExists();
            }

            _busControl = Bus.Factory.CreateUsingRabbitMq(x =>
            {
                var host = x.Host(GetHostAddress(), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUsername"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });

                x.ReceiveEndpoint(host, ConfigurationManager.AppSettings["BookingStateQueueName"], e =>
                {
                    e.StateMachineSaga(_stateMachine, _sagaRepository);
                });
            });

            _log.Info("Starting bus...");

            _busHandle = _busControl.Start();

            TaskUtil.Await(() => _busHandle.Ready);

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _log.Info("Stopping bus...");

            _busHandle?.Stop(TimeSpan.FromSeconds(30));

            return true;
        }

        ISagaRepository<BookingRequestState> GetSagaRepository()
        {
            _sagaDbContextFactory = () => new SagaDbContext<BookingRequestState, BookingRequestStateMap>(LocalDatabaseSelector.ConnectionString);

            return new EntityFrameworkSagaRepository<BookingRequestState>(_sagaDbContextFactory);
        }

        static Uri GetHostAddress()
        {
            var uriBuilder = new UriBuilder
            {
                Scheme = "rabbitmq",
                Host = ConfigurationManager.AppSettings["RabbitMQHost"]
            };

            return uriBuilder.Uri;
        }
    }
}