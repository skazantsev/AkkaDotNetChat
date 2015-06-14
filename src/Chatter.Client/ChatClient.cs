using System;
using Akka.Actor;
using Chatter.Shared;

namespace Chatter.Client
{
    public class ChatClient
    {
        private readonly Random _rand;

        private readonly IActorRef _chatActor;

        public ChatClient(IActorRef chatActor)
        {
            _chatActor = chatActor;
            _rand = new Random();
        }

        public string Login { get; private set; }

        public int Color { get; private set; }

        public void SignIn(string login)
        {
            Login = login;
            Color = _rand.Next(1, 15);
            _chatActor.Tell(new ClientMessages.SignIn(Login, Color));
        }

        public void SignOut()
        {
            _chatActor.Tell(new ClientMessages.SignOut(Login));
        }

        public void Send(string message)
        {
            _chatActor.Tell(new ClientMessages.SendMessage(Login, message, Color));
        }
    }
}
