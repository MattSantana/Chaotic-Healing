using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayTextForSeconds : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private float displayDuration = 3f;
    public void StartDisplayText()
    {
        StartCoroutine(DisplayTextCoroutine());
    }

    private IEnumerator DisplayTextCoroutine()
    {
        yield return new WaitForSeconds(displayDuration);

        displayText.text = "";
    }
}
