using UnityEngine;

public class InputReader : MonoBehaviour
{
    private PlayerControls playerControls;

    private Vector2 movement;
    private Vector2 lastMoveDirection;

    private void Awake() {
        playerControls = new PlayerControls();
    }
    private void Update() {
        PlayerInput();
    }
    public Vector2 GetMovement()
    {
        return movement;
    }

    public Vector2 GetLastMoveDirection()
    {
        return lastMoveDirection;
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        
        if (movement.x != 0 || movement.y != 0){
            lastMoveDirection = movement;
        } 
    }

    private void OnEnable() {
        playerControls.Enable();
    }
}
