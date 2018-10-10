using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WctTcp.ServerNet472
{
    public class ServiceManager : IDisposable
    {
        protected readonly string _protocol;
        protected readonly string _host;
        protected readonly int _port;
        protected readonly SecurityMode _bindingSecurity;

        protected HashSet<ServiceHost> _serviceHosts;

        public ServiceManager()
        {
            _protocol = "net.tcp";
            _host = "localhost";
            _port = 8030;
            _bindingSecurity = SecurityMode.None;
            _serviceHosts = new HashSet<ServiceHost>();
        }

        private string CreateServiceUrl(string serviceEndpoint)
        {
            return _protocol + "://" + _host + ":" + _port + "/" + serviceEndpoint;
        }

        public void RegisterService<IType, IImplementation>(string serviceEndpoint)
            where IType : class
            where IImplementation : class, IType
        {
            if (String.IsNullOrWhiteSpace(serviceEndpoint)) throw new ArgumentException(nameof(serviceEndpoint));

            var address = CreateServiceUrl(serviceEndpoint);

            // If using a DI/IoC, then resolve this type with the container else use this method
            var serviceImple = Activator.CreateInstance(typeof(IImplementation));

            var host = new ServiceHost(serviceImple, new[] { new Uri(address) });

            host.AddServiceEndpoint(typeof(IType), new NetTcpBinding(_bindingSecurity), "");

            host.Opened += (object sender, EventArgs ea) =>
            {
                System.Diagnostics.Debug.WriteLine($"Starting service {serviceImple.GetType().FullName} listening on endpoint {serviceEndpoint}");
            };

            _serviceHosts.Add(host);
        }

        public void StartServices()
        {
            foreach (var host in _serviceHosts)
            {
                host.Open();
            }
        }

        void IDisposable.Dispose()
        { 
            foreach (var host in _serviceHosts)
            {
                host.Close();
            }
        }
    }
}

