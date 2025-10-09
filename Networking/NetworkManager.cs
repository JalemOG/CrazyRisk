using System;
using System.Net;
using System.Net.Sockets;

namespace CrazyRisk.Networking
{
    public class NetworkManager : IDisposable
    {
        private TcpListener?  server;
        private TcpClient?    client;
        private NetworkStream? stream;

        public bool IsServer { get; private set; }
        public bool IsConnected => stream is not null && client is not null && client.Connected;

        public bool IsListening => server?.Server?.IsBound == true;
        public IPEndPoint? LocalEndpoint => server is null ? null : (IPEndPoint)server.LocalEndpoint;
        public IPEndPoint? RemoteEndpoint => client?.Client?.RemoteEndPoint as IPEndPoint;

        public void StartServer(int port, IPAddress? address = null, int backlog = 10)
        {
            if (server is not null) throw new InvalidOperationException("Servidor ya iniciado.");
            IsServer = true;
            address ??= IPAddress.Any; // IPv4
            server = new TcpListener(address, port);
            server.Start(backlog);
        }

        public void AcceptClient()
        {
            if (server is null) throw new InvalidOperationException("Servidor no iniciado.");
            client = server.AcceptTcpClient();
            stream = client.GetStream();
        }

        public bool TryAcceptClient(int msTimeout)
        {
            if (server is null) throw new InvalidOperationException("Servidor no iniciado.");
            var end = DateTime.UtcNow.AddMilliseconds(msTimeout);
            while (DateTime.UtcNow < end)
            {
                if (server.Pending())
                {
                    client = server.AcceptTcpClient();
                    stream = client.GetStream();
                    return true;
                }
                System.Threading.Thread.Sleep(25);
            }
            return false;
        }

        public void Connect(string host, int port)
        {
            if (client is not null) throw new InvalidOperationException("Cliente ya conectado o en uso.");

            // Fuerza IPv4 si pasan IPv6-mapeada
            host = host.Trim();
            if (host.StartsWith("[") && host.EndsWith("]")) host = host.Substring(1, host.Length - 2);
            const string ffff = "::ffff:";
            var idx = host.LastIndexOf(ffff, StringComparison.OrdinalIgnoreCase);
            if (idx >= 0) host = host[(idx + ffff.Length)..];

            var ip = IPAddress.TryParse(host, out var parsed) ? parsed : Dns.GetHostEntry(host).AddressList[0];
            var fam = ip.AddressFamily == AddressFamily.InterNetworkV6 ? AddressFamily.InterNetwork : AddressFamily.InterNetwork;
            var c = new TcpClient(fam);
            c.Connect(host, port);
            client = c;
            stream = client.GetStream();
            IsServer = false;
        }

        public void Send(byte[] data, int offset = 0, int? count = null)
        {
            if (stream is null) throw new InvalidOperationException("No hay conexión activa.");
            stream.Write(data, offset, count ?? data.Length);
            stream.Flush();
        }

        public int Receive(byte[] buffer, int offset = 0, int? count = null)
        {
            if (stream is null) throw new InvalidOperationException("No hay conexión activa.");
            return stream.Read(buffer, offset, count ?? buffer.Length);
        }

        public void Close()
        {
            try { stream?.Close(); } catch { }
            try { client?.Close(); } catch { }
            try { server?.Stop(); } catch { }
            stream = null; client = null; server = null; IsServer = false;
        }

        public void Dispose() { Close(); GC.SuppressFinalize(this); }
    }
}
