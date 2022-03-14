using System;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Server : MonoBehaviour
    {
        [SerializeField] private Canvas serverCanvas;
        [SerializeField] private GameObject messageContainer;
        [SerializeField] private GameObject messagePrefab;
        [SerializeField] private GameObject[] sceneElements;
        
        private NetworkServer _networkServer = null;
        public bool StartServer()
        {
            _networkServer = new NetworkServer();

            if (_networkServer.NetworkStart())
            {
                _networkServer.OnReceiveClientMessage += OnMessageReceived;
                
                return true;
            }
            
            return false;
        }

        private void Start()
        {
            serverCanvas.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (_networkServer.IsConnected)
            {
                _networkServer.ListenForMessages();
            }
        }

        private void OnMessageReceived(string clientMessage)
        {
            GameObject messageGameObject = Instantiate(messagePrefab);
            Text message = messageGameObject.GetComponent<Text>();

            message.text = $" Client: {clientMessage}";

            messageGameObject.SetActive(true);

            message.transform.SetParent(messageContainer.transform);
            message.transform.SetSiblingIndex(0);

            ParseMessage(clientMessage);
        }

        private void ParseMessage(string message)
        {
            string[] commands = message.Split(' ');

            if (commands.Length > 1)
            {
                if (commands[0] == "/color")
                {
                    string colorHex = commands[1];

                    if (ColorUtility.TryParseHtmlString(colorHex, out var color))
                    {
                        foreach (var sceneElement in sceneElements)
                        {
                            sceneElement.GetComponent<Renderer>().material.color = color;
                        }

                        _networkServer.SendMessageToClient($"SetColor {colorHex}");
                    }
                }
            }
        }
    }
}
