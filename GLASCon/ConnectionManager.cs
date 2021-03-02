using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GLASCon
{
    public class ConnectionManager
    {
        // Variables
        Socket socket;
        IPEndPoint inEp = new(IPAddress.Parse("127.0.0.1"), 5001);
        IPEndPoint outEp = new(IPAddress.Parse("127.0.0.1"), 5002);

        public delegate void PacketReceivedHandler(string json);
        public event PacketReceivedHandler PacketReceived;

        public ConnectionManager()
        {
            socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Bind(inEp);

            _ = AwaitPacketAsync().ConfigureAwait(false);
        }

        private async Task AwaitPacketAsync()
        {

            while (true)
            {
                byte[] data = new byte[8192];

                try
                {
                    var result = await socket.ReceiveFromAsync(data, SocketFlags.None, inEp);
                }
                catch (Exception)
                {
                    continue;
                }

                Debug.WriteLine("Got packet!");

                PacketReceived?.Invoke(Encoding.ASCII.GetString(data).Trim());
            }
        }

        public async Task SendPacketAsync(string json)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(json);
                await socket.SendToAsync(data, SocketFlags.None, outEp);
            }
            catch (Exception)
            {
                return;
            }

            Debug.WriteLine("Packet sent!");
        }
    }
}
