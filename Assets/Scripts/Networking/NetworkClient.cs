using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Networking
{
    /*
     * A simple TCP networking client that uses the TcpClient provided in .NET
     */
    public class NetworkClient
    {
        private TcpClient _tcpClient;
        private NetworkStream _tcpStream;
        private byte[] _buffer = new byte[1024];
        private int _receivedBytes;
        private ConcurrentQueue<string> _messageBuffer = new ConcurrentQueue<string>();

        public bool IsConnected => _tcpClient != null && _tcpClient.Connected;

        public Action<string> OnReceiveServerMessage;
        

        public bool NetworkStart()
        {
            if (_tcpClient == null)
            {
                try
                {
                    _tcpClient = new TcpClient();

                    _tcpClient.Connect(NetworkVariables.IPAddress, NetworkVariables.Port);
                    
                    Debug.Log($"Client connected: {NetworkVariables.IPAddress}:{NetworkVariables.Port}");

                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error starting client ${e.Message}");

                    NetworkStop();
                }
            }
            
            return false;
        }
        
        public void NetworkSendMessage(string message)
        {
            try
            {
                _tcpStream = _tcpClient.GetStream();

                if (!_tcpClient.Connected)
                {
                    throw new Exception("TcpClient not connected");
                }

                // convert client message string to ASCII bytes
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                
                _tcpStream.Write(messageBytes, 0, messageBytes.Length);

                Debug.Log($"Sent message to server: {message}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error sending message to server: {e.Message}");
            }
        }
        
        public void ListenForMessages()
        {
            if (_tcpClient != null)
            {
                _tcpClient.GetStream().BeginRead(_buffer, 0, _buffer.Length, OnServerMessage, _tcpClient.GetStream());
            }

            if (!_messageBuffer.IsEmpty)
            {
                if (_messageBuffer.TryDequeue(out var message))
                {
                    OnReceiveServerMessage.Invoke(message);
                }
            }
        }

        public void NetworkStop()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Close();
            }

            _tcpClient = null;
        }
        
        private void OnServerMessage(IAsyncResult result)
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
