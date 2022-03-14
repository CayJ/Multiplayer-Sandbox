using System;
using System.Net.Sockets;
using UnityEngine;

namespace Networking
{
    /*
     * A simple TCP networking client that uses the TcpClient provided in .NET
     */
    public class NetworkClient
    {
        private TcpClient _tpcClient;
        private NetworkStream _tcpStream;
        private byte[] _buffer = new byte[1024];
        

        public bool NetworkStart()
        {
            if (_tpcClient == null)
            {
                try
                {
                    _tpcClient = new TcpClient();

                    _tpcClient.Connect(NetworkVariables.IPAddress, NetworkVariables.Port);
                    
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

        public void NetworkStop()
        {
            if (_tpcClient != null)
            {
                _tpcClient.Close();
            }

            _tpcClient = null;
        }
    }
}
