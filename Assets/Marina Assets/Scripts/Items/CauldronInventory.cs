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
}