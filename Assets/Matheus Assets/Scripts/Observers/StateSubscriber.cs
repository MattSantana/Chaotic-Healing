using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class StateSubscriber : MonoBehaviour
{
    [SerializeField] private Button skinnyState;
    [SerializeField] private Button strongState;
    [SerializeField] private GameObject roguelikePanel;

    #region //Strong State delegate
    public delegate void OnStrongStateChosen();
    public static event OnStrongStateChosen onStrongStateChosen;
    #endregion

    #region Skinny State delegate
    public delegate void OnSkinnyStateChosen();
    public static event OnStrongStateChosen onSkinnyStateChosen;
    #endregion


    private void Awake() {
        skinnyState.onClick.AddListener(SkinnyStateSelected);
        strongState.onClick.AddListener(StrongStateSelected);
    }
    private void Start() {
        
    }
    private void StrongStateSelected()
    {
        onStrongStateChosen?.Invoke();
        //strongState.interactable = false;
        ButtonPresets();
    }

    private void SkinnyStateSelected()
    {
        onSkinnyStateChosen?.Invoke();
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
