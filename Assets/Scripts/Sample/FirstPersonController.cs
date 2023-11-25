using UnityEngine;
using UnityEngine.InputSystem;

namespace Sample
{
    public class FirstPersonController : MonoBehaviour
    {
        private enum PlayerState
        {
            Idle,
            Move
        }

        [SerializeField] private InputManager inputManager;
        
        [Header("Movement Params")]
        [SerializeField] private float walkSpeed = 4.0f;
        [SerializeField] private float sprintSpeed = 8.0f;
        private bool _isSprinting = false;
        private PlayerState _playerState;

        [Header("Look Params")]
        [SerializeField] private float sensitivity = 1.0f;
        [SerializeField] private Transform cameraTransform;
        private float _mouseX = 0.0f;
        private float _mouseY = 0.0f;

        [Header("Jump Params")]
        [SerializeField] private float jumpHeight = 2.0f;
        [SerializeField] private float gravityScale = 1.5f;
        private float _yVelocity;
        
        private CharacterController _characterController;
        private Animator _animator;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();

            inputManager.inputMaster.Player.Jump.started += _ => ClientJump();
            
            inputManager.inputMaster.Player.Sprint.started += _ => ClientSprint();
            inputManager.inputMaster.Player.Sprint.canceled += _ => ClientSprint();
        }

        private void Update()
        {
            CharacterControllerGrounded();
            ClientInput();
            ClientVisuals();
        }
        
        private void CharacterControllerGrounded()
        {
            if (_yVelocity <= 0.0f && _characterController.isGrounded)
            {
                _yVelocity = Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                _yVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
            }
        }

        private void ClientJump()
        {
            _yVelocity = Mathf.Sqrt(jumpHeight * -2.0f * Physics.gravity.y);
        }

        private void ClientSprint()
        {
            _isSprinting = !_isSprinting;
        }
        
        private void ClientInput()
        {
            float horizontalInput = inputManager.inputMaster.Player.Move.ReadValue<Vector2>().x;
            float verticalInput = inputManager.inputMaster.Player.Move.ReadValue<Vector2>().y;
            Vector3 moveInput = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

            Vector3 moveDirection = transform.rotation * moveInput * (_isSprinting ? sprintSpeed : walkSpeed);
            moveDirection.y = _yVelocity;
            moveDirection *= Time.deltaTime;
            _characterController.Move(moveDirection);

            _playerState = (moveInput.normalized.magnitude == 0.0f ? PlayerState.Idle : PlayerState.Move);
        }
        
        private void ClientVisuals()
        { 
            _animator.SetFloat("Speed", _playerState == PlayerState.Idle ? 0.0f : 1.0f);
        }
        
        private void LateUpdate()
        {
            CameraMove();
        }

        private void CameraMove()
        {
            _mouseX = inputManager.inputMaster.Player.Look.ReadValue<Vector2>().x * sensitivity * 0.05f;
            _mouseY -= inputManager.inputMaster.Player.Look.ReadValue<Vector2>().y * sensitivity * 0.05f;
            _mouseY = Mathf.Clamp(_mouseY, -90.0f, 90.0f);

            cameraTransform.localRotation = Quaternion.Euler(_mouseY, 0.0f, 0.0f);
            transform.Rotate(0.0f, _mouseX, 0.0f);
        }
    }
}
