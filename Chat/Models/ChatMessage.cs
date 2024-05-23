namespace Chat.Models
{
    public class ChatMessage
    {
        public ChatMessageRole Role { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }
    }
}
