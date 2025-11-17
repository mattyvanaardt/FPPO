using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Input Actions Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Move";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string readyl = "ReadyL";
    [SerializeField] private string readyr = "ReadyR";

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction readyLAction;
    private InputAction readyRAction;

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }

    public bool ReadyL { get; private set; }
    public bool ReadyR { get; private set; }

    public bool LeftJab { get; private set; }
    public bool RightJab { get; private set; }
    public bool leftHook { get; private set; }
    public bool RightHook { get; private set; }
    public bool leftUppercut { get; private set; }
    public bool RightUppercut { get; private set; }
    public bool leftBlock { get; private set; }
    public bool RightBlock { get; private set; }

    private bool leftReadyLastFrame = false;
    private bool rightReadyLastFrame = false;

    private void Awake()
    {
        var map = playerControls.FindActionMap(actionMapName);
        movementAction = map.FindAction(movement);
        rotationAction = map.FindAction(rotation);
        readyLAction = map.FindAction(readyl);
        readyRAction = map.FindAction(readyr);

        SubscribeToInputEvents();
    }

    private void OnEnable() => playerControls.FindActionMap(actionMapName).Enable();
    private void OnDisable() => playerControls.FindActionMap(actionMapName).Disable();

    private void SubscribeToInputEvents()
    {
        // Movement
        movementAction.performed += ctx => MovementInput = ctx.ReadValue<Vector2>();
        movementAction.canceled += ctx => MovementInput = Vector2.zero;

        // Rotation
        rotationAction.performed += ctx => RotationInput = ctx.ReadValue<Vector2>();
        rotationAction.canceled += ctx => RotationInput = Vector2.zero;

        // ReadyL
        readyLAction.started += ctx => ReadyL = true;
        readyLAction.canceled += ctx =>
        {
            ReadyL = false;
            if (MovementInput.magnitude < 0.05f)
            {
                LeftJab = true;
                Debug.Log("Left Jab triggered on release");
            }
        };

        // ReadyR
        readyRAction.started += ctx => ReadyR = true;
        readyRAction.canceled += ctx =>
        {
            ReadyR = false;
            if (MovementInput.magnitude < 0.05f)
            {
                RightJab = true;
                Debug.Log("Right Jab triggered on release");
            }
        };
    }

    private void Update()
    {
        DetectMovementAttacks();

        leftReadyLastFrame = ReadyL;
        rightReadyLastFrame = ReadyR;
    }

    private void DetectMovementAttacks()
    {
        if (ReadyL && MovementInput.magnitude > 0.05f)
        {
            if (MovementInput.x < 0)
            {
                leftHook = true;
                Debug.Log("Left Hook Detected");
            }
            if (MovementInput.y > 0)
            {
                leftUppercut = true;
                Debug.Log("Left Uppercut Detected");
            }
            if (MovementInput.y < 0)
            {
                leftBlock = true;
                Debug.Log("Left Block Detected");
            }
        }

        if (ReadyR && MovementInput.magnitude > 0.05f)
        {
            if (MovementInput.x > 0)
            {
                RightHook = true;
                Debug.Log("Right Hook Detected");
            }
            if (MovementInput.y > 0)
            {
                RightUppercut = true;
                Debug.Log("Right Uppercut Detected");
            }
            if (MovementInput.y < 0)
            {
                RightBlock = true;
                Debug.Log("Right Block Detected");
            }
        }
    }

    public void ResetAttackFlags()
    {
        LeftJab = RightJab = leftHook = RightHook = leftUppercut = RightUppercut = leftBlock = RightBlock = false;
    }
}