using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class StateSubscriber : MonoBehaviour
{
    [SerializeField] private Button skinnyState;
    [SerializeField] private Button strongState;
    [SerializeField] private Button utilityStats;
    [SerializeField] private GameObject roguelikePanel;

    #region //Strong State delegate
    public delegate void OnStrongStateChosen();
    public static event OnStrongStateChosen onStrongStateChosen;
    #endregion

    #region Skinny State delegate
    public delegate void OnSkinnyStateChosen();
    public static event OnStrongStateChosen onSkinnyStateChosen;
    #endregion


    #region stats manager controller 
    private int strongerIndex;
    #endregion

    private void Awake() {
        skinnyState.onClick.AddListener(SkinnyStateSelected);
        strongState.onClick.AddListener(StrongStateSelected);
        utilityStats.onClick.AddListener(UtilityStatsSelected);
    }
    private void Start() {
        
    }
    private void StrongStateSelected()
    {
        //mudo a skin e atributos
        onStrongStateChosen?.Invoke();

        ButtonPresets();
    }

    private void SkinnyStateSelected()
    {
        //mudo a skin e atributos
        onSkinnyStateChosen?.Invoke();

        ButtonPresets();        
    }
    private void UtilityStatsSelected()
    {
        
        //skinnyState.interactable = false;
        ButtonPresets();        
    }


    private void ButtonPresets()
    {
        Time.timeScale = 1;
    }

    private static void ResetGameplayEvent()
    {
        //reset gameplay event
        CountDownTime.onGameSessionFinished.Invoke();
    }

}
