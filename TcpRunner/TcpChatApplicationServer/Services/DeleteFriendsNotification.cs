using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpChatApplicationServer.Models;

namespace TcpChatApplicationServer.Services
{
    public class DeleteFriendsNotification : IServiceClient
    {
        public DeleteFriendsNotification()
        {

        }
        public void Handle(ref List<ClientInformation> clientInformations, TcpClient tcpClient)
        {
            var getMessage = new char[1024];

            while (true)
            {
                var requestsTableNotificationStreamReader = new StreamReader(tcpClient.GetStream());
                requestsTableNotificationStreamReader.Read(getMessage, 0, getMessage.Length);

                var getSeperatedMessages = String.Join("", getMessage.Where(x => x != '\0')).Split("BONUS").Where(x => x != String.Empty);

                foreach (var seperatedMessage in getSeperatedMessages)
                {
                    try
                    {
                        var getMessageObject = JsonConvert.DeserializeObject<DeleteFriendNotification>(seperatedMessage);
                        var getClientInformation = clientInformations.FirstOrDefault(x => x.ClientId == getMessageObject.ReceiverId);
                        if (getClientInformation != null)
                        {
                            var getReceiverClient = getClientInformation.tcpClient;
                            var getMessageContent = String.Empty;
                            getMessageContent = getMessageObject.SenderId + " Has Removed You From His/Her Friends List";
                            var streamWriter = new StreamWriter(getReceiverClient.GetStream());
                            streamWriter.AutoFlush = true;
                            streamWriter.Write(getMessageContent);
                            streamWriter.Flush();
                        }
                        var getSenderInformation = clientInformations.FirstOrDefault(x => x.ClientId == getMessageObject.SenderId);
                        if (getSenderInformation != null)
                        {
                            var getSenderClient = getSenderInformation.tcpClient;
                            var getMessageContent = String.Empty;
                            getMessageContent = "You Have Removed " + getMessageObject.ReceiverId + " From Your Friends List";
                            var streamWriter = new StreamWriter(getSenderClient.GetStream());
                            streamWriter.AutoFlush = true;
                            streamWriter.Write(getMessageContent);
                            streamWriter.Flush();
                        }
                        getMessage = new char[1024];
                    }
                    catch (Exception ex)
                    {
                        getMessage = new char[1024];
                        continue;
                    }
                    getMessage = new char[1024];
                }
            }
        }
    }
}
