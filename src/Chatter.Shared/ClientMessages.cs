namespace Chatter.Shared
{
    public class ClientMessages
    {
        public class SignIn
        {
            public SignIn(string login, int color)
            {
                Login = login;
                Color = color;
            }

            public string Login { get; private set; }

            public int Color { get; private set; }
        }

        public class SignOut
        {
            public SignOut(string login)
            {
                Login = login;
            }

            public string Login { get; private set; }
        }

        public class SendMessage
        {
            public SendMessage(string login, string message, int color)
            {
                Login = login;
                Color = color;
                Message = message;
            }

            public string Login { get; private set; }

            public string Message { get; private set; }

            public int Color { get; private set; }
        }

        public class GetChatLog
        {
            public GetChatLog(string login)
            {
                Login = login;
            }

            public string Login { get; private set; }
        }
    }
}
