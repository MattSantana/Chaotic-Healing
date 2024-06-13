using UnityEngine;

public class EventReciever : MonoBehaviour
{
    private GameStateHandler gameStateHandler;

    private void Start() {
        gameStateHandler = FindObjectOfType<GameStateHandler>();
    }
    public void AutoDisable()
    {
        gameObject.SetActive(false);
        gameStateHandler.isIntroActive = false;
    }

    public void IntroActive()
    {
        gameStateHandler.isIntroActive = true;
        Time.timeScale = 0;
    }
}
