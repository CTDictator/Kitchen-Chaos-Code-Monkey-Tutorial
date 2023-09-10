using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private PlayerController controller;
    private bool isWalking;

    private void Update()
    {
        Vector2 inputVector = controller.GetMovementVectorNormalized();
        Vector3 moveDirection = new(inputVector.x, 0.0f, inputVector.y);
        transform.position += movementSpeed * Time.deltaTime * moveDirection;
        isWalking = moveDirection != Vector3.zero;
        float rotationSpeed = 10.0f;
        transform.forward = Vector3.Slerp(transform.forward,
            moveDirection, Time.deltaTime * rotationSpeed);
        Debug.Log(inputVector);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
