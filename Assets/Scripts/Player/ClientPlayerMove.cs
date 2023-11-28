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
        
        void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //m_CharacterController = GetComponent<CharacterController>();
            //m_FirstPersonController = GetComponent<FirstPersonController>();
            //m_PlayerCamera = GetComponentInChildren<Camera>();
            
            m_FirstPersonController.enabled = false;
            m_CharacterController.enabled = false;
            m_PlayerCamera.enabled = false;
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            m_CharacterController.enabled = true;
            
            enabled = IsClient;
            if (!IsOwner)
            {
                enabled = false;
                //m_CharacterController.enabled = false;
                return;
            }
            
            m_FirstPersonController.enabled = true;
            
            m_PlayerCamera.enabled = true;
        }
    }
}
