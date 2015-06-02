using System.Collections.Generic;
using System.Linq;
using Akka.Actor;
using Chatter.Shared;

namespace Chatter.Server
{
    public class ChatServerActor : ReceiveActor
    {
        private static readonly Dictionary<string, IActorRef> UserSessions = new Dictionary<string, IActorRef>();

        private static readonly Stack<ChatMessage> Log = new Stack<ChatMessage>();

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
                Log.Push(chatMsg);
                BroadcastToAll(new ServerMessages.NewMessage(chatMsg));
            });

            Receive<ClientMessages.GetChatLog>(x =>
            {
                var lastMessages = Log.Take(10).ToList();
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
