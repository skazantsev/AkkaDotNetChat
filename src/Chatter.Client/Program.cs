using Akka.Actor;
using Chatter.Shared;
using System;
using System.Threading;

namespace Chatter.Client
{
    public class Program
    {
        public static ActorSystem ChatActorSystem;
        public static ChatClient Chat;

        public static void Main(string[] args)
        {
            ChatActorSystem = ActorSystem.Create("ChatActorSystem");
            var chatActor = ChatActorSystem.ActorOf(Props.Create(() => new ChatClientActor()), "chatClient");
            var chatServerActor = ChatActorSystem.ActorOf(Props.Create(() => new StubChatServerActor()), "chatServer");

            Chat = new ChatClient(chatActor);
            Chat.SignIn("Sergey");

            Thread.Sleep(500);
            Chat.Send("Hello!");

            Console.ReadLine();
        }

        public class StubChatServerActor : ReceiveActor
        {
            public StubChatServerActor()
            {
                Receive<ClientMessages.SignIn>(x =>
                {
                    Sender.Tell(new ServerMessages.SignInSuccess());
                    Context.ActorSelection("/user/chatClient").Tell(new ServerMessages.NewUserConnected(x.Login));
                });

                Receive<ClientMessages.SignOut>(x => Sender.Tell(new ServerMessages.SignOutSuccess()));

                Receive<ClientMessages.SendMessage>(x =>
                {
                    Context.ActorSelection("/user/chatClient").Tell(new ServerMessages.NewMessage(x.Login, x.Message));
                });
            }
        }
    }
}
