using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountDownTime : MonoBehaviour
{
    [SerializeField] private float currentTime;
    [SerializeField] private float startingTime = 120f;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject timesUpObj;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameWon;
    [SerializeField] private AudioSource backGroundMusic;
    private PlayerGold playerGold;
    private GameObject player;

    private GameStateHandler gameStateHandler;
    private bool wasTheSessionCounted = false;
    private bool wasTheTimeUpCasted = false;
    public int gameSections;

    [SerializeField]private float gameSessTransitionTime;
    private float startGameSessTransitionTime;

    public delegate void OnGameSessionFinished();
    public static OnGameSessionFinished onGameSessionFinished;

    private int goldMeta = 2000;

    public float delayGameFinish;
    public float startDelayGameFinish;

    void Start()
    {
        startGameSessTransitionTime = gameSessTransitionTime;   
        startDelayGameFinish = delayGameFinish;
        currentTime = startingTime;
        gameStateHandler = FindObjectOfType<GameStateHandler>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerGold = FindObjectOfType<PlayerGold>();
        UpdateCountDownText();

        gameSections = gameStateHandler.GetGameSections();
    }

    private void Update()
    {
        CountDown();
        UpdateCountDownText();

        if (currentTime <= 0)
        {
            //se o tempo acabar
            if( gameStateHandler.currentGameSection >= gameSections ){
                FinishingEnteireGameplay();
            }
            else{
                FinishingGameSession();
            }
            
        }
        else
        {
            //enquanto ainda tiver tempo
        }
    }

    private void FinishingEnteireGameplay()
    {
        
    }

    private void FinishingGameSession()
    {
        if (!wasTheSessionCounted)
        {
            gameStateHandler.currentGameSection++;
            wasTheSessionCounted = true;
        }
        else{
            if(playerGold.currentGold >  goldMeta)
            {
                gameWon.SetActive(true);

                delayGameFinish-=Time.deltaTime;

                if(delayGameFinish < 0)
                {
                    SceneManager.LoadScene(0);
                }

            }
            else{
                gameOver.SetActive(true);

                if(delayGameFinish < 0)
                {
                    SceneManager.LoadScene(0);
                }
            }
        }

        StartingNewGameSession();
        //backGroundMusic.Stop();
    }

    private void StartingNewGameSession()
    {
        // método responsável por realizar o delay antes da inicialização do reset das variáveis do game
        gameSessTransitionTime -= Time.deltaTime;

        if (gameSessTransitionTime <= 0)
        {
            if(!wasTheTimeUpCasted)
            {   
                timesUpObj.SetActive(true);
                wasTheTimeUpCasted = true;
            }
        }
    }

    private void ResetingGameSessionStats()
    {
        //lugar para resetar as variáveis geradas durante UMA sessão de gameplay. Caso queira, É possível chamar isso após um evento.
        
        currentTime = startingTime;
        wasTheSessionCounted = false;
        gameSessTransitionTime = startGameSessTransitionTime;
        wasTheTimeUpCasted = false;
    }

    public void CountDown()
    {
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Clamp(currentTime, 0, startingTime);
    }

    void UpdateCountDownText()
    {
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        countDownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnEnable() {
        onGameSessionFinished+=ResetingGameSessionStats;
    }

    private void OnDisable() {
        onGameSessionFinished-=ResetingGameSessionStats;
    }
}
