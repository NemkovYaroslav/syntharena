using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player
{
    [RequireComponent(typeof(NetworkObject))]
    [RequireComponent(typeof(NetworkTransform))]
    public class PlayerController : NetworkBehaviour
    {
        private enum PlayerState
        {
            Idle,
            Move
        }

        [Header("Spawn Params")]
        [SerializeField] private float positionRange = 5.0f;
        
        [Header("Movement Params")]
        [SerializeField] public float walkSpeed = 4.0f;
        [SerializeField] public float sprintSpeed = 8.0f;
        [HideInInspector] public bool isSprinting;

        [Header("Look Params")]
        [SerializeField] private Transform cameraTransform;
        [HideInInspector] private float mouseX = 0.0f;
        [HideInInspector] private float mouseY = 0.0f;
        [SerializeField] private float sensitivity = 1.0f;
        
        private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();
        
        private NetworkVariable<Vector3> networkPositionDirection = new NetworkVariable<Vector3>();
        private NetworkVariable<Vector3> networkRotationDirection = new NetworkVariable<Vector3>();
        private Vector3 _oldInputPosition;
        private Vector3 _oldInputRotation;

        private CharacterController _characterController;
        private Animator _animator;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            transform.position =
                new Vector3(Random.Range(positionRange, -positionRange), 0.0f,
                    Random.Range(positionRange, -positionRange));
        }

        void Update()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (IsClient && IsOwner)
            {
                ClientInput();
            }

            ClientMoveAndRotate();
            ClientVisuals();
        }

        private void ClientInput()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 moveInput = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

            isSprinting = Input.GetKey(KeyCode.LeftShift);
            
            mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
            mouseY -= Input.GetAxisRaw("Mouse Y") * sensitivity;
            mouseY = Mathf.Clamp(mouseY, -90.0f, 90.0f);
            Vector3 lookInput = new Vector3(mouseX, mouseY, 0.0f);
            
            if (_oldInputPosition != moveInput || _oldInputRotation != lookInput)
            {
                _oldInputPosition = moveInput;
                _oldInputRotation = lookInput;
                
                UpdateClientPositionAndRotationServerRpc(moveInput, lookInput);
            }
            
            UpdatePlayerStateServerRpc(moveInput.normalized.magnitude == 0.0f ? PlayerState.Idle : PlayerState.Move);
        }

        [ServerRpc]
        private void UpdateClientPositionAndRotationServerRpc(Vector3 newPositionDirection, Vector3 newRotationDirection)
        {
            networkPositionDirection.Value = newPositionDirection;
            networkRotationDirection.Value = newRotationDirection;
        }

        [ServerRpc]
        private void UpdatePlayerStateServerRpc(PlayerState newPlayerState)
        {
            networkPlayerState.Value = newPlayerState;
        }

        private void ClientMoveAndRotate()
        {
            if (networkPositionDirection.Value != Vector3.zero)
            { 
                Vector3 moveDirection = 
                    transform.rotation * networkPositionDirection.Value 
                                       * (Time.deltaTime * (isSprinting ? sprintSpeed : walkSpeed));
                
                _characterController.Move(moveDirection);
            }
            
            if (networkRotationDirection.Value != Vector3.zero)
            {
                cameraTransform.localRotation = Quaternion.Euler(networkRotationDirection.Value.y, 0.0f, 0.0f);
                transform.Rotate(0.0f, networkRotationDirection.Value.x, 0.0f);
            }
        }

        private void ClientVisuals()
        {
            _animator.SetFloat("Speed", networkPlayerState.Value == PlayerState.Idle ? 0.0f : 1.0f);
        }
    }
}