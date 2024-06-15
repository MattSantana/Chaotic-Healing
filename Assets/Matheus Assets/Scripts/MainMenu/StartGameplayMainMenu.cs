using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameplayMainMenu : MonoBehaviour
{
    [SerializeField] private Button startGameplay;

    private void Awake() {
        startGameplay.onClick.AddListener(StartTheGame);
    }

    private void StartTheGame()
    {
        SceneManager.LoadScene(1);
    }
}
