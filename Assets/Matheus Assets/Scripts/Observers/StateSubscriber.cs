using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class StateSubscriber : MonoBehaviour
{
    [SerializeField] private Button skinnyState;
    [SerializeField] private Button strongState;

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
    private void StrongStateSelected()
    {
        Debug.Log("I'm Strong");
        onStrongStateChosen?.Invoke();
    }

    private void SkinnyStateSelected()
    {
        Debug.Log("I'm skinny");
        onSkinnyStateChosen?.Invoke();
    }
}
