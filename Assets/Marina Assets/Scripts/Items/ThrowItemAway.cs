using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItemAway : MonoBehaviour
{
    [SerializeField] private GameObject dumbSlot;

    private Inventory inventory;
    private CauldronInventory cauldronInventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        cauldronInventory = FindObjectOfType<CauldronInventory>();
    }

    private void Update()
    {
        if (dumbSlot.transform.childCount > 0)
        {
            Transform droppedItemTransform = dumbSlot.transform.GetChild(0);
            GameObject droppedItem = droppedItemTransform.gameObject;
            string itemName = droppedItem.name;

            if (droppedItem != null)
            {
                inventory.DestroyItem(itemName);
                cauldronInventory.DestroyItem(itemName);

                Destroy(droppedItem);
            }
        }
    }
}