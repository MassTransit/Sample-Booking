namespace Booking.Activities.Tests
{
    using System.IO;
    using System.Text;
    using log4net.Config;
    using MassTransit.Log4NetIntegration.Logging;
    using MassTransit.Logging;
    using NUnit.Framework;


    [SetUpFixture]
    public class ContextSetup
    {
        [SetUp]
        public void Setup()
        {
            ConfigureLogger();

            Logger.UseLogger(new Log4NetLogger());
        }

        static void ConfigureLogger()
        {
            const string logConfig = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<log4net>
  <root>
    <level value=""INFO"" />
    <appender-ref ref=""console"" />
  </root>
  <logger name=""NHibernate"">
    <level value=""ERROR"" />
  </logger>
  <appender name=""console"" type=""log4net.Appender.ConsoleAppender"">
    <layout type=""log4net.Layout.PatternLayout"">
      <conversionPattern value=""%m%n"" />
    </layout>
  </appender>
</log4net>";

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(logConfig)))
            {
                XmlConfigurator.Configure(stream);
            }
        }
    }
}