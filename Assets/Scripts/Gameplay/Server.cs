using System;
using Networking;
using UnityEngine;

namespace Gameplay
{
    public class Server : MonoBehaviour
    {
        [SerializeField] private Canvas serverCanvas;
        private NetworkServer _networkServer = null;
        public bool StartServer()
        {
            _networkServer = new NetworkServer();

            if (_networkServer.NetworkStart())
            {
                return true;
            }
            
            return false;
        }

        private void Start()
        {
            serverCanvas.gameObject.SetActive(true);
        }
    }
}
