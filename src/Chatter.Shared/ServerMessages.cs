namespace Chatter.Shared
{
    public class ServerMessages
    {
        public class SignInSuccess
        { }

        public class SignInFailure
        { }

        public class SignOutSuccess
        { }

        public class SignOutFailure
        { }

        public class NewUserConnected
        {
            public NewUserConnected(string user)
            {
                User = user;
            }

            public string User { get; private set; }
        }

        public class NewMessage
        {
            public NewMessage(string user, string message)
            {
                User = user;
                Message = message;
            }

            public string User { get; private set; }

            public string Message { get; private set; }
        }
    }
}
