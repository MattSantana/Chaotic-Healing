using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionTransition : MonoBehaviour
{
    [SerializeField] private GameObject fadeOutObj;
    [SerializeField] private GameObject roguelikePanel;

    public void GameSessionTransitionEffect()
    {
        fadeOutObj.SetActive(true);
        roguelikePanel.SetActive(false);
    }

}
