using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    [RequireComponent(typeof(ServerPlayerMove))]
    [DefaultExecutionOrder(1)]
    public class ClientPlayerMove : NetworkBehaviour
    {
        [SerializeField]
        CharacterController m_CharacterController;
        
        [SerializeField]
        FirstPersonController m_FirstPersonController;
        
        [SerializeField]
        Camera m_PlayerCamera;
        
        //[SerializeField] Slider m_HealthBar;
        
        public NetworkVariable<int> Health = new NetworkVariable<int>(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            m_FirstPersonController.enabled = false;
            m_CharacterController.enabled = false;
            m_PlayerCamera.enabled = false;
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            enabled = IsClient;
            if (!IsOwner)
            {
                enabled = false;
                m_CharacterController.enabled = false;
                return;
            }
            
            m_FirstPersonController.enabled = true;
            m_CharacterController.enabled = true;
            m_PlayerCamera.enabled = true;
            
            //Health.OnValueChanged += OnHealthChanged;
            //OnHealthChanged(0, Health.Value);
        }
        
        /*
        void OnHealthChanged(int previousValue, int newValue)
        {
            SetHealthBarValue(newValue);
        }
        */
        
        /*
        void SetHealthBarValue(int healthBarValue)
        {
            m_HealthBar.value = healthBarValue;
        }
        */
        
        private void Update()
        {
            Debug.Log(OwnerClientId + " has " + Health.Value + " health");
            
            if (!IsOwner)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                TakeDamage(10);
            }
        }
        
        private void TakeDamage(int amount)
        {
            Health.Value = Health.Value - amount;

            if (Health.Value <= 0)
            {
                Health.Value = 100;
                //transform.position = RandomPointsPlayerSpawner.Instance.GetNextSpawnPoint().transform.position;
            }
        }
        
        /*
        public override void OnNetworkDespawn()
        {
            Health.OnValueChanged -= OnHealthChanged;
        }
        */
        
    }
}
