using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PL_Controller : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4.0f;
    public float sprintSpeed = 8.0f;
    public bool  isSprinting;
    public float inputSmoothTime = 0.075f;

    [Header("Jumping")]
    public float jumpHeight   = 2.0f;
    public float gravityScale = 1.5f;
    
    [Header("Mouse Look")]
    public Transform lookCamera;
    public float sensitivity = 1.0f;

    private CharacterController characterController;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 playerInput;
    private Vector3 moveDirection;

    private float mouseX;
    private float mouseY;

    private float yVelocity;

    private Vector3 playerInputSmoothed;
    private Vector3 smoothPlayerInput;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
        
        GetInput();
        DoMouseLook();
        FakeThosePhysicsBoy();
        ManipulateController();
    }

    void GetInput()
    {
        // get the "raw" input data without smoothing
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput   = Input.GetAxisRaw("Vertical");

        playerInput = new Vector3(horizontalInput, 0.0f, verticalInput).normalized;

        smoothPlayerInput =
            Vector3.SmoothDamp(smoothPlayerInput, playerInput, ref playerInputSmoothed, inputSmoothTime);

        mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
        mouseY -= Input.GetAxisRaw("Mouse Y") * sensitivity;

        isSprinting = Input.GetKey(KeyCode.LeftShift);

        mouseY = Mathf.Clamp(mouseY, -90.0f, 90.0f);

        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            Jump();
        }
    }

    void DoMouseLook()
    {
        lookCamera.localRotation = Quaternion.Euler(mouseY, 0.0f, 0.0f);
        transform.Rotate(0.0f, mouseX, 0.0f);
    }

    void FakeThosePhysicsBoy()
    {
        if (yVelocity <= 0 && characterController.isGrounded)
        {
            yVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            yVelocity += Physics.gravity.y * gravityScale * Time.deltaTime;
        }
    }

    void Jump()
    {
        yVelocity = Mathf.Sqrt(jumpHeight * -2.0f * Physics.gravity.y);
    }
    
    void ManipulateController()
    {
        moveDirection = transform.rotation * smoothPlayerInput;

        float speed = isSprinting ? sprintSpeed : walkSpeed;
        
        moveDirection *= speed;
        moveDirection.y = yVelocity;
        moveDirection *= Time.deltaTime;

        characterController.Move(moveDirection);
    }
}