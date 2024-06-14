using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    private PotionCrafting potionCrafting;
    private Recipes currentPotionRecipe;
    [HideInInspector] public List<string> remainingIngredients;

    [SerializeField] private GameObject[] itemPrefabs; // Array de prefabs de itens
    private Dictionary<string, GameObject> itemDictionary; // Dicion�rio de itens

    private void Start()
    {
        potionCrafting = FindObjectOfType<PotionCrafting>();

        // Inicializar o dicion�rio de itens
        itemDictionary = new Dictionary<string, GameObject>();
        foreach (var itemPrefab in itemPrefabs)
        {
            itemDictionary[itemPrefab.name] = itemPrefab;
        }
    }

    public void StartNewRound()
    {
        // Sortear uma nova po��o
        int randomIndex = Random.Range(0, potionCrafting.recipes.Length);
        currentPotionRecipe = potionCrafting.recipes[randomIndex];
        remainingIngredients = new List<string>(currentPotionRecipe.requiredItems);
        Debug.Log("Nova po��o sorteada: " + currentPotionRecipe);
    }

    public GameObject GetNextIngredient()
    {
        if (remainingIngredients.Count == 0) return null;

        int randomIndex = Random.Range(0, remainingIngredients.Count);
        string ingredientName = remainingIngredients[randomIndex];
        remainingIngredients.RemoveAt(randomIndex);

        // Encontrar o GameObject correspondente ao nome do ingrediente
        GameObject ingredient = FindItemByName(ingredientName);

        return ingredient;
    }

    private GameObject FindItemByName(string itemName)
    {
        if (itemDictionary.ContainsKey(itemName))
        {
            return itemDictionary[itemName];
        }

        else
        {
            Debug.LogWarning("Item n�o encontrado no dicion�rio: " + itemName);
            return null;
        }
    }
}
