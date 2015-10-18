using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BookingWeb
{
    using System.Configuration;
    using MassTransit;
    using MassTransit.Util;


    public class WebApiApplication : System.Web.HttpApplication
    {
        static IBusControl _bus;
        static BusHandle _busHandle;

        public static IBus Bus
        {
            get { return _bus; }
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            _bus = MassTransit.Bus.Factory.CreateUsingRabbitMq(x =>
            {
                x.Host(GetHostAddress(), h =>
                {
                    h.Username(ConfigurationManager.AppSettings["RabbitMQUsername"]);
                    h.Password(ConfigurationManager.AppSettings["RabbitMQPassword"]);
                });
            });

            _busHandle = _bus.Start();

            TaskUtil.Await(() => _busHandle.Ready);
        }

        protected void Application_End()
        {
            _busHandle?.Stop(TimeSpan.FromSeconds(30));
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
