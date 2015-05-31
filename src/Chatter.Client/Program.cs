using Akka.Actor;
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

            Thread.Sleep(2000);

            var chatClient1 = new ChatClient(CreateActor);
            chatClient1.SignIn("Sergey");

            var chatClient2 = new ChatClient(CreateActor);
            chatClient2.SignIn("Mike");

            Thread.Sleep(2000);
            chatClient1.Send("Hello!");

            chatClient2.Send("Hi!");

            Console.ReadLine();
        }

        private static IActorRef CreateActor(string login)
        {
            var actorName = string.Format("chatClient_{0}", login);
            return ActorSystem.ActorOf<ChatClientActor>(actorName);
        }
    }
}
