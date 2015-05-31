using System.Collections.Generic;
using Akka.Actor;
using Chatter.Shared;
using System;
using System.Threading;

namespace Chatter.Client
{
    public class Program
    {
        public static ActorSystem ActorSystem;

        public static void Main(string[] args)
        {
            ActorSystem = ActorSystem.Create("ChatClientSystem");
            ActorSystem.ActorOf(Props.Create(() => new StubChatServerActor()), "chatServer");

            var chatClient1 = new ChatClient(CreateActor);
            chatClient1.SignIn("Sergey");

            var chatClient2 = new ChatClient(CreateActor);
            chatClient2.SignIn("Mike");

            Thread.Sleep(500);
            chatClient1.Send("Hello!");

            chatClient2.Send("Hi!");

            Console.ReadLine();
        }

        private static IActorRef CreateActor(string login)
        {
            var actorName = string.Format("chatClient_{0}", login);
            return ActorSystem.ActorOf<ChatClientActor>(actorName);
        }

        public class StubChatServerActor : ReceiveActor
        {
            // TODO: Move sessions outside actor
            private readonly Dictionary<string, IActorRef> _userSessions;

            public StubChatServerActor()
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

            // TODO: Do not tell sender (sender should print a message immediately)
            private void BroadcastToAll<T>(T message)
            {
                foreach (var session in _userSessions.Values)
                {
                    session.Tell(message);
                }
            }
        }
    }
}
