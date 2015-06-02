namespace Chatter.Shared
{
    public class ChatMessage
    {
        public ChatMessage(string from, string text)
        {
            From = from;
            Text = text;
        }

        public string From { get; private set; }

        public string Text { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", From, Text);
        }
    }
}
