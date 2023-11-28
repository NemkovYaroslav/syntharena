using Network;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(ServerPlayerMove))]
    [DefaultExecutionOrder(1)]
    public class ClientPlayerMove : NetworkBehaviour
    {
        [FormerlySerializedAs("mCharacterController")] [FormerlySerializedAs("m_CharacterController")] [SerializeField]
        private CharacterController characterController;
        
        [FormerlySerializedAs("mFirstPersonController")] [FormerlySerializedAs("m_FirstPersonController")] [SerializeField]
        private FirstPersonController firstPersonController;
        
        [FormerlySerializedAs("mPlayerCamera")] [FormerlySerializedAs("m_PlayerCamera")] [SerializeField]
        private Camera playerCamera;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            firstPersonController.enabled = false;
            characterController.enabled = false;
            playerCamera.enabled = false;
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            characterController.enabled = true;
            
            enabled = IsClient;
            if (!IsOwner)
            {
                enabled = false;
                return;
            }
            
            firstPersonController.enabled = true;
            
            playerCamera.enabled = true;
        }
    }
}
