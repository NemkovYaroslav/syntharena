using Cinemachine;
using Network;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(ClientNetworkTransform))]
    public class PlayerControllerAlternative : NetworkBehaviour
    {
        private enum PlayerState
        {
            Idle,
            Move
        }

        [Header("Movement Params")] [SerializeField]
        private float walkSpeed = 4.0f;

        [SerializeField] private float sprintSpeed = 8.0f;
        [HideInInspector] private bool isSprinting;

        [SerializeField] private float gravityScale = 1.5f;
        [HideInInspector] private float yVelocity;

        [Header("Look Params")] [SerializeField]
        private Transform cameraTransform;

        private float _mouseX = 0.0f;
        private float _mouseY = 0.0f;
        [SerializeField] private float sensitivity = 1.0f;

        private NetworkVariable<PlayerState> networkPlayerState = new NetworkVariable<PlayerState>();

        public NetworkVariable<float> networkPlayerHealth = new NetworkVariable<float>(100.0f);
        
        private CharacterController _characterController;
        private Animator _animator;

        public override void OnNetworkSpawn()
        {
            CinemachineVirtualCamera cinemachineVirtualCamera =
                cameraTransform.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
    
            cinemachineVirtualCamera.Priority = IsOwner ? 1 : 0;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            if (IsClient && IsOwner)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        void Update()
        {
            if (IsClient && IsOwner)
            {
                ClientInput();
                CharacterControllerGrounded();
                ClientVisuals();
            }
        }

        private void ClientInput()
        {
            isSprinting = Input.GetKey(KeyCode.LeftShift);
            
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            Vector3 moveInput = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

            Vector3 moveDirection = transform.rotation * moveInput * (isSprinting ? sprintSpeed : walkSpeed);
            moveDirection.y = yVelocity;
            moveDirection *= Time.deltaTime;
            _characterController.Move(moveDirection);
            
            _mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
            _mouseY -= Input.GetAxisRaw("Mouse Y") * sensitivity;
            _mouseY = Mathf.Clamp(_mouseY, -90.0f, 90.0f);

            cameraTransform.localRotation = Quaternion.Euler(_mouseY, 0.0f, 0.0f);
            transform.Rotate(0.0f, _mouseX, 0.0f);
            
            UpdatePlayerStateServerRpc(moveInput.normalized.magnitude == 0.0f ? PlayerState.Idle : PlayerState.Move);
        }
        
        private void CharacterControllerGrounded()
        {
            if (yVelocity <= 0.0f && _characterController.isGrounded)
            {
                yVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                yVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
            }
        }
        
        private void ClientVisuals()
        {
            _animator.SetFloat("Speed", networkPlayerState.Value == PlayerState.Idle ? 0.0f : 1.0f);
        }

        [ServerRpc]
        private void UpdatePlayerStateServerRpc(PlayerState newPlayerState)
        {
            networkPlayerState.Value = newPlayerState;
        }
    }
}