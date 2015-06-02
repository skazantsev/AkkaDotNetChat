using Akka.Actor;
using Chatter.Shared;

namespace Chatter.Client
{
    public class ChatClient
    {
        private readonly IActorRef _chatActor;

        public ChatClient(IActorRef chatActor)
        {
            _chatActor = chatActor;
        }

        public string Login { get; private set; }

        public void SignIn(string login)
        {
            Login = login;
            _chatActor.Tell(new ClientMessages.SignIn(Login));
        }

        public void SignOut()
        {
            _chatActor.Tell(new ClientMessages.SignOut(Login));
        }

        public void Send(string message)
        {
            _chatActor.Tell(new ClientMessages.SendMessage(Login, message));
        }
    }
}
