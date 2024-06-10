using UnityEditor.Animations;
using UnityEngine;

public class PlayerStateMachineSwitcher : MonoBehaviour
{
    private InputReader inputReader;
    private Animator myAnimator;
    [SerializeField] private AnimatorController[] myStateMachines;
    private void Awake() {
        inputReader = GetComponent<InputReader>();
        myAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        MovementAnimationsController();
    }

    private void StartState ()
    {
        myAnimator.runtimeAnimatorController = myStateMachines[0];
    }
    private void StrongState ()
    {
        myAnimator.runtimeAnimatorController = myStateMachines[1];
    }
    private void SkinnyState ()
    {
        myAnimator.runtimeAnimatorController = myStateMachines[2];
    }
    private void MovementAnimationsController()
    {
        myAnimator.SetFloat("moveX", inputReader.GetMovement().x);
        myAnimator.SetFloat("moveY",inputReader.GetMovement().y);
        myAnimator.SetFloat("lastMoveX", inputReader.GetLastMoveDirection().x);
        myAnimator.SetFloat("lastMoveY", inputReader.GetLastMoveDirection().y);
        myAnimator.SetFloat("moveMagnitude", inputReader.GetMovement().magnitude);
    }

    private void OnEnable() {
        StateSubscriber.onSkinnyStateChosen+=SkinnyState;
        StateSubscriber.onStrongStateChosen+=StrongState;
    }

    private void OnDisable() {
        StateSubscriber.onSkinnyStateChosen-=SkinnyState;
        StateSubscriber.onStrongStateChosen-=StrongState;
    }
}
