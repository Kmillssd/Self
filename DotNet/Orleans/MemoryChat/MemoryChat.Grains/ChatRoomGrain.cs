using MemoryChat.Grains.States;
using MemoryChat.Utils;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryChat.Grains
{
    /// <summary>
    /// specifies stateful grain to use specific provider
    /// </summary>
    [StorageProvider(ProviderName = "MemoryStore")]
    public class ChatRoomGrain : Grain<ChatRoomState>, IChatRoomGrain
    {
        private IAsyncStream<ChatMessage> _asyncStream;

        public override Task OnActivateAsync()
        {
            this.State.Users = new List<string>();
            this.State.ChatMessages = new List<ChatMessage>();

            _asyncStream = GetStreamProvider(MemoryChatConfiguration.MemoryChatStreamProvider).GetStream<ChatMessage>(Guid.NewGuid(), MemoryChatConfiguration.MemoryChatStreamNamespace);

            return base.OnActivateAsync();
        }

        public Task<Guid> Join(string user)
        {
            this.State.Users.Add(user);

            return Task.FromResult(_asyncStream.Guid);
        }

        public Task AcceptMessage(ChatMessage chatMessage)
        {
            this.State.ChatMessages.Add(chatMessage);

            // Notify all subscribers to stream about new chat message
            _asyncStream.OnNextAsync(chatMessage);

            return Task.CompletedTask;
        }
    }
}
