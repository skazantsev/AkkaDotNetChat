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
            var stubServiceActor = ChatActorSystem.ActorOf(Props.Create(() => new StubChatServiceActor()), "chatService");

            Chat = new ChatClient(chatActor);
            Chat.SignIn("Sergey");

            Thread.Sleep(500);
            Chat.Send("Hello!");

            Console.ReadLine();
        }

        public class StubChatServiceActor : ReceiveActor
        {
            public StubChatServiceActor()
            {
                Receive<ClientMessages.SignIn>(x =>
                {
                    Sender.Tell(new ServiceMessages.SignInSuccess());
                    Context.ActorSelection("/user/chatClient").Tell(new ServiceMessages.NewUserConnected(x.Login));
                });

                Receive<ClientMessages.SignOut>(x => Sender.Tell(new ServiceMessages.SignOutSuccess()));

                Receive<ClientMessages.SendMessage>(x =>
                {
                    Context.ActorSelection("/user/chatClient").Tell(new ServiceMessages.NewMessage(x.Login, x.Message));
                });
            }
        }
    }
}
