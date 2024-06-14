using System.Collections;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [Space(5)]
    [Header("——— DROPPED ITEM COMPONENTS.")]
    [SerializeField] private float bounceHeight = 1.0f;
    [SerializeField] private float bounceDuration = 1.0f;
    [SerializeField] private float moveDuration = 1.0f;
    [SerializeField] private float initialMoveSpeed = 2.0f;

    [Space(5)]
    [Header("——— COLLIDER COMPONENT.")]
    [SerializeField] private float colliderActivationDelay = 1.0f;
    private CircleCollider2D circleCollider;

    [Space(5)]
    [Header("——— MOVE TO PLAYER COMPONENT.")]
    [SerializeField] private float attractionRadius = 3.0f;
    [SerializeField] private float moveSpeed = 2.0f;

    [SerializeField] private GameObject itemTip;

    private bool canBeCollected = false;

    private Transform playerTransform; // Referência ao transform do jogador
    private bool isAttracted; // Indica se o item está sendo atraído pelo jogador

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        circleCollider.enabled = false;

        itemTip.SetActive(false);

        StartCoroutine(BounceThenMove(gameObject));
        StartCoroutine(ActivateColliderDelayed());
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer < attractionRadius)
        {
            if (!itemTip.activeSelf)
            {
                itemTip.SetActive(true);
            }
        }

        else
        {
            if (itemTip.activeSelf)
            {
                itemTip.SetActive(false);
            }
        }
    }

    IEnumerator BounceThenMove(GameObject item)
    {
        Vector2 originalPosition = item.transform.position;

        float elapsedTime = 0;
        while (elapsedTime < bounceDuration)
        {
            float normalizedTime = elapsedTime / bounceDuration;
            float bounceHeightNow = bounceHeight * Mathf.Sin(normalizedTime * Mathf.PI);

            float yPos = originalPosition.y + bounceHeightNow;

            item.transform.position = new Vector2(originalPosition.x, yPos);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        int direction = Random.Range(0, 2) == 0 ? -1 : 1;

        elapsedTime = 0;
        while (elapsedTime < moveDuration)
        {
            float normalizedTime = elapsedTime / moveDuration;
            float moveSpeed = initialMoveSpeed * (1 - normalizedTime);

            item.transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield break;
    }

    IEnumerator ActivateColliderDelayed()
    {
        yield return new WaitForSeconds(colliderActivationDelay);

        circleCollider.enabled = true;
        canBeCollected = true;
    }

    public bool CanBeCollected()
    {
        return canBeCollected;
    }

    public void MoveToThePlayer()
    {
        StartCoroutine(MoveTowardsPlayer());
    }

    private IEnumerator MoveTowardsPlayer()
    {
        while (Vector2.Distance(transform.position, playerTransform.position) > 0.1f)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}