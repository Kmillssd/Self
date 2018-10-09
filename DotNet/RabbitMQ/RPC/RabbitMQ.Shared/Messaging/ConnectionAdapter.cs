using RabbitMQ.Client;
using System;

namespace RabbitMQ.Shared.Messaging
{
    public class ConnectionAdapter : IDisposable
    {
        protected readonly string _hostName;
        protected readonly string _userName;
        protected readonly string _password;

        protected IConnection _connection;

        public ConnectionAdapter()
        {
            _hostName = "localhost";
            _userName = "guest";
            _password = "guest";
        }

        public bool IsConnected
        {
            get { return _connection != null && _connection.IsOpen; }
        }

        public void Connect()
        {
            _connection = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            }.CreateConnection();
        }

        public void Disconnect()
        {
            if (_connection != null) _connection.Dispose();
        }

        public IConnection GetConnection()
        {
            if (!IsConnected) Connect();
            return _connection;
        }

        void IDisposable.Dispose() => Disconnect();
    }
}
