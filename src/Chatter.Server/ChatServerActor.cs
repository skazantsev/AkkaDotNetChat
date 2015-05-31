using System.Collections.Generic;
using Akka.Actor;
using Chatter.Shared;

namespace Chatter.Server
{
    public class ChatServerActor : ReceiveActor
    {
        private static readonly Dictionary<string, IActorRef> UserSessions;

        static ChatServerActor()
        {
            UserSessions = new Dictionary<string, IActorRef>();
        }

        public ChatServerActor()
        {
            Receive<ClientMessages.SignIn>(x =>
            {
                Sender.Tell(new ServerMessages.SignInSuccess());
                UserSessions.Add(x.Login, Sender);
                BroadcastToAll(new ServerMessages.NewUserConnected(x.Login));
            });

            Receive<ClientMessages.SignOut>(x =>
            {
                UserSessions.Remove(x.Login);
                Sender.Tell(new ServerMessages.SignOutSuccess());
            });

            Receive<ClientMessages.SendMessage>(x => BroadcastToAll(new ServerMessages.NewMessage(x.Login, x.Message)));
        }

        private static void BroadcastToAll<T>(T message)
        {
            foreach (var session in UserSessions.Values)
            {
                session.Tell(message);
            }
        }
    }
}
