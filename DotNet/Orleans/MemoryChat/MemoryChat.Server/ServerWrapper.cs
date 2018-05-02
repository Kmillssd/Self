using MemoryChat.Utils;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using System;

namespace MemoryChat.Server
{
    public class ServerWrapper
    {
        #region Members

        private SiloHost _siloHost;
        private ClusterConfiguration _clusterConfiguration;

        #endregion

        #region Accessors 

        public ClusterConfiguration Configuration => _clusterConfiguration ?? throw new OrleansException("ClusterConfiguration has not been setup yet");

        #endregion

        #region Constructors

        /// <summary>
        /// Default ctor
        /// </summary>
        public ServerWrapper() { }

        /// <summary>
        /// Constructor accepting cluster configuration
        /// </summary>
        /// <param name="clusterConfiguration"></param>
        public ServerWrapper(ClusterConfiguration clusterConfiguration)
        {
            if (clusterConfiguration == null)
            {
                throw new Exception("ClusterConfiguration cannot be null");
            }

            _clusterConfiguration = clusterConfiguration;
        }

        #endregion

        /// <summary>
        /// Initialise cluster configuration using configuration supplied or using default configuration
        /// </summary>
        /// <param name="clusterConfiguration">Used if not null</param>
        /// <returns></returns>
        public int Initialise(ClusterConfiguration clusterConfiguration = null)
        {
            if (clusterConfiguration == null)
            {
                _clusterConfiguration = ClusterConfiguration.LocalhostPrimarySilo();
                _clusterConfiguration.AddMemoryStorageProvider();
                _clusterConfiguration.AddMemoryStorageProvider("PubSubStore");
                _clusterConfiguration.AddSimpleMessageStreamProvider(MemoryChatConfiguration.MemoryChatStreamProvider);
                //_clusterConfiguration.AddAzureTableStorageProvider("AzureStore", "UseDevelopmentStorage=true");
            }
            else
            {
                _clusterConfiguration = clusterConfiguration;
            }

            return 0;
        }

        /// <summary>
        /// Initialise and starts SiloHost reporting any issues
        /// </summary>
        /// <returns></returns>
        public int Start()
        {
            if (_siloHost != null)
            {
                throw new Exception("Silo server is already running");
            }

            if (_clusterConfiguration == null)
            {
                throw new Exception("ClusterConfiguration has not been setup yet");
            }

            _siloHost = new SiloHost($"MemoryChatSilo-{Guid.NewGuid()}", _clusterConfiguration);

            try
            {
                _siloHost.InitializeOrleansSilo();

                if (_siloHost.StartOrleansSilo())
                {
                    Console.WriteLine($"Successfully started silo server {_siloHost.Name}");
                    return 0;
                }
                else
                {
                    throw new OrleansException($"Failed to start silo server {_siloHost.Name}");
                }
            }
            catch (Exception ex)
            {
                _siloHost.ReportStartupError(ex);
                Console.Error.WriteLine(ex);
                return 1;
            }
        }

        /// <summary>
        /// Stop and dispose of running SiloHost
        /// </summary>
        /// <returns></returns>
        public int Stop()
        {
            if (_siloHost == null)
            {
                throw new OrleansException("There is no silo server running to stop");
            }

            try
            {
                _siloHost.ShutdownOrleansSilo();
                _siloHost.Dispose();

                Console.WriteLine($"Successfully stopped silo server {_siloHost.Name}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }

            return 0;
        }
    }
}
