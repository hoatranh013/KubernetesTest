using Grpc.Net.Client;
using GrpcServiceInteractingBetweenUsers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

public class TcpServerGetAddFriendsRequests
{
    public static async Task Main(string[] args)
    {
        var ipAddress = IPAddress.Parse("127.0.0.1");
        var ipEndpoint = new IPEndPoint(ipAddress, 3000);
        var tcpServer = new TcpListener(ipEndpoint);
        var grpcChannel = GrpcChannel.ForAddress("localhost");
        var sendFriendRequestService = new SendFriendRequestHandler.SendFriendRequestHandlerClient(grpcChannel);
        tcpServer.Start();
        while (true)
        {
            var tcpClient = await tcpServer.AcceptTcpClientAsync();
            Task.Run(async () =>
            {
                var getContent = new char[65425];
                var tcpClientStream = tcpClient.GetStream();
                var streamReader = new StreamReader(tcpClientStream);
                await streamReader.ReadAsync(getContent, 0, getContent.Length);
                var getJsonMessage = String.Join("", getContent.Where(x => x != '\0'));

                var getJsonMessagesSplit = getJsonMessage.Split("BONUS").Where(x => x != String.Empty);

                var sendMessageAddingFriendsRequest = sendFriendRequestService.AddFriends();
                var sendMessageStream = sendMessageAddingFriendsRequest.RequestStream;
                foreach (var message in getJsonMessagesSplit)
                {
                    try
                    {
                        var getMessageRequest = JsonConvert.DeserializeObject<SendFriendRequestServiceRequest>(message);
                        await sendMessageStream.WriteAsync(getMessageRequest);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                await sendMessageStream.CompleteAsync();
            });

        }

    }
}