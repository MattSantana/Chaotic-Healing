using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerControls playerControls;
    private Inventory inventory;
    private PotionCrafting potionCrafting;
    private PlayerInventory playerInventory;
    private CombatSkills combat;

    private Vector2 movement;
    private Vector2 lastMoveDirection;

    private void Awake() 
    {
        playerControls = new PlayerControls();
        inventory = FindObjectOfType<Inventory>();
        potionCrafting = FindObjectOfType<PotionCrafting>();
        playerInventory = FindAnyObjectByType<PlayerInventory>();
        combat  = GetComponent<CombatSkills>();
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
        playerControls.Attack.Special.performed += SpecialAttack;
        playerControls.Attack.Normal.performed += NormalAttack;
        playerControls.UI.ZInteraction.performed += CreatePotion;
        playerControls.UI.ZInteraction.performed += Crafting;
        playerControls.UI.ZInteraction.performed += CollectItem;
    }

    private void SpecialAttack(InputAction.CallbackContext context)
    {
        combat.SpecialAttack();
    }

    private void NormalAttack(InputAction.CallbackContext context)
    {
        if(combat.isDashing) { return; }
        if(combat.isSlaming) {return;}

        combat.NormalAttack();
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