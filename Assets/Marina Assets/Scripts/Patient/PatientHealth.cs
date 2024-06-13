using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class PatientHealth : MonoBehaviour
{
    [Space(5)]
    [Header("————— PATIENT HEALTH VARIABLES.")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Space(5)]
    [Header("————— HEALTH DECAY VARIABLES.")]
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

    private bool isPatientActive = false;
    private bool cured = false;

    private PlayerGold playerGold;

    private void Awake()
    {
        playerGold = FindAnyObjectByType<PlayerGold>();
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

        if (Input.GetKeyDown(KeyCode.H))
        {
            cured = true;
        }
    }

    private void Patient()
    {
        if (isPatientActive)
        {
            if (cured)
            {
                PatientCured();
                return;
            }

            else
            {
                if (currentHealth <= 0)
                {
                    PatientDied();
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
        isPatientActive = true;
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void PatientCured()
    {
        Debug.Log("O paciente sobreviveu!");
        isPatientActive = false;
        thought.SetActive(false);


        playerGold.WinGold(100);
    }

    private void PatientDied()
    {
        Debug.Log("O paciente morreu!");
        isPatientActive = false;
        thought.SetActive(false);

        playerGold.LoseGold(100);
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