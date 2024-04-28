using System.Threading.Tasks;
using Grpc.Net.Client;
using Pos;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://47.120.41.97:2333");
var client = new Pos.PosServicer.PosServicerClient(channel);
var reply = await client.EasyCallAsync(
                  new EasyCallRequest { Code = 1 });
Console.WriteLine("Greeting: " + reply.Code);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();