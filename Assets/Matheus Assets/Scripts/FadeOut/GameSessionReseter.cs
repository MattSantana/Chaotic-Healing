using UnityEngine;

public class GameSessionReseter : MonoBehaviour
{
    [SerializeField] private  GameObject timerText;
    public void AutoDesativate()
    {
        //Resetando a gameplay session
        ResetGameplayEvent();
        timerText.SetActive(true);
        gameObject.SetActive(false);
    }

    private static void ResetGameplayEvent()
    {
        //reset gameplay event
        CountDownTime.onGameSessionFinished.Invoke();
    }
}
