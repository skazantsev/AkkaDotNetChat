using Akka.Actor;
using Chatter.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Chatter.Server
{
    public class ChatServerActor : ReceiveActor
    {
        private static readonly Dictionary<string, IActorRef> UserSessions = new Dictionary<string, IActorRef>();

        private static LinkedList<ChatMessage> _log = new LinkedList<ChatMessage>();

        public ChatServerActor()
        {
            Receive<ClientMessages.SignIn>(x =>
            {
                UserSessions.Add(x.Login, Sender);
                Sender.Tell(new ServerMessages.SignInSuccess(x.Login));
                BroadcastToAll(new ServerMessages.NewUserConnected(x.Login));
            });

            Receive<ClientMessages.SignOut>(x =>
            {
                UserSessions.Remove(x.Login);
                Sender.Tell(new ServerMessages.SignOutSuccess());
            });

            Receive<ClientMessages.SendMessage>(x =>
            {
                var chatMsg = new ChatMessage(x.Login, x.Message);
                _log.AddFirst(chatMsg);
                if (_log.Count > 100)
                    _log = new LinkedList<ChatMessage>(_log.Take(10));
                BroadcastToAll(new ServerMessages.NewMessage(chatMsg));
            });

            Receive<ClientMessages.GetChatLog>(x =>
            {
                var lastMessages = _log.Take(10).Reverse().ToList();
                Sender.Tell(new ServerMessages.MessageLog(lastMessages));
            });
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
