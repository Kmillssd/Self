using System;
using System.ServiceModel;
using WcfTcp.Contracts;

namespace WcfTcp.Client.Services
{
    public abstract class ClientService<TType>
           where TType : class, IService
    {
        protected readonly string _protocol;
        protected readonly string _host;
        protected readonly int _port;
        protected readonly SecurityMode _bindingSecurity;
        protected readonly string _serviceEndpoint;

        protected TType _proxy;
        public TType Proxy
        {
            get
            {
                return _proxy;
            }
        }

        public ClientService(string serviceEndpoint)
        {
            if (String.IsNullOrWhiteSpace(serviceEndpoint)) throw new ArgumentException(nameof(serviceEndpoint));

            _protocol = "net.tcp";
            _host = "localhost";
            _port = 8030;
            _bindingSecurity = SecurityMode.None;
            _serviceEndpoint = serviceEndpoint;

            var endpointAddress = new EndpointAddress(CreateServiceUrl());
            var binding = new NetTcpBinding(_bindingSecurity);
            var factory = new ChannelFactory<TType>(binding, endpointAddress);

            _proxy = factory.CreateChannel();
        }

        private string CreateServiceUrl()
        {
            return _protocol + "://" + _host + ":" + _port + "/" + _serviceEndpoint;
        }
    }
}
