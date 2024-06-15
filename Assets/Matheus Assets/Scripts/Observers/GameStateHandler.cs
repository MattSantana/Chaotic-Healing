using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
    public enum State {
        Intro,
        Gameplay, 
        End,
    }

    [SerializeField] private State currentState;	
    [SerializeField] private int maxGameSections;
    public int currentGameSection;
    [HideInInspector] public bool isIntroActive = false;
    [HideInInspector] public bool isGameEnded = false;
    [SerializeField] private GameObject tutorialPanel;

    private void Start() {

    }

    private void Update()
    {
        TutorialPanelController();

        if (maxGameSections == currentGameSection)
        {
            isGameEnded = true;
        }

        if (isGameEnded)
        {
            currentState = State.End;
        }
    }

    private void TutorialPanelController()
    {
        // apenas trocar os inputs para o new input system

        if (Input.GetKeyDown(KeyCode.H) && !isIntroActive && currentState == State.Gameplay)
        {
            tutorialPanel?.SetActive(true);
            tutorialPanel?.GetComponent<Animator>().CrossFadeInFixedTime("TutorialRising", 0.3f);
            currentState = State.Intro;
        }

        if (Input.GetKeyDown(KeyCode.H) && isIntroActive && currentState == State.Intro)
        {
            Time.timeScale = 1;
            tutorialPanel?.GetComponent<Animator>().CrossFadeInFixedTime("TutorialClosing", 0.3f);
            currentState = State.Gameplay;
        }
    }

    public int GetGameSections()
    {
        return maxGameSections;
    }
}
