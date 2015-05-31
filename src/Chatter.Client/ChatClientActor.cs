using System;
using Akka.Actor;
using Chatter.Shared;

namespace Chatter.Client
{
    public class ChatClientActor : ReceiveActor
    {
        public ChatClientActor()
        {
            Unauthenticated();
        }

        public void Unauthenticated()
        {
            Receive<ClientMessages.SignIn>(x =>
            {
                DispatchToChatService(x);
                BecomeAuthenticating();
            });
        }

        public void Authenticating()
        {
            Receive<ServerMessages.SignInSuccess>(x =>
            {
                BecomeAuthenticated();
                Console.WriteLine("[SYS_MSG] Successfull sign in");
            });

            Receive<ServerMessages.SignInFailure>(x =>
            {
                BecomeUnauthenticated();
                Console.WriteLine("[SYS_MSG] Oops, try it later.");
            });
        }

        public void Authenticated()
        {
            Receive<ClientMessages.SignOut>(x =>
            {
                DispatchToChatService(x);
                BecomeUnauthenticating();
            });

            Receive<ClientMessages.SendMessage>(x => DispatchToChatService(x));

            Receive<ServerMessages.NewUserConnected>(x => Console.WriteLine("{0} joined the chat", x.User));

            Receive<ServerMessages.NewMessage>(x => Console.WriteLine("{0}: {1}", x.User, x.Message));
        }

        public void Unauthenticating()
        {
            Receive<ServerMessages.SignOutSuccess>(x =>
            {
                BecomeUnauthenticated();
                Console.Write("[SYS_MSG] Enter your name:");
            });

            Receive<ServerMessages.SignOutFailure>(x =>
            {
                BecomeAuthenticated();
                Console.WriteLine("[SYS_MSG] Oops, try it later.");
            });
        }

        private static void DispatchToChatService<T>(T obj)
        {
            Context.ActorSelection("akka.tcp://ChatServerSystem@localhost:9500/user/chatServer").Tell(obj);
        }

        private void BecomeUnauthenticated()
        {
            Become(Unauthenticated);
        }

        private void BecomeAuthenticating()
        {
            Become(Authenticating);
        }

        private void BecomeAuthenticated()
        {
            Become(Authenticated);
        }

        private void BecomeUnauthenticating()
        {
            Become(Unauthenticating);
        }
    }
}
