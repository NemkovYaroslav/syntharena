using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Network
{
    public class UINetworkManager : MonoBehaviour
    {
        [SerializeField] private Button startServerButton;
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;
    
        [SerializeField]
        private TextMeshProUGUI playersInGameText;
    
        // because we use multiple screens
        private void Awake()
        {
            Cursor.visible = true;
        }
    
        private void Update()
        {
            playersInGameText.text = $"Players: {PlayerManager.Instance.PlayersInGame}";
        }

        private void Start()
        {
            startHostButton.onClick.AddListener(
                () =>
                {
                    Debug.unityLogger.Log(LogType.Log,
                        NetworkManager.Singleton.StartHost() ? "Host started..." : "Host could not be started...");
                    
                    DisableButtons();
                }
            );
            startServerButton.onClick.AddListener(
                () =>
                {
                    Debug.unityLogger.Log(LogType.Log,
                        NetworkManager.Singleton.StartServer() ? "Server started..." : "Server could not be started...");
                    
                    DisableButtons();
                }
            );
            startClientButton.onClick.AddListener(
                () =>
                {
                    Debug.unityLogger.Log(LogType.Log,
                        NetworkManager.Singleton.StartClient() ? "Client started..." : "Client could not be started...");
                    
                    DisableButtons();
                }
            );
        }

        private void DisableButtons()
        {
            startHostButton.enabled = false;
            startServerButton.enabled = false;
            startClientButton.enabled = false;
        }
    }
}
