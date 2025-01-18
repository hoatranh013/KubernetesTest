using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpChatApplicationServer.Models;

namespace TcpChatApplicationServer.Services
{
    public interface IServiceClient
    {
        public void Handle(ref List<ClientInformation> clientInformations, TcpClient tcpClient);
    }
}
