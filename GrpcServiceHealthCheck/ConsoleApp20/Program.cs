// Online C# Editor for free
// Write, Edit and Run your C# code using C# Online Compiler

using Grpc.Health.V1;
using Grpc.Net.Client;
using System;

public class HelloWorld
{
    public static async Task Main(string[] args)
    {
        var channel = GrpcChannel.ForAddress("http://grpcserviceinteractingbetweenusers:8080");
        var client = new Health.HealthClient(channel);

        var status = HealthCheckResponse.Types.ServingStatus.NotServing;

        while (status != HealthCheckResponse.Types.ServingStatus.Serving)
        {
            Thread.Sleep(5000);
            try
            {
                var response = await client.CheckAsync(new HealthCheckRequest());
                status = response.Status;
                Console.WriteLine(status);
                Console.WriteLine("YYYYYYYYYYYYYYYYYYYYY");
            }
            catch (Exception ex)
            {
                Console.WriteLine("XXXXXXXXXXXX");
                Console.WriteLine(status);
                continue;
            }
        }
        Console.WriteLine(status);
    }
}