using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private Inventory inventory;
    private CauldronInventory cauldronInventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        cauldronInventory = FindObjectOfType<CauldronInventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CollectableItem item = collision.GetComponent<CollectableItem>();

        if (item != null)
        {
            bool collected = inventory.AddItem(item);

            if (collected)
            {
                cauldronInventory.AddItemToCauldronInventory(item.icon, item.itemName);
                Destroy(item.gameObject);
            }
        }
    }
}