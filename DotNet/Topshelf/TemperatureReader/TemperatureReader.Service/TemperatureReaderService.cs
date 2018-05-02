using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TemperatureReader.Service
{
    public class TemperatureReaderService
    {
        private readonly TemperatureReaderServiceConfiguration _temperatureReaderConfiguration;
        private readonly byte[] _buffer;

        private TcpListener _listener;

        public TemperatureReaderService(TemperatureReaderServiceConfiguration temperatureReaderConfiguration)
        {
            if (temperatureReaderConfiguration == null)
            {
                throw new Exception("TemperatureReaderConfiguration cannot be null");
            }

            _temperatureReaderConfiguration = temperatureReaderConfiguration;
            _buffer = new byte[256];
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Parse(_temperatureReaderConfiguration.IpAddress), _temperatureReaderConfiguration.Port);

            _listener.Start();

            Console.WriteLine($"Server has started on {_temperatureReaderConfiguration.IpAddress}:{_temperatureReaderConfiguration.Port}");

            while (true)
            {
                Console.WriteLine("Waiting for a connection to be established...");

                var tcpClient = _listener.AcceptTcpClient();

                Console.WriteLine("Connection successful");

                var incomingStream = tcpClient.GetStream();

                int packetSize;
                if ((packetSize = incomingStream.Read(_buffer, 0, _buffer.Length)) != 0)
                {
                    var data = Encoding.ASCII.GetString(_buffer, 0, packetSize);

                    Console.WriteLine($"Received data: {data}");

                    var response = Encoding.ASCII.GetBytes("200");

                    incomingStream.Write(response, 0, response.Length);

                    Console.WriteLine($"Sent data: {data}");
                }

                tcpClient.Close();
            }
        }

        public void Stop()
        {
            _listener.Stop();

            Console.WriteLine($"Server has stopped on {_temperatureReaderConfiguration.IpAddress}:{_temperatureReaderConfiguration.Port} ");
        }
    }
}
