using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private float collectRange = 2f;
    [SerializeField] private float collectDelay = 0.1f;

    private Inventory inventory;
    private CauldronInventory cauldronInventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        cauldronInventory = FindObjectOfType<CauldronInventory>();
    }

    public void CollectOrMoveClosestItem()
    {
        CollectableItem closestItem = null;
        float closestDistance = collectRange;

        // Encontra todos os itens colecionáveis na cena
        CollectableItem[] items = FindObjectsOfType<CollectableItem>();

        foreach (var item in items)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);
            if (distance <= collectRange && distance < closestDistance)
            {
                // Verifica se o item pode ser coletado
                DroppedItem droppedItem = item.GetComponent<DroppedItem>();
                if (droppedItem != null && droppedItem.CanBeCollected())
                {
                    closestItem = item;
                    closestDistance = distance;
                }
            }
        }

        // Se encontrar um item dentro do raio de coleta, move ele em direção ao player e depois coleta
        if (closestItem != null)
        {
            StartCoroutine(MoveAndCollectItem(closestItem));
        }
    }

    private IEnumerator MoveAndCollectItem(CollectableItem item)
    {
        item.GetComponent<DroppedItem>().MoveToThePlayer();
        bool collected = inventory.AddItem(item);

        // Espera um pouco antes de coletar o item
        yield return new WaitForSeconds(collectDelay);

        if (collected)
        {
            cauldronInventory.AddItemToCauldronInventory(item.icon, item.itemName);
            Destroy(item.gameObject);
        }
    }
}