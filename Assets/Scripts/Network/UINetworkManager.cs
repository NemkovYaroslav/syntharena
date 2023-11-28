using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Network
{
    public class UINetworkManager : MonoBehaviour
    {
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;
    
        [SerializeField] private TextMeshProUGUI playersInGameText;

        [SerializeField] private Canvas mainMenuHolder;
        [SerializeField] private Canvas mobileControl;
    
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
                    EnableMobileControllers();
                    DisableMainMenu();
                }
            );
            startClientButton.onClick.AddListener(
                () =>
                {
                    Debug.unityLogger.Log(LogType.Log,
                        NetworkManager.Singleton.StartClient() ? "Client started..." : "Client could not be started...");
                    
                    DisableButtons();
                    EnableMobileControllers();
                    DisableMainMenu();
                }
            );
        }

        private void DisableButtons()
        {
            startHostButton.enabled = false;
            startClientButton.enabled = false;
        }

        private void EnableMobileControllers()
        {
            mobileControl.gameObject.SetActive(true);
        }

        private void DisableMainMenu()
        {
            mainMenuHolder.gameObject.SetActive(false);
        }
    }
}
