using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    //[SerializeField] private float distance = 5.0f;
    [SerializeField] private float blinkInterval = 0.1f;

    [SerializeField] private Image stunUIEffect;

    private bool isStunned = false;
    private float stunEndTime = 0f;

    private PlayerControls playerControls;
    private Vector2 movement;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stunUIEffect.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isStunned)
        {
            if (Time.time >= stunEndTime)
            {
                isStunned = false;
                stunUIEffect.gameObject.SetActive(false);
            }

            else
            {
                return;
            }
        }

        Move();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    #region ————— STUN METHODS.

    public void Stun (float duration)
    {
        isStunned = true;
        stunUIEffect.gameObject.SetActive(true);
        stunEndTime = Time.time + duration;
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
