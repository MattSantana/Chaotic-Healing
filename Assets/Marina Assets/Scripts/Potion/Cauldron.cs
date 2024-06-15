using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{
    [Space(5)]
    [Header("��� CAULDRON COMPONENTS.")]
    [SerializeField] private GameObject cauludronRecipeSlots;
    [SerializeField] private GameObject cauldronInventorySlots;
    [SerializeField] private ParticleSystem cauldronSmoke;
    [SerializeField] private AudioSource audioSource; // Refer�ncia ao AudioSource para tocar o som

    [Space(5)]
    [Header("��� ITEM DROP COMPONENTS.")]
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
                Debug.LogWarning("SpriteRenderer n�o encontrado no objeto dropado.");
            }
        }

        else
        {
            Debug.LogWarning("Sprite da po��o n�o encontrado, ou po��o sem nome.");
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
            Debug.LogWarning("SpriteRenderer do pensamento n�o atribu�do ao paciente.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cauludronRecipeSlots.SetActive(true);
            cauldronInventorySlots.SetActive(true);

            if (audioSource != null)
            {
                audioSource.volume = 1f; // Ajusta o volume para o m�ximo ao entrar na �rea
                audioSource.Play(); // Toca o som
            }

            cauldronSmoke.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cauludronRecipeSlots.SetActive(false);
            cauldronInventorySlots.SetActive(false);

            if (audioSource != null)
            {
                StartCoroutine(FadeOutAudio()); // Inicia a rotina para diminuir o volume gradualmente
            }

            cauldronSmoke.Stop();
        }
    }

    private IEnumerator FadeOutAudio()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 1f; // Diminui o volume gradualmente em 1 segundo
            yield return null;
        }

        audioSource.Stop(); // Para o �udio quando o volume chegar a zero
        audioSource.volume = startVolume; // Restaura o volume original para futuras intera��es
    }
}
