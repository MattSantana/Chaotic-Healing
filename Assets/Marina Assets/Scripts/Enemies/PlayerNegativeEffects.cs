using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNegativeEffects : MonoBehaviour
{
    [SerializeField] private float stunDuration;

    [SerializeField] private float blinkInterval = 0.1f;

    [SerializeField] private Image stunUIEffect;

    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine;

    private Movement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        stunUIEffect.gameObject.SetActive(false);
    }

    #region ————— STUN METHODS.

    public void ApplyStun(float duration)
    {
        stunDuration = duration;
        StartCoroutine(StunCoroutine());
    }

    private IEnumerator StunCoroutine()
    {
        playerMovement.canMove = false;
        stunUIEffect.gameObject.SetActive(true);

        yield return new WaitForSeconds(stunDuration);

        playerMovement.canMove = true;
        stunUIEffect.gameObject.SetActive(false);
    }

    #endregion

    #region ————— BLINK METHODS.

    public void StartBlinking(float blinkDuration)
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(Blink(blinkDuration));
    }

    private IEnumerator Blink(float blinkDuration)
    {
        float endTime = Time.time + blinkDuration;
        while (Time.time < endTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        spriteRenderer.enabled = true;
    }

    #endregion
}
