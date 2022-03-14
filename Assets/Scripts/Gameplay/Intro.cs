using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class Intro : MonoBehaviour
    {
        [SerializeField] private Button startServerButton;
        [SerializeField] private Button startClientButton;
        [SerializeField] private Text debugMessageText;
        void Awake()
        {
            // Caps the application framerate at 60
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            startServerButton.onClick.AddListener(OnStartServer);
            startClientButton.onClick.AddListener(OnStartClient);
        }

        private void OnStartServer()
        {
            SetDebugText("Starting server...");
        }

        private void OnStartClient()
        {
            SetDebugText("Starting client...");
        }

        private void SetDebugText(string message)
        {
            debugMessageText.text = message;
        }
    }
}
