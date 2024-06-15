using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class PatientHealth : MonoBehaviour
{
    [Space(5)]
    [Header("————— PATIENT HEALTH VARIABLES.")]
    [SerializeField] private float maxHealth = 100f;
    public float currentHealth;
    [SerializeField] private float destroyDelay = 2f;

    [Space(5)]
    [Header("————— PARTICLES VARIABLES.")]
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private ParticleSystem aliveParticle;

    [Space(5)]
    [Header("————— HEALTH DECAY VARIABLES.")]
    [SerializeField] private float healthDecayRate; // Taxa de decaimento da vida por segundo.
    [SerializeField] private float decayInterval = 5f;
    private float decayTimer = 0f;

    [Space(5)]
    [Header("————— HEALTH BAR VARIABLES.\n - Não coloque nada em 'patientHealthBar'.")]
    [SerializeField] private MMProgressBar patientHealthBar;
    [SerializeField] private float minValueProgressBar;
    [SerializeField] private float maxValueProgressBar;
    private GameObject healthBarObject;

    [Space(5)]
    [Header("————— THOUGHT SPRITE VARIABLES.")]
    [SerializeField] private SpriteRenderer thoughtSpriteRenderer;
    [SerializeField] private GameObject thought;

    [Space(5)]
    [Header("————— AUDIO VARIABLES.")]
    [SerializeField] private AudioClip patientDeathSound; // Som de morte do paciente
    [SerializeField] private AudioClip[] patientCureSounds; // Array de sons de cura do paciente
    private AudioSource audioSource;

    private bool isPatientActive = false;
    [HideInInspector] public bool isPatientAlive = false;

    private PlayerGold playerGold;
    private Inventory inventory;
    private CauldronInventory cauldronInventory;
    private PotionCrafting potionCrafting;
    private PatientManager patientManager;

    private void Awake()
    {
        playerGold = FindAnyObjectByType<PlayerGold>();
        inventory = FindObjectOfType<Inventory>();
        cauldronInventory = FindObjectOfType<CauldronInventory>();
        potionCrafting = FindObjectOfType<PotionCrafting>();
        patientManager = FindObjectOfType<PatientManager>();

        audioSource = GetComponent<AudioSource>(); // Obtém a referência ao componente AudioSource
    }

    private void Start()
    {
        currentHealth = maxHealth;

        healthBarObject = GameObject.Find("Patient Health Bar");

        if (healthBarObject != null)
        {
            patientHealthBar = healthBarObject.GetComponent<MMProgressBar>();
        }
    }

    private void Update()
    {
        Patient();
    }

    private void Patient()
    {
        if (isPatientActive)
        {
            if (patientManager.IsCurrentPotionSameAsSlotItem())
            {
                PatientCured();
                isPatientAlive = true;
                return;
            }

            if (currentHealth <= 0)
            {
                PatientDied();
                isPatientAlive = false;
                return;
            }

            decayTimer -= Time.deltaTime;

            if (decayTimer <= 0)
            {
                currentHealth = Mathf.Max(currentHealth - healthDecayRate, 0);
                UpdateHealthBar();

                decayTimer = decayInterval;
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (patientHealthBar != null)
        {
            patientHealthBar.UpdateBar(currentHealth, minValueProgressBar, maxValueProgressBar);
        }
    }

    public void ActivatePatient()
    {
        isPatientAlive = true;
        isPatientActive = true;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void PatientCured()
    {
        patientManager.DestroySlotImage();

        // Ativar partículas de cura
        if (aliveParticle != null)
        {
            aliveParticle.Play();
        }

        // Tocar som de cura aleatório
        if (patientCureSounds != null && patientCureSounds.Length > 0 && audioSource != null)
        {
            AudioClip randomCureSound = patientCureSounds[Random.Range(0, patientCureSounds.Length)];
            audioSource.PlayOneShot(randomCureSound);
        }

        Debug.Log("O paciente sobreviveu!");
        isPatientActive = false;
        thought.SetActive(false);

        playerGold.WinGold(100);

        StartCoroutine(DestroyPatientAndWait());
    }

    private void PatientDied()
    {
        potionCrafting.FailCrafting();
        patientManager.DestroySlotImage();

        // Ativar partículas de morte
        if (deathParticle != null)
        {
            deathParticle.Play();
        }

        // Tocar som de morte do paciente
        if (patientDeathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(patientDeathSound);
        }

        Debug.Log("O paciente morreu!");
        isPatientActive = false;
        thought.SetActive(false);

        // Destrói todos os itens nos inventários
        inventory.DestroyAllItems();
        cauldronInventory.DestroyAllItems();
        potionCrafting.DestroyItemsInCraftingSlots();

        playerGold.LoseGold(100);

        StartCoroutine(DestroyPatientAndWait());
    }

    private IEnumerator DestroyPatientAndWait()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);

        // Chama o spawn de um novo paciente no PatientManager após um intervalo de tempo
        patientManager.Invoke("SpawnPatient", 2f); // Invoca o método SpawnPatient após 2 segundos
    }

    public void UpdateThoughtSprite(Sprite newSprite)
    {
        if (thoughtSpriteRenderer != null)
        {
            thoughtSpriteRenderer.sprite = newSprite;
        }

        else
        {
            Debug.LogWarning("SpriteRenderer do pensamento não atribuído ao paciente.");
        }
    }
}
