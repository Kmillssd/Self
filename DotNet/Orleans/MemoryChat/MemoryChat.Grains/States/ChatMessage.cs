using Orleans.Concurrency;

namespace MemoryChat.Grains.States
{
    /// <summary>
    /// State passed between grains marked as immutable to utilize Orleans optimisations
    /// </summary>
    [Immutable]
    public class ChatMessage
    {
        public string User { get; set; }
        public string Message { get; set; }
    }
}
