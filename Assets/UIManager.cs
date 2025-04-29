using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Start Game Panel")]
        public GameObject startGamePanel;
        public TMP_InputField playerCountInput;
        public Button startGameButton;

        [Header("Pass Device Panel")]
        public GameObject passDevicePanel;
        public TMP_Text passDeviceText;
        public Button continueButton;

        [Header("Turn UI")] public GameObject turnPanel;
        public Button endTurnButton;
        public Button rollDiceButton;
        private void Awake()
        {
            Instance = this;

            startGameButton.onClick.AddListener(OnStartGamePressed);
            continueButton.onClick.AddListener(OnContinuePressed);

            startGamePanel.SetActive(true);
            passDevicePanel.SetActive(false);
        }
        
        private void OnTurn()
        {
            passDevicePanel.SetActive(false);
            startGamePanel.SetActive(false);
            turnPanel.SetActive(true);
            
        }
        private void OnStartGamePressed()
        {
            if (int.TryParse(playerCountInput.text, out int playerCount) && playerCount >= 2)
            {
                startGamePanel.SetActive(false);
                PlayerManager.Instance.CreatePlayers(playerCount);
                PlayerManager.Instance.StartGame();
            }
            else
            {
                Debug.LogWarning("Invalid player count. Minimum is 2.");
            }
        }

        public void ShowPassDevice(int nextPlayerNumber)
        {
            turnPanel.SetActive(false);
            passDeviceText.text = $"Pass the device to Player {nextPlayerNumber + 1}";
            passDevicePanel.SetActive(true);
        }

        private void OnContinuePressed()
        {
            passDevicePanel.SetActive(false);
            PlayerManager.Instance.StartCurrentPlayerTurn();
            OnTurn();
        }
    }
}