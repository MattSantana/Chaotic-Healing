using UnityEngine;

public class TimesUp : MonoBehaviour
{
    [SerializeField] private AudioSource finalWhistleAudio;

    [SerializeField] private GameObject rougueLikeController;
    [SerializeField] private GameObject timerText;


    private void OnEnable() {
        timerText.SetActive(false);
    }
    public void GameTimesUp()
    {
        Delay();
    }

    public void FinalWhistle()
    {
        finalWhistleAudio.Play();
    }

    private void Delay()
    {
        rougueLikeController.SetActive(true); 
        
        
        Time.timeScale = 0;
        gameObject.SetActive(false);
    }
}
