using System;

namespace Chatter.Client
{
    public static class UserNotifier
    {
        public static void RequestAuthentication()
        {
            Console.Write(">>>Enter your name:");
        }

        public static void SignInSuccess()
        {
            Console.WriteLine(">>>Successfull sign in");
        }

        public static void SignInFailure()
        {
            Console.WriteLine(">>>Oops, try it later.");
        }

        public static void SignOutSuccess()
        {
            RequestAuthentication();
        }

        public static void SignOutFailure()
        {
            Console.WriteLine(">>>Oops, try it later.");
        }

        public static void NewMessage(string user, string message)
        {
            Console.WriteLine("{0}: {1}", user, message);
        }

        public static void NewUserConnected(string user)
        {
            Console.WriteLine("{0} joined the chat", user);
        }
    }
}
