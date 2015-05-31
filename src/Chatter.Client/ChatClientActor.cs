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
                UserNotifier.SignInSuccess();
            });

            Receive<ServerMessages.SignInFailure>(x =>
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

            Receive<ServerMessages.NewUserConnected>(x => UserNotifier.NewUserConnected(x.User));

            Receive<ServerMessages.NewMessage>(x => UserNotifier.NewMessage(x.User, x.Message));
        }

        public void Unauthenticating()
        {
            Receive<ServerMessages.SignOutSuccess>(x =>
            {
                BecomeUnauthenticated();
                UserNotifier.SignOutSuccess();
            });

            Receive<ServerMessages.SignOutFailure>(x =>
            {
                BecomeAuthenticated();
                UserNotifier.SignOutFailure();
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
