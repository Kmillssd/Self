using MemoryChat.Grains.States;
using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace MemoryChat.Grains
{
    /// <summary>
    /// Reentrant specifies this grain to be able to process additional incoming requests awhile currently processing another
    /// </summary>
    [Reentrant]
    public class ChatMessageGrain : Grain, IChatMessageGrain
    {
        public async Task SendMessage(string chatRoom, ChatMessage chatMessage)
        {
            var roomGrain = this.GrainFactory.GetGrain<IChatRoomGrain>(chatRoom);

            await roomGrain.AcceptMessage(chatMessage);
        }
    }
}
