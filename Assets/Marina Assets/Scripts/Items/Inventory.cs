using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;

    public InventoryItem(string name, Sprite icon)
    {
        this.itemName = name;
        this.icon = icon;
    }
}

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    [SerializeField] private int maxSlots = 6;
    [SerializeField] private List<Image> inventorySlotIcons = new List<Image>();
    [SerializeField] private Image imagePrefab;

    public bool AddItem(CollectableItem item)
    {
        if (items.Count < maxSlots)
        {
            InventoryItem newItem = new InventoryItem(item.itemName, item.icon);
            items.Add(newItem);
            AddItemToInventory(item.icon, item.itemName);
            return true;
        }

        else
        {
            Debug.Log("Inventário cheio!");
            return false;
        }
    }

    public void AddItemToInventory(Sprite itemSprite, string itemName)
    {
        for (int i = 0; i < inventorySlotIcons.Count; i++)
        {
            if (inventorySlotIcons[i].transform.childCount == 0)
            {
                Image newImage = Instantiate(imagePrefab, inventorySlotIcons[i].transform);
                newImage.sprite = itemSprite;
                newImage.gameObject.name = itemName;
                newImage.rectTransform.sizeDelta = inventorySlotIcons[i].rectTransform.sizeDelta;
                return;
            }
        }
    }

    public void DestroyItem(string itemName)
    {
        // Procura pelo item com o nome especificado e o destrói
        for (int i = 0; i < inventorySlotIcons.Count; i++)
        {
            Transform slotTransform = inventorySlotIcons[i].transform;
            if (slotTransform.childCount > 0)
            {
                Image itemImage = slotTransform.GetChild(0).GetComponent<Image>();
                if (itemImage != null && itemImage.gameObject.name == itemName)
                {
                    Destroy(itemImage.gameObject);

                    // Remova o item da lista de itens, se necessário:
                    items.RemoveAll(item => item.itemName == itemName);
                    return;
                }
            }
        }
    }

    public void DestroyUsedItems(string[] usedItemNames)
    {
        foreach (string itemName in usedItemNames)
        {
            DestroyItem(itemName);
        }
    }

    public void DestroyAllItems()
    {
        foreach (var slot in inventorySlotIcons)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }

        items.Clear(); // Limpa a lista de itens
    }
}