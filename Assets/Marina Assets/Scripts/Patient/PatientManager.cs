using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;

public class PatientManager : MonoBehaviour
{
    [SerializeField] private GameObject[] patientPrefabs; // Array de prefabs dos pacientes
    [SerializeField] private Transform patientSpawnPoint; // Ponto de spawn do paciente
    [SerializeField] private MMProgressBar patientHealthBar;

    [Tooltip("Slots de criação para colocar os itens.")]
    [SerializeField] private GameObject patientSlot;
    [SerializeField] private Image patientSlotImage;
    [SerializeField] private GameObject patientTips;

    [SerializeField] private GameObject cauldronInventorySlots;

    [Space(5)]
    [Header("————— AUDIO VARIABLES.")]
    [SerializeField] private AudioClip patientCureSound; // Som de cura do paciente
    private AudioSource audioSource;

    public bool canSpawnPatient = true;

    private PotionCrafting potionCrafting;
    private Inventory inventory;

    private void Awake()
    {
        potionCrafting = FindObjectOfType<PotionCrafting>();
        inventory = FindObjectOfType<Inventory>();

        audioSource = GetComponent<AudioSource>(); // Obtém a referência ao componente AudioSource
    }

    private void Start()
    {
        patientTips.SetActive(false);

        SpawnPatient();
    }

    public void SpawnPatient()
    {
        if (canSpawnPatient)
        {
            // Seleciona aleatoriamente um prefab do array
            GameObject selectedPrefab = patientPrefabs[Random.Range(0, patientPrefabs.Length)];

            // Instancia o prefab selecionado no ponto de spawn
            GameObject newPatientObject = Instantiate(selectedPrefab, patientSpawnPoint.position, patientSpawnPoint.rotation);

            // Obtém o componente PatientHealth do objeto instanciado e ativa o paciente
            PatientHealth newPatient = newPatientObject.GetComponent<PatientHealth>();
            if (newPatient != null)
            {
                newPatient.ActivatePatient();
                potionCrafting.ChangeRandomRecipe();
            }

            else
            {
                Debug.LogWarning("O prefab selecionado não possui o componente PatientHealth.");
            }
        }
    }

    public bool IsCurrentPotionSameAsSlotItem()
    {
        string currentPotionName = potionCrafting.GetCurrentPotionName();

        if (patientSlot.transform.childCount > 0)
        {
            string slotItemName = patientSlot.transform.GetChild(0).name;

            if (currentPotionName == slotItemName)
            {
                patientSlotImage.raycastTarget = false;
                patientTips.SetActive(false);

                // Tocar som de cura
                if (patientCureSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(patientCureSound);
                }

                // Remove o item do inventário do jogador
                inventory.DestroyItem(slotItemName);

                return true;
            }
        }

        return false;
    }

    public Recipes GetCurrentPatientRecipe()
    {
        if (patientSlot.transform.childCount > 0)
        {
            string slotItemName = patientSlot.transform.GetChild(0).name;
            return potionCrafting.GetRecipeByName(slotItemName); // Implemente este método para buscar a receita pelo nome
        }

        return null;
    }

    public void ChangeSlotRaycast()
    {
        patientTips.SetActive(true);
        patientSlotImage.raycastTarget = true;
    }

    public void DestroySlotImage()
    {
        if (patientSlot.transform.childCount > 0)
        {
            // Itera sobre todos os filhos do slot
            for (int i = 0; i < patientSlot.transform.childCount; i++)
            {
                // Destrói cada filho
                Destroy(patientSlot.transform.GetChild(i).gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cauldronInventorySlots.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cauldronInventorySlots.SetActive(false);
        }
    }
}