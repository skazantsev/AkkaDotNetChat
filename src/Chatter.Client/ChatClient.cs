using System;
using Akka.Actor;
using Chatter.Shared;

namespace Chatter.Client
{
    public class ChatClient
    {
        private readonly Func<string, IActorRef> _chatActorBuilder;

        private IActorRef _chatActor;

        public ChatClient(Func<string, IActorRef> chatActorBuilde)
        {
            _chatActorBuilder = chatActorBuilde;
        }

        public string Login { get; private set; }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(Login); }
        }

        public void SignIn(string login)
        {
            if (string.IsNullOrEmpty(login))
                throw new ArgumentNullException("login");

            Login = login;
            _chatActor = _chatActorBuilder(Login);
            _chatActor.Tell(new ClientMessages.SignIn(Login));
        }

        public void SignOut()
        {
            AssertIsAuthenticated();
            _chatActor.Tell(new ClientMessages.SignOut(Login));
        }

        public void Send(string message)
        {
            AssertIsAuthenticated();
            _chatActor.Tell(new ClientMessages.SendMessage(Login, message));
        }

        public void GetLog()
        {
            AssertIsAuthenticated();
            _chatActor.Tell(new ClientMessages.GetChatLog(Login));
        }

        private void AssertIsAuthenticated()
        {
            if (!IsAuthenticated)
                throw new InvalidOperationException("The user is not authenticated.");
        }
    }
}
