using MemoryChat.Utils;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;

namespace MemoryChat.Client
{
    public class ClientWrapper
    {
        #region Members

        private ClientConfiguration _clientConfiguration;
        private IClusterClient _clusterClient;

        #endregion

        #region Accessors 

        public ClientConfiguration Configuration => _clientConfiguration ?? throw new OrleansException("ClientConfiguration has not been setup yet");

        /// <summary>
        /// Access client once connected
        /// </summary>
        public IClusterClient Client => _clusterClient?.IsInitialized == true ? _clusterClient : throw new OrleansException("Cluster client has not been successfully connected yet");

        #endregion

        #region Constructors 

        /// <summary>
        /// Default ctor
        /// </summary>
        public ClientWrapper() { }

        /// <summary>
        ///  Constructor accepting client configuration 
        /// </summary>
        /// <param name="clientConfiguration"></param>
        public ClientWrapper(ClientConfiguration clientConfiguration)
        {
            if (clientConfiguration == null)
            {
                throw new Exception("ClientConfiguration cannot be null");
            }

            _clientConfiguration = clientConfiguration;
        }

        #endregion

        /// <summary>
        /// Initialise client configuration using configuration supplied or using default configuration
        /// </summary>
        /// <param name="clientConfiguration"></param>
        /// <returns></returns>
        public int Initialise(ClientConfiguration clientConfiguration = null)
        {
            if (clientConfiguration == null)
            {
                _clientConfiguration = ClientConfiguration.LocalhostSilo();
                _clientConfiguration.AddSimpleMessageStreamProvider(MemoryChatConfiguration.MemoryChatStreamProvider);
            }
            else
            {
                _clientConfiguration = clientConfiguration;
            }

            return 0;
        }

        /// <summary>
        /// Connect client to cluster
        /// </summary>
        /// <returns></returns>
        public int Connect()
        {
            if (_clusterClient != null)
            {
                throw new OrleansException("Cluster client is already connected");
            }

            if (_clientConfiguration == null)
            {
                throw new OrleansException("ClientConfiguration has not been setup yet");
            }

            try
            {
                _clusterClient = new ClientBuilder()
                    .UseConfiguration(_clientConfiguration)
                    .Build();

                Console.WriteLine("Cluster client attempting to connect...");

                _clusterClient
                    .Connect()
                    .Wait();

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return 1;
            }

            if (!_clusterClient.IsInitialized)
            {

                Console.WriteLine("Cluster client failed to connect");
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Disconnect and dispose connected client
        /// </summary>
        /// <returns></returns>
        public int Disconnect()
        {
            if (_clusterClient == null)
            {
                throw new OrleansException("There is no cluster client to disconnect");
            }

            try
            {
                _clusterClient.Close();
                _clusterClient.Dispose();

                Console.WriteLine("Cluster client successfully disconnected");
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
