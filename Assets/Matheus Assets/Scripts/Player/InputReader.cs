using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerControls playerControls;
    private Inventory inventory;
    private PotionCrafting potionCrafting;
    private PlayerInventory playerInventory;

    private Vector2 movement;
    private Vector2 lastMoveDirection;

    private void Awake() 
    {
        playerControls = new PlayerControls();
        inventory = FindObjectOfType<Inventory>();
        potionCrafting = FindObjectOfType<PotionCrafting>();
        playerInventory = FindAnyObjectByType<PlayerInventory>();
    }

    private void Update() 
    {
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

    private void OnEnable() 
    {
        playerControls.Enable();
        playerControls.UI.ToggleInventory.performed += ToggleInventory;
        playerControls.UI.ZInteraction.performed += CreatePotion;
        playerControls.UI.ZInteraction.performed += Crafting;
        playerControls.UI.ZInteraction.performed += CollectItem;
    }

    private void ToggleInventory(InputAction.CallbackContext context)
    {
        inventory.ToggleInventory();
    }

    private void CreatePotion(InputAction.CallbackContext context)
    {
        potionCrafting.CreatePotion();
    }

    private void Crafting(InputAction.CallbackContext context)
    {
        potionCrafting.Crafting();
    }

    private void CollectItem(InputAction.CallbackContext context)
    {
        playerInventory.CollectOrMoveClosestItem();
    }
}