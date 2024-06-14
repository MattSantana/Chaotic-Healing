using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [Space(5)]
    [Header("——— CAULDRON COMPONENTS.")]
    [SerializeField] private GameObject cauludronRecipeSlots;
    [SerializeField] private GameObject cauldronInventorySlots;
    [SerializeField] private ParticleSystem cauldronSmoke;

    [Space(5)]
    [Header("——— ITEM DROP COMPONENTS.")]
    [SerializeField] private GameObject droppedItem;
    [SerializeField] private SpriteRenderer droppedItemSprite;

    private PotionCrafting potionCrafting;

    private void Awake()
    {
        cauludronRecipeSlots.SetActive(false);
        cauldronInventorySlots.SetActive(false);

        potionCrafting = FindObjectOfType<PotionCrafting>();
    }

    public void DropPotion()
    {
        Sprite potionSprite = potionCrafting.GetCurrentPotionSprite();
        string potionName = potionCrafting.GetCurrentPotionName();

        if (potionSprite != null && !string.IsNullOrEmpty(potionName))
        {
            GameObject droppedItemInstance = Instantiate(droppedItem, transform.position, Quaternion.identity);
            SpriteRenderer spriteRenderer = droppedItemInstance.GetComponent<SpriteRenderer>();
            CollectableItem collectableItem = droppedItemInstance.GetComponent<CollectableItem>();

            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = potionSprite;

                if (collectableItem != null)
                {
                    collectableItem.icon = potionSprite;
                    collectableItem.itemName = potionName;
                    droppedItemInstance.name = potionName;
                }
            }

            else
            {
                Debug.LogWarning("SpriteRenderer não encontrado no objeto dropado.");
            }
        }

        else
        {
            Debug.LogWarning("Sprite da poção não encontrado, ou poção sem nome.");
        }
    }

    public void UpdatePotionSprite(Sprite newSprite)
    {
        if (droppedItemSprite != null)
        {
            droppedItemSprite.sprite = newSprite;
        }

        else
        {
            Debug.LogWarning("SpriteRenderer do pensamento não atribuído ao paciente.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cauludronRecipeSlots.SetActive(true);
            cauldronInventorySlots.SetActive(true);

            cauldronSmoke.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cauludronRecipeSlots.SetActive(false);
            cauldronInventorySlots.SetActive(false);

            cauldronSmoke.Stop();
        }
    }
}
