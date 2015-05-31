using Akka.Actor;
using System;

namespace Chatter.Client
{
    public class Program
    {
        public static ActorSystem ActorSystem;

        public static void Main(string[] args)
        {
            ActorSystem = ActorSystem.Create("ChatClientSystem");
            var chatClientActor = ActorSystem.ActorOf<ChatClientActor>(string.Format("chatClient_{0}", Guid.NewGuid()));
            var chatClient = new ChatClient(chatClientActor);

            Console.Write("[SYS_MSG] Enter your name:");
            string userName;
            while (true)
            {
                userName = Console.ReadLine();
                if (string.IsNullOrEmpty(userName))
                    Console.WriteLine("[SYS_MSG] User name can't be empty.");
                else
                    break;
            }

            chatClient.SignIn(userName);

            while (true)
            {
                chatClient.Send(Console.ReadLine());
            }
        }
    }
}
