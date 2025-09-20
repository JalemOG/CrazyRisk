using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CrazyRisk.Networking
{
    public class NetworkManager
    {
        private bool isServer;
        private TcpListener server;
        private TcpClient client;
        private NetworkStream stream;
        
        public NetworkManager(bool isServer)
        {
            this.isServer = isServer;
        }
        
        public void StartServer(string ip, int port)
        {
            if (!isServer) return;
            
            server = new TcpListener(IPAddress.Parse(ip), port);
            server.Start();
            Console.WriteLine("Servidor iniciado...");
        }
        
        public void ConnectClient(string ip, int port)
        {
            if (isServer) return;
            
            client = new TcpClient();
            client.Connect(ip, port);
            stream = client.GetStream();
            Console.WriteLine("Conectado al servidor...");
        }
        
        public void SendMessage(Message msg)
        {
            string data = msg.Serialize();
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            
            if (isServer && client != null)
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            else if (!isServer && stream != null)
            {
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        
        public Message ReceiveMessage()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            return Message.Deserialize(data);
        }
        
        public void CloseConnection()
        {
            stream?.Close();
            client?.Close();
            server?.Stop();
        }
    }
}