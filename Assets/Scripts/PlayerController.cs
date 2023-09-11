using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private PlayerInputActions inputActions;
    private void Awake()
    {
        inputActions = new();
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector.Normalize();
        return inputVector;
    }
}
