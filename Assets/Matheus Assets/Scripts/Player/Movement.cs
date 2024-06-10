using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    private InputReader inputReader;
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        inputReader = GetComponent<InputReader>();
    }
    private void FixedUpdate(){
        Move();
    }
    private void Move()
    {
        rb.MovePosition( rb.position + inputReader.GetMovement() * (moveSpeed * Time.fixedDeltaTime) );
    }

}
