using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class StateSubscriber : MonoBehaviour
{
    [SerializeField] private Button skinnyState;
    [SerializeField] private Button strongState;
    [SerializeField] private Button utilityStats;
    [SerializeField] private GameObject roguelikePanel;


    #region Start State delegate
    public delegate void OnStartStateChosen();
    public static event OnStartStateChosen onStartStateChosen;
    #endregion

    #region //Strong State delegate
    public delegate void OnStrongStateChosen();
    public static event OnStrongStateChosen onStrongStateChosen;
    #endregion

    #region Skinny State delegate
    public delegate void OnSkinnyStateChosen();
    public static event OnSkinnyStateChosen onSkinnyStateChosen;
    #endregion


    #region stats manager controller 
    private int strongerStateIndex = 0;
    private int skinnyStateIndex = 0;
    private int utilityStatsIndex = 0;
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
        strongerStateIndex++;
        StateChangerFilter();
        ButtonPresets();
        InterfaceInteractions.onCardActivated("Stronger");
    }

    private void SkinnyStateSelected()
    {
        skinnyStateIndex++;
        StateChangerFilter();
        ButtonPresets();  
        InterfaceInteractions.onCardActivated("Faster");      
    }
    private void UtilityStatsSelected()
    {
        utilityStatsIndex++;
        ButtonPresets();     
        InterfaceInteractions.onCardActivated("Utility");   
    }

    private void StateChangerFilter()
    {
        //mudança visual apenas 
        
        if( strongerStateIndex > skinnyStateIndex && strongerStateIndex >= utilityStatsIndex)
        {
            // estado gordinho 

            //mudo a skin e atributos
            onStrongStateChosen?.Invoke();
        }
        else if( strongerStateIndex == skinnyStateIndex && strongerStateIndex >= utilityStatsIndex)
        {
            //estado de corpo normal 

            onStartStateChosen?.Invoke();
        }
        else if( skinnyStateIndex == strongerStateIndex && skinnyStateIndex >= utilityStatsIndex)
        {
            //estado de corpo normal

            onStartStateChosen?.Invoke();
        }
        else if( skinnyStateIndex > strongerStateIndex && skinnyStateIndex >= utilityStatsIndex )
        {
            // estado de corpo magrinho(faster)

            //mudo a skin e atributos
            onSkinnyStateChosen?.Invoke();
        }

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
