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
    public class RequestsTableNotificationTopic : IServiceClient
    {
        public RequestsTableNotificationTopic()
        {

        }
        public void Handle(ref List<ClientInformation> clientInformations, TcpClient tcpClient)
        {
            var getMessage = new char[65676];
            while (true)
            {
                var requestsTableNotificationStreamReader = new StreamReader(tcpClient.GetStream());
                requestsTableNotificationStreamReader.Read(getMessage, 0, getMessage.Length);

                var getJsonBody = String.Join("", getMessage.Where(x => x != '\0'));

                var getJsonsSeperated = getJsonBody.Split("BONUS");

                foreach (var jsonSeperated in getJsonsSeperated)
                {
                    try
                    {
                        var getMessageObject = JsonConvert.DeserializeObject<RequestNotification>(jsonSeperated);
                        var getClientInformation = clientInformations.FirstOrDefault(x => x.ClientId == getMessageObject.ReceiverId);

                        if (getClientInformation != null)
                        {
                            var getReceiverClient = getClientInformation.tcpClient;

                            var getMessageContent = getMessageObject.SenderId + " Send To You A Request Of Adding Friend ";

                            var streamWriter = new StreamWriter(getReceiverClient.GetStream());
                            streamWriter.AutoFlush = true;
                            streamWriter.Write(getMessageContent);
                            streamWriter.Flush();
                            getMessage = new char[1024];
                        }

                        var getSenderInformation = clientInformations.FirstOrDefault(x => x.ClientId == getMessageObject.SenderId);

                        if (getSenderInformation != null)
                        {
                            var getSenderClient = getSenderInformation.tcpClient;

                            var getMessageContent = "Send Message To User " + getMessageObject.ReceiverId + " Successfully";

                            var streamWriter = new StreamWriter(getSenderClient.GetStream());
                            streamWriter.AutoFlush = true;
                            streamWriter.Write(getMessageContent);
                            streamWriter.Flush();
                            getMessage = new char[1024];
                        }
                    }
                    catch (Exception ex)
                    {
                        getMessage = new char[1024];
                        continue;
                    }
                    getMessage = new char[65676];
                }
            }
        }
    }
}
