using NetCoreServer;
using System;
using System.Net;
using System.Net.Sockets;

namespace Sakamoto.Server
{
    class CServer
    {
        public static CHttpServer server;
        private static int port = 40001;
        private static bool Initalized = false;
        public static void Init()
        {
            try
            {
                server = new CHttpServer(IPAddress.Loopback, port);
                server.Start();
            }
            catch
            {
                Console.WriteLine("Error while starting SHttpServer!");
            }
            finally
            {
                Initalized = true;
            }
        }

    }
    class CHttpSession : HttpSession
    {
        public CHttpSession(NetCoreServer.HttpServer server) : base(server) { }
        private Events.COnReceivedRequest COnReceivedRequest = new Events.COnReceivedRequest(CServer.server);
        protected override void OnReceivedRequest(HttpRequest request)
        {
            
            SendResponseAsync(COnReceivedRequest.Handle(request));
        }
        protected override void OnReceivedRequestError(HttpRequest request, string error)
        {
            Console.WriteLine($"Request error: {error}");
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"HTTP session caught an error: {error}");
        }
    }

    class CHttpServer : NetCoreServer.HttpServer
    {
        public CHttpServer(IPAddress address, int port) : base(address, port) { }

        protected override TcpSession CreateSession() { return new CHttpSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"HTTP session caught an error: {error}");
        }
    }
}
