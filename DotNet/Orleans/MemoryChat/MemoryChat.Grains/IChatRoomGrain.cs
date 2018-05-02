using MemoryChat.Grains.States;
using Orleans;
using System;
using System.Threading.Tasks;

namespace MemoryChat.Grains
{
    public interface IChatRoomGrain : IGrainWithStringKey
    {
        Task<Guid> Join(string user);
        Task AcceptMessage(ChatMessage chatMessage);
    }
}
