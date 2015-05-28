namespace Chatter.Shared
{
    public class ClientMessages
    {
        public class SignIn
        {
            public SignIn(string login)
            {
                Login = login;}

            public string Login { get; private set; }
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
            public SendMessage(string login, string message)
            {
                Login = login;
                Message = message;
            }

            public string Login { get; private set; }

            public string Message { get; private set; }
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
