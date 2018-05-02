using MemoryChat.Grains.States;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace MemoryChat.Grains.Observers
{
    public class MemoryChatStreamObserver : IAsyncObserver<ChatMessage>
    {
        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }

        public Task OnNextAsync(ChatMessage item, StreamSequenceToken token = null)
        {
            Console.WriteLine($"{item.User}: {item.Message}");
            return Task.CompletedTask;
        }
    }
}
