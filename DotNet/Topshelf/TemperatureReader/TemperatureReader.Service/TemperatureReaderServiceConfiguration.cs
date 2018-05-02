using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemperatureReader.Service
{
    public class TemperatureReaderServiceConfiguration
    {
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public static TemperatureReaderServiceConfiguration Default => new TemperatureReaderServiceConfiguration()
        {
            IpAddress = "127.0.0.1",
            Port = 19001
        };
    }
}
