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

    void Start()
    {       

        StartCoroutine(BounceThenMove(gameObject));
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
}