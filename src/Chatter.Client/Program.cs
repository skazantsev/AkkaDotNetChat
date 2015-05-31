using Akka.Actor;
using System;

namespace Chatter.Client
{
    public class Program
    {
        public static ActorSystem ActorSystem;

        public static ChatClient ChatClient;

        public static void Main(string[] args)
        {
            ActorSystem = ActorSystem.Create("ChatClientSystem");
            var chatClientActor = ActorSystem.ActorOf<ChatClientActor>(string.Format("chatClient_{0}", Guid.NewGuid()));
            ChatClient = new ChatClient(chatClientActor);

            Console.Write(">>>Enter your name:");
            string userName;
            while (true)
            {
                userName = Console.ReadLine();
                if (string.IsNullOrEmpty(userName))
                    Console.WriteLine("User name can't be empty.");
                else
                    break;
            }

            ChatClient.SignIn(userName);

            while (true)
            {
                try
                {
                    ChatClient.Send(Console.ReadLine());
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine(">>>not authenticated");
                }
            }
        }
    }
}
