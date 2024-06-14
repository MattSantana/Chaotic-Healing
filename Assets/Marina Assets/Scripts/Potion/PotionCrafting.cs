using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

[System.Serializable]
public class Recipes
{
    [Space(5)]
    [Header("————— RECIPES.")]
    [Tooltip("Itens necessários para uma receita específica.")]
    public string[] requiredItems;
    public GameObject[] requiredItemsIcons;
    public Sprite potionSprite;
}

public class PotionCrafting : MonoBehaviour
{
    [Space(5)]
    [Header("————— RECIPE COMPONENTS.")]
    [Tooltip("Lista de todas as receitas possíveis.")]
    public Recipes[] recipes;
    [Tooltip("Slots de criação para colocar os itens.")]
    [SerializeField] private GameObject[] craftingSlots;

    [Space(5)]
    [Header("————— RECIPE PROGRESS COMPONENTS.")]
    [Tooltip("Imagem de progresso para mostrar o avanço da criação da poção.")]
    [SerializeField] private Image progressImage;
    [Tooltip("Texto de ajuda para mostrar mensagens ao jogador.")]
    [SerializeField] private TextMeshProUGUI helpText;
    [Tooltip("Velocidade de preenchimento da barra de progresso.")]
    [SerializeField] private float fillSpeed;

    private bool canCraft = false; // Indica se a criação da poção pode começar.
    private float currentFillAmount = 0f; // Quantidade atual de preenchimento da barra de progresso.
    private Recipes currentRecipe; // Receita atualmente selecionada

    private Cauldron cauldron; // Referência ao caldeirão para criar poções.
    private DisplayTextForSeconds displayTextController; // Controlador para exibir textos por alguns segundos.
    private CauldronInventory cauldronInventory; // Inventário do caldeirão para gerenciar os itens.

    private void Awake()
    {
        cauldron = FindObjectOfType<Cauldron>();
        displayTextController = FindObjectOfType<DisplayTextForSeconds>();
        cauldronInventory = FindObjectOfType<CauldronInventory>();

    }

    private void Start()
    {
        progressImage.gameObject.SetActive(false);
    }

    #region ————— PROGRESS BAR (CRAFTING THE POTION).

    // Atualiza o progresso da criação da poção
    private void UpdateProgress()
    {
        if (currentFillAmount < 1f)
        {
            // Incrementa a quantidade de preenchimento com base no tempo e na velocidade de preenchimento.
            currentFillAmount += fillSpeed * Time.deltaTime;
            currentFillAmount = Mathf.Clamp01(currentFillAmount);

            progressImage.fillAmount = currentFillAmount; // Atualiza a imagem de progresso.
        }
        else
        {
            CompleteCrafting(); // Completa a criação da poção quando a barra está cheia.
        }
    }

    // Finaliza o processo de criação da poção
    private void CompleteCrafting()
    {
        displayTextController.StartDisplayText(); // Mensagem de feedback para o jogador.
        helpText.text = "A receita está pronta!";
        canCraft = false; // Reseta a capacidade de criar

        DestroyCraftingItems(); // Destroi os itens nos slots de criação.
        DestroyUsedItemsInInventory(); // Destroi os itens usados do inventário do jogador inventário.
        cauldron.DropPotion(); // Cria a poção no caldeirão.

        ResetProgress(); // Reseta o progresso da barra.
    }

    // Reseta a barra de progresso
    private void ResetProgress()
    {
        currentFillAmount = 0f;
        progressImage.fillAmount = currentFillAmount;
        progressImage.gameObject.SetActive(false); // Desativa a barra de progresso.
    }

    #endregion

    #region ————— ITEM MANAGEMENT.

