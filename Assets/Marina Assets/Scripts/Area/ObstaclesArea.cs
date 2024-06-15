using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesArea : MonoBehaviour
{
    [SerializeField] private Transform[] spawnItemsPoints;
    private Dictionary<string, GameObject> spawnedItems = new Dictionary<string, GameObject>(); // Armazena itens spawnados

    private PotionCrafting potionCrafting;
    private Areas areas;

    private void Awake()
    {
        potionCrafting = FindObjectOfType<PotionCrafting>();
        areas = FindObjectOfType<Areas>();
    }

    private void Update()
    {
        if (areas.inDungeonArea)
        {
            SpawnAllRecipesItems();
        }

        else
        {
            DestroyAllDroppedItems();
        }
    }

    public void DestroyAllDroppedItems()
    {
        GameObject[] droppedItems = GameObject.FindGameObjectsWithTag("DropItem");

        foreach (GameObject item in droppedItems)
        {
            Destroy(item);
        }
    }

    public void SpawnAllRecipesItems()
    {
        foreach (Recipes recipe in potionCrafting.recipes)
        {
            SpawnRecipeItems(recipe);
        }
    }

    private void SpawnRecipeItems(Recipes recipe)
    {
        List<int> usedSpawnIndexes = new List<int>(); // Lista de índices de spawn usados

        foreach (string item in recipe.requiredItems)
        {
            if (!potionCrafting.IsItemAlreadyDropped(item))
            {
                GameObject droppedObject = potionCrafting.GetItemPrefabByName(item);
                if (droppedObject != null)
                {
                    int randomIndex = GetRandomUnusedSpawnIndex(usedSpawnIndexes);
                    if (randomIndex != -1)
                    {
                        Transform spawnPoint = spawnItemsPoints[randomIndex];

                        // Instancia o objeto no ponto de spawn escolhido
                        GameObject spawnedItem = Instantiate(droppedObject, spawnPoint.position, Quaternion.identity);
                        spawnedItems[item] = spawnedItem; // Armazena o item spawnado

                        potionCrafting.RegisterItemDrop(item);

                        // Adiciona o índice usado à lista
                        usedSpawnIndexes.Add(randomIndex);
                    }
                    else
                    {
                        Debug.LogWarning("Todos os pontos de spawn foram usados para spawn de itens.");
                    }
                }
                else
                {
                    Debug.LogWarning("Prefab não encontrado para o item: " + item);
                }
            }
        }
    }

    // Método para obter um índice de spawn não utilizado
    private int GetRandomUnusedSpawnIndex(List<int> usedIndexes)
    {
        // Cria uma lista de índices de spawn disponíveis
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < spawnItemsPoints.Length; i++)
        {
            if (!usedIndexes.Contains(i))
            {
                availableIndexes.Add(i);
            }
        }

        // Retorna um índice aleatório dos índices disponíveis, ou -1 se não houver nenhum disponível
        if (availableIndexes.Count > 0)
        {
            return availableIndexes[Random.Range(0, availableIndexes.Count)];
        }

        else
        {
            return -1;
        }
    }

    // Método para respawnar um item específico
    public void RespawnItem(string itemName)
    {
        if (spawnedItems.ContainsKey(itemName))
        {
            GameObject itemPrefab = potionCrafting.GetItemPrefabByName(itemName);
            if (itemPrefab != null)
            {
                // Encontra o ponto de respawn aleatório
                Transform spawnPoint = spawnItemsPoints[Random.Range(0, spawnItemsPoints.Length)];

                // Instancia um novo objeto na posição do spawn escolhido
                GameObject spawnedItem = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
                spawnedItems[itemName] = spawnedItem; // Atualiza na lista de itens spawnados
            }
            else
            {
                Debug.LogWarning("Prefab não encontrado para o item: " + itemName);
            }
        }

        else
        {
            Debug.LogWarning("Item '" + itemName + "' não encontrado na lista de itens spawnados.");
        }
    }

    // Método para lidar com o respawn do item perdido no buraco
    public void RespawnLostItem(string itemName)
    {
        RespawnItem(itemName); // Chama o método de respawn genérico
    }
}