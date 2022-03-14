using System;
using Networking;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Intro : MonoBehaviour
    {
        [SerializeField] private Canvas introCanvas;
        [SerializeField] private Button startServerButton;
        [SerializeField] private Button startClientButton;
        [SerializeField] private Text debugMessageText;
        
        [SerializeField] private Client client;
        [SerializeField] private Server server;
        
        void Awake()
        {
            // Caps the application framerate at 60
            Application.targetFrameRate = 60;
            
            Cursor.lockState = CursorLockMode.None;
            Screen.SetResolution(512, 512, FullScreenMode.Windowed);
        }

        private void Start()
        {
            startServerButton.onClick.AddListener(OnStartServer);
            startClientButton.onClick.AddListener(OnStartClient);
        }

        private void OnStartServer()
        {
            SetDebugText("Starting server...");

            if (server.StartServer())
            {
                server.gameObject.SetActive(true);
                introCanvas.gameObject.SetActive(false);
            }
            else
            {
                SetDebugText("Error starting client. Make sure that a server is running.");
            }
        }

        private void OnStartClient()
        {
            SetDebugText("Starting client...");

            if (client.StartClient())
            {
                client.gameObject.SetActive(true);
                introCanvas.gameObject.SetActive(false);
            }
            else
            {
                SetDebugText("Error starting client. Make sure that a server is running.");
            }
        }

        private void SetDebugText(string message)
        {
            debugMessageText.text = message;
        }
    }
}