    // Destroi os itens nos slots de criação
    private void DestroyCraftingItems()
    {
        foreach (var slot in craftingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Destroy(slot.transform.GetChild(0).gameObject); // Destroi o item dentro do slot
            }
        }
    }

    // Destroi os itens usados no inventário
    private void DestroyUsedItemsInInventory()
    {
        string[] usedItemNames = GetItemNamesFromSlots(); // Pega os nomes dos itens nos slots de criação
        FindObjectOfType<Inventory>().DestroyUsedItems(usedItemNames); // Chama o método para destruir os itens no inventário
    }

    // Pega os nomes dos itens nos slots de criação.
    private string[] GetItemNamesFromSlots()
    {
        // Cria um array de strings com o tamanho igual ao número de slots de criação.
        string[] itemNames = new string[craftingSlots.Length];

        for (int i = 0; i < craftingSlots.Length; i++)
        {
            // Verifica se o slot atual tem pelo menos um item (filho) dentro dele.
            if (craftingSlots[i].transform.childCount > 0)
            {
                // Pega o nome do primeiro item (filho) dentro do slot e salva no array itemNames.
                itemNames[i] = craftingSlots[i].transform.GetChild(0).name;
            }
        }

        // Retorna o array com os nomes dos itens.
        return itemNames;
    }

    // Retorna os itens ao inventário do caldeirão
    private void ReturnItemsToCauldronInventory()
    {
        foreach (var slot in craftingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Transform itemTransform = slot.transform.GetChild(0);
                Image itemImage = itemTransform.GetComponent<Image>();
                string itemName = itemTransform.name;

                cauldronInventory.AddItemToCauldronInventory(itemImage.sprite, itemName); // Adiciona o item de volta ao inventário do caldeirão
                Destroy(itemTransform.gameObject); // Remove o item do slot de criação
            }
        }
    }

    #endregion

    #region ————— CRAFTING PROCESS.

    // Atualiza o progresso da criação da poção se a criação for permitida.
    public void Crafting()
    {
        if (canCraft)
        {
            UpdateProgress();
        }
    }

    // Inicia o processo de criação da poção.
    public void CreatePotion()
    {
        string[] itemNames = GetItemNamesFromSlots(); // Pega os nomes dos itens nos slots de criação.

        if (HasEmptySlots(itemNames))
        {
            ShowHelpText("Itens insuficientes.");
            ReturnItemsToCauldronInventory(); // Retorna os itens ao inventário do caldeirão se um ou mais dos slots estiverem vazios.
            return;
        }

        if (ValidateRecipe(itemNames))
        {
            ShowHelpText("Ingredientes prontos! Pode começar a criar sua receita.");
            canCraft = true;
            progressImage.gameObject.SetActive(true); // Ativa a barra de progresso
            SetRaycastTarget(false); // Define o alvo do raycast como falso para impedir a retirada dos itens.
        }
        else
        {
            ShowHelpText("Não há essa poção no livro de receitas.");
            ReturnItemsToCauldronInventory(); // Retorna os itens ao inventário do caldeirão se a receita não for válida.
        }
    }

    // Verifica se há slots vazios
    private bool HasEmptySlots(string[] itemNames)
    {
        foreach (var name in itemNames)
        {
            if (string.IsNullOrEmpty(name))
            {
                return true; // Retorna verdadeiro se houver slots vazios.
            }
        }
        return false;
    }

    // Valida se os itens nos slots de criação correspondem a alguma receita
    private bool ValidateRecipe(string[] itemNames)
    {
        HashSet<string> itemSet = new HashSet<string>(itemNames);

        foreach (var recipe in recipes)
        {
            if (recipe.requiredItems.Length == itemNames.Length)
            {
                HashSet<string> requiredItemSet = new HashSet<string>(recipe.requiredItems);
                if (itemSet.SetEquals(requiredItemSet))
                {
                    return true; // Retorna verdadeiro se os itens nos slots correspondem à receita
                }
            }
        }

        return false;
    }

    #endregion

    #region ————— UTILITY METHODS.

    // Define o alvo do raycast para os itens nos slots de criação
    private void SetRaycastTarget(bool status)
    {
        foreach (var slot in craftingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Image itemImage = slot.transform.GetChild(0).GetComponent<Image>();
                if (itemImage != null)
                {
                    itemImage.raycastTarget = status; // Define o status do raycast
                }
            }
        }
    }

    // Exibe uma mensagem de ajuda para o jogador
    private void ShowHelpText(string message)
    {
        displayTextController.StartDisplayText();
        helpText.text = message;
    }

    #endregion

    #region ————— RANDOM RECIPE.

    public Recipes GetRandomRecipe()
    {
        if (recipes.Length == 0)
        {
            Debug.LogError("Não há receitas disponíveis.");
            return null;
        }

        int randomIndex = Random.Range(0, recipes.Length);
        return recipes[randomIndex];
    }

    public void ChangeRandomRecipe()
    {
        currentRecipe = GetRandomRecipe();
        if (currentRecipe != null)
        {
            Debug.Log("Nova receita aleatória selecionada:");
            Debug.Log("Receita: " + GetRecipeAsString(currentRecipe));

            UpdatePatientSprite(currentRecipe.potionSprite);
        }

        else
        {
            Debug.LogWarning("Não foi possível encontrar uma nova receita aleatória.");
        }
    }

    private void UpdatePatientSprite(Sprite newSprite)
    {
        GameObject patientObject = GameObject.FindGameObjectWithTag("Patient");
        if (patientObject != null)
        {
            PatientHealth patientHealth = patientObject.GetComponent<PatientHealth>();
            if (patientHealth != null)
            {
                patientHealth.UpdateThoughtSprite(newSprite);
            }
            else
            {
                Debug.LogWarning("Componente PatientHealth não encontrado no paciente.");
            }
        }
        else
        {
            Debug.LogWarning("Paciente não encontrado com a tag 'Patient'.");
        }
    }

    private string GetRecipeAsString(Recipes recipe)
    {
        string recipeString = "Itens necessários: ";
        for (int i = 0; i < recipe.requiredItems.Length; i++)
        {
            recipeString += recipe.requiredItems[i];
            if (i < recipe.requiredItems.Length - 1)
            {
                recipeString += ", ";
            }
        }
        return recipeString;
    }

    // Retorna a receita aleatória atual
    public Recipes GetCurrentRecipe()
    {
        return currentRecipe;
    }


    #endregion

    #region ————— ENEMY DROP RECIPE ITEMS.

    private HashSet<string> droppedItems = new HashSet<string>(); // HashSet para armazenar itens já dropados

    public bool IsItemAlreadyDropped(string item)
    {
        return droppedItems.Contains(item);
    }

    public void RegisterItemDrop(string item)
    {
        droppedItems.Add(item);
    }

    // Retorna o prefab do item pelo nome
    public GameObject GetItemPrefabByName(string itemName)
    {
        foreach (var recipe in recipes)
        {
            for (int i = 0; i < recipe.requiredItems.Length; i++)
            {
                if (recipe.requiredItems[i] == itemName)
                {
                    GameObject itemPrefab = recipe.requiredItemsIcons[i];
                    if (itemPrefab != null)
                    {
                        return itemPrefab;
                    }
                    else
                    {
                        Debug.LogWarning("Prefab não encontrado para o item: " + itemName);
                        return null;
                    }
                }
            }
        }

        Debug.LogWarning("Prefab não encontrado para o item: " + itemName);
        return null;
    }

    #endregion
}