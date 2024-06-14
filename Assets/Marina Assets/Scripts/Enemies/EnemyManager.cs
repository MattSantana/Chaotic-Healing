using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    public bool IsPlayerStunned { get; private set; } = false;
    public float resumeMovementDelay = 1.0f;

    private PlayerNegativeEffects player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerNegativeEffects>();
    }

    public void StunPlayer(float stunDuration)
    {
        StartCoroutine(StunCoroutine(stunDuration));
    }

    private IEnumerator StunCoroutine(float stunDuration)
    {
        IsPlayerStunned = true;
        yield return new WaitForSeconds(stunDuration);
        IsPlayerStunned = false;

        player.StartBlinking(resumeMovementDelay);
        yield return new WaitForSeconds(resumeMovementDelay);
    }
}