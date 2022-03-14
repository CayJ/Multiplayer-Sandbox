using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Networking
{
    /*
     * A simple TCP networking server that uses the TcpListener provided in .NET
     */
    public class NetworkServer
    {
        private TcpListener _tcpListener;
        private TcpClient _tcpClient;
        private byte[] _buffer = new byte[1024];
        private int _receivedBytes;
        private ConcurrentQueue<string> _messageBuffer = new ConcurrentQueue<string>();
        
        public bool IsConnected => _tcpListener != null;

        public Action<string> OnReceiveClientMessage;


        public bool NetworkStart()
        {
            if (_tcpListener == null)
            {
                try
                {
                    _tcpListener = new TcpListener(IPAddress.Parse(NetworkVariables.IPAddress), NetworkVariables.Port);

                    _tcpListener.Start();

                    _tcpListener.BeginAcceptTcpClient(OnClientConnected, null);
                    
                    Debug.Log($"Server connected: {NetworkVariables.IPAddress}:{NetworkVariables.Port}");

                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error starting server ${e.Message}");

                    NetworkStop();
                }
            }
            
            return false;
        }

        public void NetworkStop()
        {
            if (_tcpListener != null)
            {
                _tcpListener.Stop();
            }

            _tcpListener = null;
        }

        public void ListenForMessages()
        {
            if (_tcpListener != null && _tcpClient != null)
            {
                _tcpClient.GetStream().BeginRead(_buffer, 0, _buffer.Length, OnClientMessage, _tcpClient.GetStream());
            }

            if (!_messageBuffer.IsEmpty)
            {
                if (_messageBuffer.TryDequeue(out var message))
                {
                    OnReceiveClientMessage.Invoke(message);
                }
            }
        }

        public void SendMessageToClient(string message)
        {
            if (_tcpListener != null && _tcpClient != null)
            {
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                _tcpClient.GetStream().Write(messageBytes, 0, messageBytes.Length);

                Debug.Log($"Sent message to client: {message}");
            }
        }

        private void OnClientConnected(IAsyncResult result)
        {
            _tcpClient = _tcpListener.EndAcceptTcpClient(result);

            Debug.Log("Server received client connection");
        }

        private void OnClientMessage(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                _receivedBytes = _tcpClient.GetStream().EndRead(result);

                if (_receivedBytes > 0)
                {
                    string message = Encoding.ASCII.GetString(_buffer, 0, _receivedBytes);

                    _messageBuffer.Enqueue(message);
                }
            }
        }
    }
}