using System;
using Networking;
using UnityEngine;

namespace Gameplay
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private Canvas clientCanvas;
        private NetworkClient _networkClient = null;
        public bool StartClient()
        {
            _networkClient = new NetworkClient();

            if (_networkClient.NetworkStart())
            {
                return true;
            }
            
            return false;
        }

        private void Start()
        {
            clientCanvas.gameObject.SetActive(true);
        }
    }
}
