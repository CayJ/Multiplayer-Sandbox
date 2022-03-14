using System;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private Canvas clientCanvas;
        [SerializeField] private InputField commandInput;
        [SerializeField] private GameObject[] sceneElements;
        private NetworkClient _networkClient = null;
        
        public bool StartClient()
        {
            _networkClient = new NetworkClient();

            if (_networkClient.NetworkStart())
            {
                _networkClient.OnReceiveServerMessage += OnMessageReceived;
                return true;
            }
            
            return false;
        }

        private void Start()
        {
            clientCanvas.gameObject.SetActive(true);
        }

        private void Update()
        {
            // handle client input

            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnSubmitCommand();
            }
            
            if (_networkClient.IsConnected)
            {
                _networkClient.ListenForMessages();
            }
        }

        private void OnSubmitCommand()
        {
            if (commandInput.text.Length > 0)
            {
                if (_networkClient != null)
                {
                    _networkClient.NetworkSendMessage(commandInput.text);
                }

                commandInput.text = string.Empty;
            }
        }
        
        private void OnMessageReceived(string clientMessage)
        {
            ParseMessage(clientMessage);
        }
        
        private void ParseMessage(string message)
        {
            string[] commands = message.Split(' ');

            if (commands.Length > 1)
            {
                if (commands[0] == "SetColor")
                {
                    string colorHex = commands[1];

                    if (ColorUtility.TryParseHtmlString(colorHex, out var color))
                    {
                        foreach (var sceneElement in sceneElements)
                        {
                            sceneElement.GetComponent<Renderer>().material.color = color;
                        }
                    }
                }
            }
        }
    }
}
