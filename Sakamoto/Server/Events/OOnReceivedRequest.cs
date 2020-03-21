using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sakamoto.Server.Events
{
	class OOnReceivedRequest : HttpSession
    {

        public OOnReceivedRequest(NetCoreServer.HttpServer server) : base(server) { }
        public void Handle(HttpRequest request, out HttpResponse response)
        {
            //Console.WriteLine(request);


            byte[] ba = Encoding.Default.GetBytes(request.Body);
            string hexString = BitConverter.ToString(ba).Replace("-", " ");
            Console.WriteLine(hexString);

            response = Response
                           .SetBegin(420) // Make osu client shows server is busy...
                           .MakeGetResponse("pog");
        }
    }
}
