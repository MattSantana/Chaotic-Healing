using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using MoreMountains.Tools;

[System.Serializable]
public class Recipes
{
    [Space(5)]
    [Header("����� RECIPES.")]
    [Tooltip("Itens necess�rios para uma receita espec�fica.")]
    public string potionName;
    public string[] requiredItems;
    public GameObject[] requiredItemsIcons;
    public Sprite potionSprite;
}

public class PotionCrafting : MonoBehaviour
{
    [Space(5)]
    [Header("����� RECIPE COMPONENTS.")]
    [Tooltip("Lista de todas as receitas poss�veis.")]
    public Recipes[] recipes;
    [Tooltip("Slots de cria��o para colocar os itens.")]
    [SerializeField] private GameObject[] craftingSlots;

    [Space(5)]
    [Header("����� RECIPE PROGRESS COMPONENTS.")]
    [Tooltip("Imagem de progresso para mostrar o avan�o da cria��o da po��o.")]
    [SerializeField] private MMProgressBar potionProgressBar;
    [SerializeField] private float minValueProgressBar;
    [SerializeField] private float maxValueProgressBar;
    [Tooltip("Texto de ajuda para mostrar mensagens ao jogador.")]
    [SerializeField] private TextMeshProUGUI helpText;
    [Tooltip("Velocidade de preenchimento da barra de progresso.")]
    [SerializeField] private float fillSpeed;

    [Space(5)]
    [Header("����� RECIPE TIPS COMPONENTS.")]
    [SerializeField] private GameObject tipsCreating;
    [SerializeField] private GameObject tipsCrafting;

    private bool canCraft = false; // Indica se a cria��o da po��o pode come�ar.
    private float currentFillAmount = 0f; // Quantidade atual de preenchimento da barra de progresso.
    private Recipes currentRecipe; // Receita atualmente selecionada

    public bool canCreate = false;
    public bool potionWasSelected = false;
    public bool finishPotion = false;

    private Cauldron cauldron; // Refer�ncia ao caldeir�o para criar po��es.
    private DisplayTextForSeconds displayTextController; // Controlador para exibir textos por alguns segundos.
    private CauldronInventory cauldronInventory; // Invent�rio do caldeir�o para gerenciar os itens.
    private PatientManager patientManager; // PatientManager.
    private Movement playerMovement; // Refer�ncia ao script de movimento

    private void Awake()
    {
        cauldron = FindObjectOfType<Cauldron>();
        displayTextController = FindObjectOfType<DisplayTextForSeconds>();
        cauldronInventory = FindObjectOfType<CauldronInventory>();
        patientManager = FindObjectOfType<PatientManager>();
        playerMovement = FindObjectOfType<Movement>();

        tipsCreating.SetActive(false);
        tipsCrafting.SetActive(false);
    }

    private void Start()
    {
        potionProgressBar.gameObject.SetActive(false);
    }

    #region ����� PROGRESS BAR (CRAFTING THE POTION).

    // Atualiza o progresso da cria��o da po��o
    private void UpdateProgress()
    {
        if (potionProgressBar.BarTarget < 1)
        {
            currentFillAmount = Mathf.Max(currentFillAmount + fillSpeed, 0);

            potionProgressBar.UpdateBar(currentFillAmount, minValueProgressBar, maxValueProgressBar);
        }

        else
        {
            CompleteCrafting(); // Completa a cria��o da po��o quando a barra est� cheia.
        }
    }

    // Finaliza o processo de cria��o da po��o
    private void CompleteCrafting()
    {
        finishPotion = true;

        tipsCreating.SetActive(false);
        tipsCrafting.SetActive(false);

        displayTextController.StartDisplayText(); // Mensagem de feedback para o jogador.
        helpText.text = "A receita est� pronta!";
        canCraft = false; // Reseta a capacidade de criar
        playerMovement.canMove = true; // Permite que o jogador volte a se mover

        DestroyCraftingItems(); // Destroi os itens nos slots de cria��o.
        DestroyUsedItemsInInventory(); // Destroi os itens usados do invent�rio do jogador invent�rio.
        cauldron.DropPotion(); // Cria a po��o no caldeir�o.
        patientManager.ChangeSlotRaycast(); // Ativa o raycast para o paciente receber a po��o.

        ResetProgress(); // Reseta o progresso da barra.
    }

