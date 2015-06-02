using System.Collections.Generic;

namespace Chatter.Shared
{
    public class ServerMessages
    {
        public class SignInSuccess
        {
            public SignInSuccess(string user)
            {
                User = user;
            }

            public string User { get; private set; }
        }

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
            public NewMessage(ChatMessage message)
            {
                Message = message;
            }

            public ChatMessage Message { get; private set; }
        }

        public class MessageLog
        {
            public MessageLog(List<ChatMessage> log)
            {
                Log = log;
            }

            public List<ChatMessage> Log { get; private set; }
        }
    }
}
