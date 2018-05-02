using MemoryChat.Grains.States;
using Orleans;
using System.Threading.Tasks;

namespace MemoryChat.Grains
{
    public interface IChatMessageGrain : IGrainWithGuidKey
    {
        Task SendMessage(string chatRoom, ChatMessage chatMessage);
    }
}
