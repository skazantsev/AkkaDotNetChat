﻿using System;
using System.Configuration;
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
                DispatchToChatService(new ClientMessages.GetChatLog(x.User));
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

            Receive<ServerMessages.NewUserConnected>(x =>
            {
                Console.ForegroundColor = (ConsoleColor) x.Color;
                Console.Write(x.User);
                Console.ResetColor();
                Console.WriteLine(" joined the chat");
            });

            Receive<ServerMessages.NewMessage>(x =>
            {
                Console.ForegroundColor = (ConsoleColor)x.Message.Color;
                Console.Write(x.Message.From);
                Console.ResetColor();
                Console.WriteLine(" " + x.Message.Text);
            });

            Receive<ServerMessages.MessageLog>(x =>
            {
                foreach (var message in x.Log)
                {
                    Console.WriteLine(message);
                }
            });
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
            Context.ActorSelection(ConfigurationManager.AppSettings["ChatServerActorPath"]).Tell(obj);
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
