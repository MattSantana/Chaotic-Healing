using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Space(5)]
    [Header("——— ENEMY HEALTH COMPONENTS.")]
    [SerializeField] private float maxHealth;
    [Header("(Don't put values ​​in 'currentHealth'. Viewing only.)")]
    public float currentHealth;

    [Space(5)]
    [Header("——— ENEMY UI COMPONENTS.")]
    [SerializeField] private Image healthBar;

    [Space(5)]
    [Header("——— ENEMY DAMAGE TAKEN COMPONENTS.")]
    [SerializeField] private float damageTaken;
    [SerializeField] private ParticleSystem damageTakenParticle;
    [SerializeField] private ParticleSystem deathParticle;

    [Space(5)]
    [Header("——— ITEM DROP COMPONENTS.")]
    [SerializeField] private GameObject droppedItem;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // TESTING ENEMY TAKING DAMAGE:
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakingDamage(damageTaken);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Die();
        }
    }

    public void TakingDamage(float amount)
    {
        if (currentHealth <= 0)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        damageTakenParticle.Play();
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth;
        healthBar.fillAmount = healthPercentage;
    }

    private void Die()
    {
        // Start death animation here.

        deathParticle.Play();

        Instantiate(droppedItem, transform.position, Quaternion.identity);

        Destroy(gameObject, 2f); // Change seconds later. It depends on the animation.
    }
}