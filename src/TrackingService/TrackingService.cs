namespace TrackingService
{
    using System;
    using System.Configuration;
    using System.Data.Entity;
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

            ITrackingEventWriter writer = GetTrackingEventWriter();

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

                x.ReceiveEndpoint(host, ConfigurationManager.AppSettings["EventTrackingQueueName"], e =>
                {
                    e.Consumer(() => new EventTrackingConsumer(writer));
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

            using (var context = _sagaDbContextFactory())
            {
                context.Database.CreateIfNotExists();
            }

            return new EntityFrameworkSagaRepository<BookingRequestState>(_sagaDbContextFactory);
        }

        ITrackingEventWriter GetTrackingEventWriter()
        {
            Func<DbContext> contextProvider = () => new TrackingEventDbContext(LocalDatabaseSelector.ConnectionString);

            using (var context = contextProvider())
            {
                context.Database.CreateIfNotExists();
            }

            return new EntityFrameworkTrackingEventWriter(contextProvider);
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