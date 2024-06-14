using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float fadeDuration = 1f;

    private TextMeshProUGUI textMesh;
    private Color originalColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void ShowText(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
        originalColor = color;
        StartCoroutine(FadeAndMove());
    }

    private IEnumerator FadeAndMove()
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = transform.localPosition;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            transform.localPosition = originalPosition + Vector3.down * moveSpeed * (elapsedTime / fadeDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}