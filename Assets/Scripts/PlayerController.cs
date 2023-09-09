using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;

    private void Update()
    {
        Vector2 inputVector = new();
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1;
        }
        inputVector.Normalize();
        Vector3 moveDirection = new(inputVector.x, 0.0f, inputVector.y);
        transform.position += movementSpeed * Time.deltaTime * moveDirection;
        float rotationSpeed = 10.0f;
        transform.forward = Vector3.Slerp(transform.forward, 
            moveDirection, Time.deltaTime * rotationSpeed);
        Debug.Log(inputVector);
    }
}
