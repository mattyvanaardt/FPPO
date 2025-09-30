using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Xml.Serialization;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3f;

    [Header("Mouse Sensitivity")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float UpDownLookRange = 80f;

    [Header("Dodge Settings")]
    [SerializeField] private float dodgeDistance = 5f;
    [SerializeField] private float dodgeCooldown = 1f;
    [SerializeField] private float dodgeIFrames = 0.2f;

    [Header("Ready R settings")]
    [SerializeField] private float readyRTime = 0.5f;

    [Header("Ready L settings")]
    [SerializeField] private float readyLTime = 0.5f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InputHandler playerInputHandler;

    private Vector3 currentMovement;
    private float verticalLookRotation;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleGrounded();
    }

    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized;
    }

    private void HandleGrounded()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;
        }
        else
        {
            currentMovement.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        currentMovement.x = worldDirection.x * walkSpeed;
        currentMovement.z = worldDirection.z * walkSpeed;
        characterController.Move(currentMovement * Time.deltaTime);

    }
    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0, rotationAmount, 0);

    }

    private void ApplyVerticalRotation (float rotationAmount)
    {
        verticalLookRotation = Mathf.Clamp(verticalLookRotation - rotationAmount, -UpDownLookRange, UpDownLookRange);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);

    }
    private void HandleRotation()
    {
        float mouseXrotation = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseYrotation = playerInputHandler.RotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseXrotation);
        ApplyVerticalRotation(mouseYrotation);
    }
}
