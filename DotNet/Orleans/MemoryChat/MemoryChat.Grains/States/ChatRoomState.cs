using System.Collections.Generic;

namespace MemoryChat.Grains.States
{
    public class ChatRoomState
    {
        public string Name;
        public List<string> Users;
        public List<ChatMessage> ChatMessages;
    }
}
