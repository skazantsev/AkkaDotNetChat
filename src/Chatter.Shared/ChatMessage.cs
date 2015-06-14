namespace Chatter.Shared
{
    public class ChatMessage
    {
        public ChatMessage(string from, string text, int color)
        {
            From = from;
            Text = text;
            Color = color;
        }

        public string From { get; private set; }

        public string Text { get; private set; }

        public int Color { get; private set; }
    }
}
