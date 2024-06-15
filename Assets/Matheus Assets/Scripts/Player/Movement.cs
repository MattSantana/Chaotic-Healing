using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 1f;
    [HideInInspector] public float currentSpeed;

    private InputReader inputReader;
    private Rigidbody2D rb;

    public bool canMove = true;
    private CombatSkills combat;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        inputReader = GetComponent<InputReader>();
        combat  = GetComponent<CombatSkills>();
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        if (canMove)
        {
            if(combat.isDashing) { return; }
            if(combat.isSlaming) { return; }
            Move();
        }
    }

    private void Move()
    {
        rb.MovePosition(rb.position + inputReader.GetMovement() * (currentSpeed * Time.fixedDeltaTime));
    }
}