    // Finaliza o processo de cria��o da po��o
    public void FailCrafting()
    {
        finishPotion = true;

        tipsCreating.SetActive(false);
        tipsCrafting.SetActive(false);

        canCraft = false; // Reseta a capacidade de criar
        playerMovement.canMove = true; // Permite que o jogador volte a se mover

        displayTextController.StartDisplayText(); // Mensagem de feedback para o jogador.
        helpText.text = "Falhou! Seu paciente morreu.";

        DestroyCraftingItems(); // Destroi os itens nos slots de cria��o.
        DestroyUsedItemsInInventory(); // Destroi os itens usados do invent�rio do jogador invent�rio.

        ResetProgress(); // Reseta o progresso da barra.
    }

    // Reseta a barra de progresso
    private void ResetProgress()
    {
        currentFillAmount = 0f;

        potionProgressBar.BarProgress = currentFillAmount;
        potionProgressBar.BarTarget = currentFillAmount;
        potionProgressBar.DelayedBarDecreasingProgress = currentFillAmount;
        potionProgressBar.DelayedBarIncreasingProgress = currentFillAmount;

        potionProgressBar.InitialFillValue = currentFillAmount;
        potionProgressBar.gameObject.SetActive(false); // Desativa a barra de progresso.
    }

    #endregion

    #region ����� ITEM MANAGEMENT.

    // Destroi os itens nos slots de cria��o
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

