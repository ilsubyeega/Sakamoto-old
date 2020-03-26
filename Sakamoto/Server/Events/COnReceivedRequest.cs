using NetCoreServer;
using osu.Shared.Serialization;
using Sakamoto.Packet.Parser;
using Sakamoto.Packet;
using System;
using System.IO;
using Sakamoto.Objects;
using System.Collections.Generic;
using System.Text;

namespace Sakamoto.Server.Events
{
    class COnReceivedRequest : HttpSession
    {

        public COnReceivedRequest(NetCoreServer.HttpServer server) : base(server) { }
        public HttpResponse Handle(HttpRequest request)
        {
            if (request.Method == "GET")
                return Response.SetBegin(200).MakeGetResponse("Sakamoto<br>osu! Bancho reversing project");
            else if (request.Method == "POST") { 
                Console.WriteLine("\n");
                for (int a = 0; a < request.Headers; a++)
                {
                    Tuple<String, String> t = request.Header(a);
                    Console.WriteLine(t.Item1 + ": " + t.Item2);
                }
                byte[] bytes = Encoding.UTF8.GetBytes(request.Body);
                request.Cache;
                Console.WriteLine(BitConverter.ToString(bytes).Replace("-", " "));
                //List<RawPacket> list_packet = RawByteParser.Parse(request.Cache.Data);


                request.Header((int)request.Headers);
                return Response
                    .SetBegin(420) // Make osu client shows server is busy...
                    .SetBody("poggers");

            } else
            {
                return Response.MakeErrorResponse("wrong method", 404);
            }
        }
    }
}
