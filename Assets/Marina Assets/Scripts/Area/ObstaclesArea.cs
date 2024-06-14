using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesArea : MonoBehaviour
{
    [SerializeField] private Transform[] spawnItemsPoints;
    private Dictionary<string, GameObject> spawnedItems = new Dictionary<string, GameObject>(); // Armazena itens spawnados

    private PotionCrafting potionCrafting;

    private void Awake()
    {
        potionCrafting = FindObjectOfType<PotionCrafting>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SpawnAllRecipesItems();
        }
    }

    private void SpawnAllRecipesItems()
    {
        foreach (Recipes recipe in potionCrafting.recipes)
        {
            SpawnRecipeItems(recipe);
        }
    }

    private void SpawnRecipeItems(Recipes recipe)
    {
        foreach (string item in recipe.requiredItems)
        {
            if (!potionCrafting.IsItemAlreadyDropped(item))
            {
                GameObject droppedObject = potionCrafting.GetItemPrefabByName(item);
                if (droppedObject != null)
                {
                    // Escolhe aleatoriamente um ponto de spawn
                    Transform spawnPoint = spawnItemsPoints[Random.Range(0, spawnItemsPoints.Length)];

                    // Instancia o objeto no ponto de spawn escolhido
                    GameObject spawnedItem = Instantiate(droppedObject, spawnPoint.position, Quaternion.identity);
                    spawnedItems[item] = spawnedItem; // Armazena o item spawnado

                    potionCrafting.RegisterItemDrop(item);
                }

                else
                {
                    Debug.LogWarning("Prefab n�o encontrado para o item: " + item);
                }
            }
        }
    }

    // M�todo para respawnar um item espec�fico
    public void RespawnItem(string itemName)
    {
        if (spawnedItems.ContainsKey(itemName))
        {
            GameObject itemPrefab = potionCrafting.GetItemPrefabByName(itemName);
            if (itemPrefab != null)
            {
                // Encontra o ponto de respawn aleat�rio
                Transform spawnPoint = spawnItemsPoints[Random.Range(0, spawnItemsPoints.Length)];

                // Instancia um novo objeto na posi��o do spawn escolhido
                GameObject spawnedItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
                spawnedItems[itemName] = spawnedItem; // Atualiza na lista de itens spawnados
            }
            else
            {
                Debug.LogWarning("Prefab n�o encontrado para o item: " + itemName);
            }
        }

        else
        {
            Debug.LogWarning("Item '" + itemName + "' n�o encontrado na lista de itens spawnados.");
        }
    }

    // M�todo para lidar com o respawn do item perdido no buraco
    public void RespawnLostItem(string itemName)
    {
        RespawnItem(itemName); // Chama o m�todo de respawn gen�rico
    }
}