using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class EnemyHealth : MonoBehaviour
{
    [Space(5)]
    [Header("————— ENEMY HEALTH VARIABLES.")]
    [SerializeField] private float maxHealth;
    public float currentHealth;

    [Space(5)]
    [Header("————— HEALTH BAR VARIABLES.")]
    [SerializeField] private MMProgressBar enemyHealthBar;
    [SerializeField] private float minValueProgressBar;
    [SerializeField] private float maxValueProgressBar;

    [Space(5)]
    [Header("————— TAKING DAMAGE VARIABLES.")]
    [SerializeField] private float damageTaken;
    [SerializeField] private ParticleSystem damageTakenParticle;
    [SerializeField] private ParticleSystem deathParticle;

    [Space(5)]
    [Header("————— ITEM VARIABLES.")]
    [SerializeField] private float itemDropChance = 0.3f; // Chance de dropar um item.

    private GameObject droppedItem;
    private PotionManager potionManager;
    private PotionCrafting potionCrafting; // Referência ao script de criação de poções

    private bool isDead = false; // Flag para verificar se o inimigo já morreu

    private void Awake()
    {
        currentHealth = maxHealth;
        potionManager = FindObjectOfType<PotionManager>();
        potionCrafting = FindObjectOfType<PotionCrafting>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Apenas para testes, você pode remover isso depois
        {
            TakingDamage(damageTaken);
        }

        if (Input.GetKeyDown(KeyCode.O)) // Apenas para testes, você pode remover isso depois
        {
            Die();
        }
    }

    public void TakingDamage(float amount)
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        damageTakenParticle.Play();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        enemyHealthBar.UpdateBar(currentHealth, minValueProgressBar, maxValueProgressBar);
    }

    private void Die()
    {
        if (isDead) return; // Se já estiver morto, não faz nada

        isDead = true;
        deathParticle.Play();

        // Verifica se deve dropar um item
        if (Random.value <= itemDropChance)
        {
            DropItem();
        }

        Destroy(gameObject, 2f); // Tempo de destruição depende da animação de morte
    }

    private void DropItem()
    {
        // Verifica a receita atualmente selecionada para dropar os itens corretos
        Recipes currentRecipe = potionCrafting.GetCurrentRecipe(); // Obtém a receita atualmente selecionada
        if (currentRecipe != null)
        {
            foreach (string item in currentRecipe.requiredItems)
            {
                // Verifica se o item já foi dropado por outro inimigo
                if (!potionCrafting.IsItemAlreadyDropped(item))
                {
                    // Instancia o item como drop
                    GameObject droppedObject = potionCrafting.GetItemPrefabByName(item);
                    if (droppedObject != null)
                    {
                        Instantiate(droppedObject, transform.position, Quaternion.identity);

                        // Registra que este item foi dropado para que não seja repetido por outro inimigo
                        potionCrafting.RegisterItemDrop(item);
                        break; // Sai do loop após dropar o primeiro item da receita
                    }
                    else
                    {
                        Debug.LogWarning("Prefab não encontrado para o item: " + item);
                    }
                }
            }
        }
    }

    public void SetDroppedItem(GameObject item)
    {
        droppedItem = item;
    }
}