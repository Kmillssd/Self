using System;
using TemperatureReader.Service;
using Topshelf;

namespace TemperatureReader.Host
{
    class Program
    {
        static void Main(string[] args)
        {

#if DEBUG
            var serviceConfiguration = TemperatureReaderServiceConfiguration.Default;
#elif !DEBUG
            var serviceConfiguration = new TemperatureReaderServiceConfiguration()
            {
                IpAddress = args[0],
                Port = Convert.ToInt32(args[1])
            };
#endif

            HostFactory.Run(hostConfig =>
            {
                hostConfig.Service<TemperatureReaderService>(s =>
                {
                    s.ConstructUsing(name => new TemperatureReaderService(serviceConfiguration));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                hostConfig.RunAsLocalSystem();
                hostConfig.SetDescription("The host for the Temperature Reader Service");
                hostConfig.SetDisplayName("Temperature Reader Service Host");
                hostConfig.SetServiceName("TemperatureReaderServiceHost");
            });
        }
    }
}
