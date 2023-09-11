using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs: EventArgs
    {
        public ClearCounter selectedCounter;
    }

    [SerializeField] private float movementSpeed;
    [SerializeField] private PlayerController controller;
    [SerializeField] private LayerMask countersLayerMask;
    private bool isWalking;
    private Vector3 lastInteractDirection;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one instance of player present.");
        }
        else Instance = this;
    }

    private void Start()
    {
        controller.OnInteractAction += Controller_OnInteractAction;
    }

    private void Controller_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = controller.GetMovementVectorNormalized();
        Vector3 moveDirection = new(inputVector.x, 0.0f, inputVector.y);
        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection;
        }
        float interactDistance = 2.0f;
        if (Physics.Raycast(transform.position, lastInteractDirection, 
            out RaycastHit hit, interactDistance, countersLayerMask))
        {
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Has a clear counter component.
                if (clearCounter != selectedCounter)
                {
                    SetSelectedCounter(clearCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = controller.GetMovementVectorNormalized();
        Vector3 moveDirection = new(inputVector.x, 0.0f, inputVector.y);

        float moveDistance = movementSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2.0f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position
            + Vector3.up * playerHeight, playerRadius, moveDirection, moveDistance);

        if (!canMove)
        {
            // Can not move towards the movement direction.
            // Try X Movement.
            Vector3 moveDirX = new Vector3(moveDirection.x, 0.0f, 0.0f).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position
                + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                // Can only move on the X.
                moveDirection = moveDirX;
            }
            else
            {
                // Can't move on X.
                // Try Z movement Instead.
                Vector3 moveDirZ = new Vector3(0.0f, 0.0f, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position
                    + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    // Can move on the Z.
                    moveDirection = moveDirZ;
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDistance * moveDirection;
        }

        isWalking = moveDirection != Vector3.zero;
        float rotationSpeed = 10.0f;
        transform.forward = Vector3.Slerp(transform.forward,
            moveDirection, Time.deltaTime * rotationSpeed);
    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs 
            { selectedCounter = this.selectedCounter });
    }
}
