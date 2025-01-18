using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpChatApplicationServer.Models
{
    public class ClientInformation
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; }
        public TcpClient tcpClient { get; set; }
    }
}
