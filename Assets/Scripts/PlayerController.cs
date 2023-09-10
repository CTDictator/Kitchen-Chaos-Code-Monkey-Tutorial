using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputActions inputActions;
    private void Awake()
    {
        inputActions = new();
        inputActions.Player.Enable();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();
        inputVector.Normalize();
        return inputVector;
    }
}
