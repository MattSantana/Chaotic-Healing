using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerGold : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private Transform floatingTextSpawnPoint;

    public int currentGold;

    private void Start()
    {
        UpdateGoldText();
    }

    public void WinGold(int amount)
    {
        currentGold += amount;
        UpdateGoldText();
        ShowFloatingText($"+{amount}", Color.green);
    }

    public void LoseGold(int amount)
    {
        currentGold -= amount;
        UpdateGoldText();
        ShowFloatingText($"-{amount}", Color.red);
    }

    private void UpdateGoldText()
    {
        goldText.text = currentGold.ToString("F2");
    }

    private void ShowFloatingText(string text, Color color)
    {
        float amountValue = float.Parse(text); // Converte o texto para um valor float
        string formattedText = amountValue.ToString("0.00"); // Formata o valor float com duas casas decimais

        GameObject floatingTextInstance = Instantiate(floatingTextPrefab, floatingTextSpawnPoint.position, Quaternion.identity, floatingTextSpawnPoint);
        FloatingText floatingText = floatingTextInstance.GetComponent<FloatingText>();
        floatingText.ShowText(formattedText, color);
    }
}