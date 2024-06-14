using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1f;
    [HideInInspector] public float currentSpeed;

    private InputReader inputReader;
    private Rigidbody2D rb;

    public bool canMove = true;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        inputReader = GetComponent<InputReader>();

        currentSpeed = moveSpeed;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void Move()
    {
        rb.MovePosition(rb.position + inputReader.GetMovement() * (currentSpeed * Time.fixedDeltaTime));
    }
}