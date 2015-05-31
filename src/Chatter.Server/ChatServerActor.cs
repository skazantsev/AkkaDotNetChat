using System.Collections.Generic;
using Akka.Actor;
using Chatter.Shared;

namespace Chatter.Server
{
    public class ChatServerActor : ReceiveActor
    {
        // TODO: Move sessions outside actor
        private readonly Dictionary<string, IActorRef> _userSessions;

        public ChatServerActor()
        {
            _userSessions = new Dictionary<string, IActorRef>();

            Receive<ClientMessages.SignIn>(x =>
            {
                Sender.Tell(new ServerMessages.SignInSuccess());
                _userSessions.Add(x.Login, Sender);
                BroadcastToAll(new ServerMessages.NewUserConnected(x.Login));
            });

            Receive<ClientMessages.SignOut>(x =>
            {
                _userSessions.Remove(x.Login);
                Sender.Tell(new ServerMessages.SignOutSuccess());
            });

            Receive<ClientMessages.SendMessage>(x => BroadcastToAll(new ServerMessages.NewMessage(x.Login, x.Message)));
        }

        // TODO: Do not tell a sender (a sender should print a message immediately)
        private void BroadcastToAll<T>(T message)
        {
            foreach (var session in _userSessions.Values)
            {
                session.Tell(message);
            }
        }
    }
}
