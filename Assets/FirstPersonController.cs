using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed = 3f;

    [Header("Mouse Sensitivity")]
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float UpDownLookRange = 80f;

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InputHandler playerInputHandler;

    private Vector3 currentMovement;
    private float verticalLookRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleGrounded();
        CheckAttacks();
    }

    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        return transform.TransformDirection(inputDirection).normalized;
    }

    private void HandleGrounded()
    {
        if (characterController.isGrounded)
            currentMovement.y = -0.5f;
        else
            currentMovement.y += Physics.gravity.y * Time.deltaTime;
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

    private void ApplyVerticalRotation(float rotationAmount)
    {
        verticalLookRotation = Mathf.Clamp(verticalLookRotation - rotationAmount, -UpDownLookRange, UpDownLookRange);
        playerCamera.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0, 0);
    }

    private void HandleRotation()
    {
        float mouseX = playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseY = playerInputHandler.RotationInput.y * mouseSensitivity;

        ApplyHorizontalRotation(mouseX);
        ApplyVerticalRotation(mouseY);
    }

    private void CheckAttacks()
    {
        if (playerInputHandler.LeftJab) Debug.Log("Left Jab");
        if (playerInputHandler.RightJab) Debug.Log("Right Jab");
        if (playerInputHandler.leftHook) Debug.Log("Left Hook");
        if (playerInputHandler.RightHook) Debug.Log("Right Hook");
        if (playerInputHandler.leftUppercut) Debug.Log("Left Uppercut");
        if (playerInputHandler.RightUppercut) Debug.Log("Right Uppercut");
        if (playerInputHandler.leftBlock) Debug.Log("Left Block");
        if (playerInputHandler.RightBlock) Debug.Log("Right Block");

        playerInputHandler.ResetAttackFlags();
    }
}