    // Fun��o para destruir os itens nos craftingSlots
    public void DestroyItemsInCraftingSlots()
    {
        foreach (var slot in craftingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                // Destruir o item no slot
                Destroy(slot.transform.GetChild(0).gameObject);
            }
        }
    }

    // Destroi os itens usados no invent�rio
    private void DestroyUsedItemsInInventory()
    {
        string[] usedItemNames = GetItemNamesFromSlots(); // Pega os nomes dos itens nos slots de cria��o
        FindObjectOfType<Inventory>().DestroyUsedItems(usedItemNames); // Chama o m�todo para destruir os itens no invent�rio
    }

    // Pega os nomes dos itens nos slots de cria��o.
    private string[] GetItemNamesFromSlots()
    {
        // Cria um array de strings com o tamanho igual ao n�mero de slots de cria��o.
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

    // Retorna os itens ao invent�rio do caldeir�o
    private void ReturnItemsToCauldronInventory()
    {
        foreach (var slot in craftingSlots)
        {
            if (slot.transform.childCount > 0)
            {
                Transform itemTransform = slot.transform.GetChild(0);
                Image itemImage = itemTransform.GetComponent<Image>();
                string itemName = itemTransform.name;

                cauldronInventory.AddItemToCauldronInventory(itemImage.sprite, itemName); // Adiciona o item de volta ao invent�rio do caldeir�o
                Destroy(itemTransform.gameObject); // Remove o item do slot de cria��o
            }
        }
    }

    #endregion

    #region ����� CRAFTING PROCESS.

    // Atualiza o progresso da cria��o da po��o se a cria��o for permitida.
    public void Crafting()
    {
        if (canCraft)
        {
            tipsCreating.SetActive(false);
            tipsCrafting.SetActive(true);

            UpdateProgress();
        }
    }

    // Inicia o processo de cria��o da po��o.
    public void CreatePotion()
    {
        string[] itemNames = GetItemNamesFromSlots(); // Pega os nomes dos itens nos slots de cria��o.

        if (!canCraft && canCreate && !finishPotion)
        {
            if (HasEmptySlots(itemNames))
            {
                ShowHelpText("Itens insuficientes.");
                ReturnItemsToCauldronInventory(); // Retorna os itens ao invent�rio do caldeir�o se um ou mais dos slots estiverem vazios.
                return;
            }

            if (ValidateRecipe(itemNames))
            {
                ShowHelpText("Ingredientes prontos! Pode come�ar a criar sua receita.");

                canCraft = true;

                playerMovement.canMove = false;

                potionProgressBar.gameObject.SetActive(true); // Ativa a barra de progresso
                SetRaycastTarget(false); // Define o alvo do raycast como falso para impedir a retirada dos itens.
            }

            else
            {
                ShowHelpText("Essa n�o � a receita correta!");
                ReturnItemsToCauldronInventory(); // Retorna os itens ao invent�rio do caldeir�o se a receita n�o for v�lida.
            }
        }
    }

    // Verifica se h� slots vazios
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

    // Valida se os itens nos slots de cria��o correspondem a alguma receita
    private bool ValidateRecipe(string[] itemNames)
    {
        HashSet<string> itemSet = new HashSet<string>(itemNames);

        if (currentRecipe.requiredItems.Length == itemNames.Length)
        {
            HashSet<string> requiredItemSet = new HashSet<string>(currentRecipe.requiredItems);
            return itemSet.SetEquals(requiredItemSet);
        }

        return false;
    }

    #endregion

    #region ����� UTILITY METHODS.

    private bool IsPatientAlive()
    {
        PatientHealth patientHealth = FindObjectOfType<PatientHealth>();

        if (patientHealth != null)
        {
            return patientHealth.isPatientAlive;
        }

        else
        {
            Debug.Log("Paciente n�o encontrado");
            return patientHealth.isPatientAlive;
        }
    }

    public string GetCurrentPotionName()
    {
        if (currentRecipe != null)
        {
            return currentRecipe.potionName;
        }
        else
        {
            Debug.LogWarning("Receita atual n�o definida.");
            return "";
        }
    }


    // Define o alvo do raycast para os itens nos slots de cria��o
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

    #region ����� RANDOM RECIPE.

    public Recipes GetRandomRecipe()
    {
        if (recipes.Length == 0)
        {
            Debug.LogError("N�o h� receitas dispon�veis.");
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
            Debug.Log("Nova receita aleat�ria selecionada:");
            Debug.Log("Receita: " + GetRecipeAsString(currentRecipe));

            finishPotion = false;

            UpdatePatientSprite(currentRecipe.potionSprite);

            tipsCreating.SetActive(true);
            tipsCrafting.SetActive(false);
        }

        else
        {
            Debug.LogWarning("N�o foi poss�vel encontrar uma nova receita aleat�ria.");
        }
    }

    public bool IsCurrentRecipeCorrect()
    {
        Recipes patientRecipe = patientManager.GetCurrentPatientRecipe(); // Obt�m a receita que o paciente precisa
        return currentRecipe == patientRecipe; // Verifica se a receita atual � igual � receita do paciente
    }

    private string GetRecipeAsString(Recipes recipe)
    {
        string recipeString = "Itens necess�rios: ";
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

    public Recipes GetRecipeByName(string itemName)
    {
        foreach (Recipes recipe in recipes)
        {
            foreach (string item in recipe.requiredItems)
            {
                if (item == itemName)
                {
                    return recipe;
                }
            }
        }

        return null; // Retorna null se n�o encontrar a receita com o nome do item
    }

    // Retorna a receita aleat�ria atual
    public Recipes GetCurrentRecipe()
    {
        return currentRecipe;
    }

    #endregion

    #region ����� GET POTION SPRITE.

    public Sprite GetCurrentPotionSprite()
    {
        if (currentRecipe != null)
        {
            return currentRecipe.potionSprite;
        }

        else
        {
            Debug.LogWarning("Receita atual n�o definida.");
            return null;
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
                Debug.LogWarning("Componente PatientHealth n�o encontrado no paciente.");
            }
        }

        else
        {
            Debug.LogWarning("Paciente n�o encontrado com a tag 'Patient'.");
        }
    }

    public void UpdatePotionSprite(Sprite newSprite)
    {
        GameObject cauldronObject = GameObject.FindGameObjectWithTag("Potion");

        if (cauldronObject != null)
        {
            Cauldron cauldron = cauldronObject.GetComponent<Cauldron>();

            if (cauldron != null)
            {
                cauldron.UpdatePotionSprite(newSprite);
            }

            else
            {
                Debug.LogWarning("Componente Cauldron n�o encontrado no paciente.");
            }
        }

        else
        {
            Debug.LogWarning("Po��o n�o encontrado com a tag 'Potion'.");
        }
    }

    #endregion

    #region ����� ENEMY DROP RECIPE ITEMS.

    private HashSet<string> droppedItems = new HashSet<string>(); // HashSet para armazenar itens j� dropados

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
                        Debug.LogWarning("Prefab n�o encontrado para o item: " + itemName);
                        return null;
                    }
                }
            }
        }

        Debug.LogWarning("Prefab n�o encontrado para o item: " + itemName);
        return null;
    }

    #endregion
}