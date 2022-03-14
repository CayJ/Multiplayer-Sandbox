using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Networking
{
    /*
     * A simple TCP networking server that uses the TcpListener provided in .NET
     */
    public class NetworkServer
    {
        private TcpListener _tcpListener;
        private NetworkStream _tcpStream;
        private byte[] _buffer = new byte[1024];
        

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

        private void OnClientConnected(IAsyncResult result)
        {
            _tcpListener.EndAcceptTcpClient(result);

            Debug.Log("Server received client connection");
        }
    }
}