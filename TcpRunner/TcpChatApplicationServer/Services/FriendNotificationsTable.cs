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
    public class FriendNotificationsTable : IServiceClient
    {
        public FriendNotificationsTable()
        {

        }
        public void Handle(ref List<ClientInformation> clientInformations, TcpClient tcpClient)
        {
            var getMessage = new char[65676];

            while (true)
            {
                var requestsTableNotificationStreamReader = new StreamReader(tcpClient.GetStream());
                requestsTableNotificationStreamReader.Read(getMessage, 0, getMessage.Length);

                var getSeperatedMessages = String.Join("", getMessage.Where(x => x != '\0')).Split("BONUS").Where(x => x != String.Empty);

                foreach (var seperatedMessage in getSeperatedMessages)
                {
                    try
                    {
                        var getMessageObject = JsonConvert.DeserializeObject<FriendNotification>(seperatedMessage);
                        var getSenderInformation = clientInformations.FirstOrDefault(x => x.ClientId == getMessageObject.SenderId);
                        var getSenderClient = getSenderInformation.tcpClient;

                        var getMessageContent = String.Empty;

                        if (getMessageObject.Status == "Accepted")
                        {
                            getMessageContent = getMessageObject.SenderId + " Accepted Your Friend Request";
                        }
                        else if (getMessageObject.Status == "Refused")
                        {
                            getMessageContent = getMessageObject.SenderId + " Refused Your Friend Request";
                        }

                        var streamWriter = new StreamWriter(getSenderClient.GetStream());
                        streamWriter.AutoFlush = true;
                        streamWriter.Write(getMessageContent);
                        streamWriter.Flush();
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
