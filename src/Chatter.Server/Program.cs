using System;
using Akka.Actor;

namespace Chatter.Server
{
    public class Program
    {
        public static ActorSystem ActorSystem;

        public static void Main(string[] args)
        {
            ActorSystem = ActorSystem.Create("ChatServerSystem");
            ActorSystem.ActorOf(Props.Create(() => new ChatServerActor()), "chatServer");

            Console.WriteLine("Welcome to Chatter service!\r\nType 'exit' to exit the service.");

            var input = string.Empty;
            while (string.IsNullOrEmpty(input) || !input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                input = Console.ReadLine();
            }
        }
    }
}
