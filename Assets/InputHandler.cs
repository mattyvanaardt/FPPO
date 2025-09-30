using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private string readyr = "ReadyR";
    [SerializeField] private string readyl = "ReadyL";
    [SerializeField] private string dodge = "Dodge";

    // headers and serialized fields for organisation and easy access in the inspector

    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction readyrAction;
    private InputAction readylAction;
    private InputAction dodgeAction;

    // declaring the input action variables

    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool ReadyRTriggered { get; private set; }
    public bool ReadyLTriggered { get; private set; }
    public bool DodgeTriggered { get; private set; }

    //declaring the input variables 

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

         movementAction = mapReference.FindAction(movement);
         rotationAction = mapReference.FindAction(rotation);
         readyrAction = mapReference.FindAction(readyr);
         readylAction = mapReference.FindAction(readyl);
         dodgeAction = mapReference.FindAction(dodge);

        //assigning input actions to the variables
        SubscribeActionvaluesToInputEvents();
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }   

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }

    private void SubscribeActionvaluesToInputEvents()
    {
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;


        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        readyrAction.performed += inputInfo => ReadyRTriggered = true;
        readyrAction.canceled += inputInfo => ReadyRTriggered = false;

        readylAction.performed += inputInfo => ReadyLTriggered = true;
        readylAction.canceled += inputInfo => ReadyLTriggered = false;

        dodgeAction.performed += inputInfo => DodgeTriggered = true;
        dodgeAction.canceled += inputInfo => DodgeTriggered = false;

        //Assigning the input values to the variables
        //Using lambda expressions for anonymous functions inline 
    }



}
