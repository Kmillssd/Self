using MemoryChat.Grains;
using MemoryChat.Grains.Observers;
using MemoryChat.Grains.States;
using MemoryChat.Utils;
using System;
using System.Threading.Tasks;

namespace MemoryChat.Client
{
    class Program
    {
        private const string EXIT_CODE = "$q";

        static int Main(string[] args)
        {
            var exitCode = 0;
            
            var clientWrapper = new ClientWrapper();

            exitCode += clientWrapper.Initialise();
            exitCode += clientWrapper.Connect();

            if (exitCode < 1)
            {
                Runner(clientWrapper).GetAwaiter().GetResult();
            }

            exitCode += clientWrapper.Disconnect();

            return exitCode;
        }

        private static async Task Runner(ClientWrapper clientWrapper)
        {
            var userResponse = "";

            Console.WriteLine("Whats your username?");
            var userName = Console.ReadLine();

            Console.WriteLine("What room do you want to join?");
            var chatRoom = Console.ReadLine();

            var streamId = await clientWrapper
                .Client
                .GetGrain<IChatRoomGrain>(chatRoom)
                .Join(userName);

            // Subscribe to observer to be notified of event
            var stream = clientWrapper
                .Client
                .GetStreamProvider(MemoryChatConfiguration.MemoryChatStreamProvider)
                .GetStream<ChatMessage>(streamId, MemoryChatConfiguration.MemoryChatStreamNamespace);

            await stream.SubscribeAsync(new MemoryChatStreamObserver());

            do
            {
                userResponse = Console.ReadLine();

               await clientWrapper
                    .Client
                    .GetGrain<IChatMessageGrain>(Guid.NewGuid())
                    .SendMessage(chatRoom, new ChatMessage()
                    {
                        Message = userResponse,
                        User = userName
                    });
            }
            while (!String.Equals(userResponse, EXIT_CODE));

            // Un-subscribe to all observers
            foreach (var handler in await stream.GetAllSubscriptionHandles())
            {
                await handler.UnsubscribeAsync();
            }
        }
    }
}
