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
            Receive<ServiceMessages.SignInSuccess>(x =>
            {
                BecomeAuthenticated();
                UserNotifier.SignInSuccess();
            });

            Receive<ServiceMessages.SignInFailure>(x =>
            {
                BecomeUnauthenticated();
                UserNotifier.SignInFailure();
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

            Receive<ServiceMessages.NewUserConnected>(x => UserNotifier.NewUserConnected(x.User));

            Receive<ServiceMessages.NewMessage>(x => UserNotifier.NewMessage(x.User, x.Message));
        }

        public void Unauthenticating()
        {
            Receive<ServiceMessages.SignOutSuccess>(x =>
            {
                BecomeUnauthenticated();
                UserNotifier.SignOutSuccess();
            });

            Receive<ServiceMessages.SignOutFailure>(x =>
            {
                BecomeAuthenticated();
                UserNotifier.SignOutFailure();
            });
        }

        private static void DispatchToChatService<T>(T obj)
        {
            Context.ActorSelection("/user/chatService").Tell(obj);
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
