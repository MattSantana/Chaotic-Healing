using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<CollectableItem> items = new List<CollectableItem>();
    [SerializeField] private int maxSlots = 6;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private List<Image> inventorySlotIcons = new List<Image>();
    [SerializeField] private Image imagePrefab;

    private void Start()
    {
        inventoryPanel.SetActive(false);
    }

    public bool AddItem(CollectableItem item)
    {
        if (items.Count < maxSlots)
        {
            items.Add(item);
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

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
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
}