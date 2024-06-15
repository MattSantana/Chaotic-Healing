using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    public enum TrapType 
    { 
        Spike,
        Hole,
        ArrowTrap,
        Arrow
    }

    [SerializeField] private TrapType trapType;

    [Space(5)]
    [Header("————— TRAP: Spike.")]
    [SerializeField] private float spikeSlowMultiplier = 0.5f;

    [Space(5)]
    [Header("————— TRAP: Hole.")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float disableDuration = 2f;

    [Space(5)]
    [Header("————— TRAP: Arrow.")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 5f;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private float arrowSlowMultiplier = 0.5f;
    [SerializeField] private float arrowSlowTimeEffect = 2f;

    [Space(5)]
    [Header("————— TRAP: Arrow Interval.")]
    [SerializeField] private float fireInterval = 3f;
    private bool isFiring = false;

    private Inventory playerInventory;
    private CauldronInventory cauldronInventory;
    private ObstaclesArea obstaclesArea;

    private void Awake()
    {
        playerInventory = FindAnyObjectByType<Inventory>();
        cauldronInventory = FindAnyObjectByType<CauldronInventory>();
        obstaclesArea = FindAnyObjectByType<ObstaclesArea>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Movement playerMovement = collision.GetComponent<Movement>();

            if (playerMovement != null)
            {
                switch (trapType)
                {
                    case TrapType.Spike:
                        playerMovement.currentSpeed *= spikeSlowMultiplier;
                        break;

                    case TrapType.Hole:
                        StartCoroutine(HandleHoleTrap(collision, playerMovement));
                        break;

                    case TrapType.ArrowTrap:
                        StartCoroutine(FireArrow(playerMovement));
                        break;

                    case TrapType.Arrow:
                        playerMovement.currentSpeed *= arrowSlowMultiplier;
                        StartCoroutine(ArrowEffect(playerMovement));
                        break;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Movement playerMovement = collision.GetComponent<Movement>();
            if (playerMovement != null)
            {
                switch (trapType)
                {
                    case TrapType.Spike:
                        playerMovement.currentSpeed = playerMovement.moveSpeed;
                        break;
                }
            }
        }
    }

    #region ————— HOLE.
    private IEnumerator HandleHoleTrap(Collider2D player, Movement playerMovement)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        PlayerNegativeEffects negativeEffects = player.GetComponent<PlayerNegativeEffects>();

        if (rb != null)
        {
            rb.gravityScale = 1f; // Muda a gravidade do jogador.
        }

        playerMovement.canMove = false; // Desabilita o movimento do jogador.

        // Salva o item que foi perdido
        string lostItemName = LoseRandomItem(playerInventory, cauldronInventory);

        yield return new WaitForSeconds(disableDuration);

        player.transform.position = respawnPoint.position; // Reposicionar o jogador

        if (rb != null)
        {
            rb.gravityScale = 0f; // Restaura a gravidade original do jogador.
        }

        playerMovement.canMove = true; // Habilita o movimento do jogador.

        // Iniciar o efeito de blink
        if (negativeEffects != null)
        {
            negativeEffects.StartBlinking(1f);
        }

        // Se o item perdido é um item que o jogador pode coletar novamente, respawná-lo
        obstaclesArea.RespawnLostItem(lostItemName); // Chama RespawnLostItem de ObstaclesArea
    }

    #endregion

    #region ————— ARROW.

    private IEnumerator Start()
    {
        if (trapType == TrapType.ArrowTrap)
        {
            while (true)
            {
                if (!isFiring)
                {
                    isFiring = true;
                    yield return StartCoroutine(FireArrow());
                    yield return new WaitForSeconds(fireInterval);
                    isFiring = false;
                }
                yield return null;
            }
        }
    }

    private IEnumerator FireArrow()
    {
        while (true)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

            // Mover a flecha para frente
            rb.velocity = -transform.up * arrowSpeed;

            yield return new WaitForSeconds(destroyDelay);

            Destroy(arrow);
        }
    }

    private IEnumerator FireArrow(Movement playerMovement)
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        // Mover a flecha para frente
        rb.velocity = -transform.up * arrowSpeed;

        yield return new WaitForSeconds(destroyDelay);

        Destroy(arrow);
    }

    private IEnumerator ArrowEffect(Movement playerMovement)
    {
        yield return new WaitForSeconds(arrowSlowTimeEffect);

        playerMovement.currentSpeed = playerMovement.moveSpeed;
    }

    #endregion

    #region ————— SPAWN LOST ITEM.

    private string LoseRandomItem(Inventory playerInventory, CauldronInventory cauldronInventory)
    {
        string lostItemName = "";

        if (playerInventory.items.Count > 0)
        {
            int randomIndex = Random.Range(0, playerInventory.items.Count);
            InventoryItem itemToRemove = playerInventory.items[randomIndex];

            playerInventory.DestroyItem(itemToRemove.itemName);
            cauldronInventory.DestroyItem(itemToRemove.itemName);

            lostItemName = itemToRemove.itemName;
        }

        return lostItemName;
    }

    #endregion
}