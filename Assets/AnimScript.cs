using UnityEngine;

public class AnimScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private Animator animator;

    [Header("Animator Layer Indices")]
    [SerializeField] private int leftArmLayer = 1;

    private bool wasReadyLastFrame = false;

    private void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(leftArmLayer);

        // --- Detect new press of Ready and start animation from beginning ---
        if (inputHandler.ReadyL && !wasReadyLastFrame)
        {
            if (animator.HasState(leftArmLayer, Animator.StringToHash("LeftReady")))
            {
                animator.Play("LeftReady", leftArmLayer, 0f); // Start from beginning
            }
            else
            {
                Debug.LogError("LeftReady state not found in layer " + leftArmLayer);
            }
        }

        // --- Hold Ready ---
        animator.SetBool("LeftReady", inputHandler.ReadyL);

        // --- Trigger Jab only if Ready finished ---
        if (inputHandler.LeftJab && state.IsName("LeftReady") && state.normalizedTime >= 1f)
        {
            Debug.Log("Playing LeftJab after Ready finished");

            if (animator.HasState(leftArmLayer, Animator.StringToHash("LeftJab")))
            {
                animator.CrossFadeInFixedTime("LeftJab", 0f, leftArmLayer);
            }
            else
            {
                Debug.LogError("LeftJab state not found in layer " + leftArmLayer);
            }

            // Reset the flag so it only fires once
            inputHandler.ResetAttackFlags();
        }

        // Keep layer active if Ready held or Jab playing
        animator.SetLayerWeight(leftArmLayer,
            inputHandler.ReadyL || state.IsName("LeftJab") ? 1f : 0f);

        // --- Movement attack debug logs ---
        if (inputHandler.MovementInput.magnitude > 0.05f)
        {
            if (inputHandler.MovementInput.x < 0) Debug.Log("Left Hook Detected");
            if (inputHandler.MovementInput.y > 0) Debug.Log("Left Uppercut Detected");
            if (inputHandler.MovementInput.y < 0) Debug.Log("Left Block Detected");
        }

        // Track previous frame for Ready press detection
        wasReadyLastFrame = inputHandler.ReadyL;
    }
}