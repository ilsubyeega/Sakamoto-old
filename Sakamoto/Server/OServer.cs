using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sakamoto.Server
{
    class OServer
    {
        public static OHttpServer server;
        private static int port = 40002;
        private static bool Initalized = false;
        public static void Init()
        {
            try
            {
                server = new OHttpServer(IPAddress.Loopback, port);
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
    class OHttpSession : HttpSession
    {
        public OHttpSession(NetCoreServer.HttpServer server) : base(server) { }
        private Events.OOnReceivedRequest OOnReceivedRequest = new Events.OOnReceivedRequest(OServer.server);
        protected override void OnReceivedRequest(HttpRequest request)
        {
            OOnReceivedRequest.Handle(request, HttpResponse response);
            SendResponseAsync(response);
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

    class OHttpServer : NetCoreServer.HttpServer
    {
        public OHttpServer(IPAddress address, int port) : base(address, port) { }

        protected override TcpSession CreateSession() { return new OHttpSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"HTTP session caught an error: {error}");
        }
    }
}
