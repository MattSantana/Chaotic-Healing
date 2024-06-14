using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronInventory : MonoBehaviour
{
    [SerializeField] private GameObject cauldronInventoryPanel;
    [SerializeField] private List<Image> cauldronInventorySlots = new List<Image>();

    [SerializeField] private Image imagePrefab;

    public void AddItemToCauldronInventory(Sprite itemSprite, string itemName)
    {
        for (int i = 0; i < cauldronInventorySlots.Count; i++)
        {
            if (cauldronInventorySlots[i].transform.childCount == 0)
            {
                Image newImage = Instantiate(imagePrefab, cauldronInventorySlots[i].transform);
                newImage.sprite = itemSprite;
                newImage.gameObject.name = itemName;
                newImage.rectTransform.sizeDelta = cauldronInventorySlots[i].rectTransform.sizeDelta;
                return;
            }
        }
    }

    public void DestroyAllItems()
    {
        foreach (var slot in cauldronInventorySlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    public void DestroyItem(string itemName)
    {
        for (int i = 0; i < cauldronInventorySlots.Count; i++)
        {
            Transform slotTransform = cauldronInventorySlots[i].transform;
            if (slotTransform.childCount > 0)
            {
                Image itemImage = slotTransform.GetChild(0).GetComponent<Image>();
                if (itemImage != null && itemImage.gameObject.name == itemName)
                {
                    Destroy(itemImage.gameObject);
                    return;
                }
            }
        }
    }
}