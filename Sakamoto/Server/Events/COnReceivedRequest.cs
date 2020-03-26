using NetCoreServer;
using System;

namespace Sakamoto.Server.Events
{
    class COnReceivedRequest : HttpSession
    {

        public COnReceivedRequest(NetCoreServer.HttpServer server) : base(server) { }
        public void Handle(HttpRequest request, out HttpResponse response)
        {
            Console.WriteLine(request);
            response = Response
                .SetBegin(420) // Make osu client shows server is busy...
                .MakeGetResponse("pog");
        }
    }
}
