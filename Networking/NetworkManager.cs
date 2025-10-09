using System;
using System.Net;
using System.Net.Sockets;

namespace CrazyRisk.Networking
{
    /// <summary>
    /// Maneja servidor/cliente TCP y operaciones de envío/recepción de forma segura.
    /// Incluye propiedades para verificar si el servidor está escuchando y el endpoint remoto.
    /// </summary>
    public class NetworkManager : IDisposable
    {
        private TcpListener?  server;   // null hasta StartServer()
        private TcpClient?    client;   // null hasta Connect()/AcceptClient()
        private NetworkStream? stream;  // null hasta tener client

        public bool IsServer { get; private set; }
        public bool IsConnected => stream is not null && client is not null && client.Connected;

        /// <summary>True si el TcpListener está activo.</summary>
        public bool IsListening => server?.Server?.IsBound == true;

        /// <summary>Endpoint local del servidor (ip:puerto) si está activo.</summary>
        public IPEndPoint? LocalEndpoint =>
            server is null ? null : (IPEndPoint)server.LocalEndpoint;

        /// <summary>Endpoint remoto del cliente conectado (si existe).</summary>
        public IPEndPoint? RemoteEndpoint =>
            client?.Client?.RemoteEndPoint as IPEndPoint;

        /// <summary>Inicia un servidor TCP en el puerto indicado.</summary>
        public void StartServer(int port, IPAddress? address = null, int backlog = 10)
        {
            if (server is not null)
                throw new InvalidOperationException("El servidor ya está iniciado.");

            IsServer = true;
            address ??= IPAddress.Any;

            server = new TcpListener(address, port);
            server.Start(backlog);
        }

        /// <summary>Acepta un cliente entrante (bloqueante).</summary>
        public void AcceptClient()
        {
            if (server is null)
                throw new InvalidOperationException("Servidor no iniciado. Llama a StartServer primero.");

            client = server.AcceptTcpClient();
            stream = client.GetStream();
        }

        /// <summary>
        /// Intenta aceptar un cliente durante un tiempo máximo (no bloquea indefinidamente).
        /// </summary>
        public bool TryAcceptClient(int millisecondsTimeout)
        {
            if (server is null)
                throw new InvalidOperationException("Servidor no iniciado. Llama a StartServer primero.");

            var start = DateTime.UtcNow;
            while ((DateTime.UtcNow - start).TotalMilliseconds < millisecondsTimeout)
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

        /// <summary>Conecta como cliente a un host/puerto.</summary>
        public void Connect(string host, int port)
        {
            if (client is not null)
                throw new InvalidOperationException("Cliente ya conectado o en uso.");

            IsServer = false;

            var c = new TcpClient();
            c.Connect(host, port);
            client = c;
            stream = client.GetStream();
        }

        /// <summary>Envía bytes por el stream activo.</summary>
        public void Send(byte[] data, int offset = 0, int? count = null)
        {
            if (stream is null)
                throw new InvalidOperationException("No hay conexión activa (stream == null).");

            int len = count ?? data.Length;
            stream.Write(data, offset, len);
            stream.Flush();
        }

        /// <summary>Lee bytes del stream activo y devuelve la cantidad leída.</summary>
        public int Receive(byte[] buffer, int offset = 0, int? count = null)
        {
            if (stream is null)
                throw new InvalidOperationException("No hay conexión activa (stream == null).");

            int len = count ?? buffer.Length;
            return stream.Read(buffer, offset, len);
        }

        /// <summary>Cierra conexiones y libera recursos.</summary>
        public void Close()
        {
            try { stream?.Close(); } catch { /* ignore */ }
            try { client?.Close(); } catch { /* ignore */ }
            try { server?.Stop(); } catch { /* ignore */ }

            stream = null;
            client = null;
            server = null;
            IsServer = false;
        }

        public void Dispose()
        {
            Close();
            GC.SuppressFinalize(this);
        }
    }
}
