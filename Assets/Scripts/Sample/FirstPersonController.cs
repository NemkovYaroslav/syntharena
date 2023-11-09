using System;
using Unity.Netcode;
using UnityEngine;

namespace Sample
{
    public class FirstPersonController : MonoBehaviour
    {
        private enum PlayerState
        {
            Idle,
            Move
        }
        
        [Header("Movement Params")] [SerializeField]
        private float walkSpeed = 4.0f;

        [SerializeField] private float sprintSpeed = 8.0f;
        [HideInInspector] private bool _isSprinting;
        [HideInInspector] private PlayerState _playerState;

        [SerializeField] private float gravityScale = 1.5f;
        [HideInInspector] private float _yVelocity;

        [Header("Look Params")] [SerializeField]
        private Transform cameraTransform;

        private float _mouseX = 0.0f;
        private float _mouseY = 0.0f;
        [SerializeField] private float sensitivity = 1.0f;
    
        private CharacterController _characterController;
        private Animator _animator;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
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
        
        private void ClientInput()
        {
            _isSprinting = Input.GetKey(KeyCode.LeftShift);
        
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
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
            _mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
            _mouseY -= Input.GetAxisRaw("Mouse Y") * sensitivity;
            _mouseY = Mathf.Clamp(_mouseY, -90.0f, 90.0f);

            cameraTransform.localRotation = Quaternion.Euler(_mouseY, 0.0f, 0.0f);
            transform.Rotate(0.0f, _mouseX, 0.0f);
        }
    }
}
