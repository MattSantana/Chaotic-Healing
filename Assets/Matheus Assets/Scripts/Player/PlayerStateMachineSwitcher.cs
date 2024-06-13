using UnityEditor.Animations;
using UnityEngine;

public class PlayerStateMachineSwitcher : MonoBehaviour
{
    private InputReader inputReader;
    
    private Animator myAnimator;
    [SerializeField] private AnimatorController[] myStateMachines;
    [SerializeField] private Atributes[] states;
    public Atributes currentState;
    private void Awake() {
        inputReader = GetComponent<InputReader>();
        myAnimator = GetComponent<Animator>();
        ReturnPlayerStats(states[0]);
    }
    private void Update()
    {
        MovementAnimationsController();
    }

    private void StartState ()
    {
        myAnimator.runtimeAnimatorController = myStateMachines[0];
        ReturnPlayerStats(states[0]);
    }
    private void StrongState ()
    {
        myAnimator.runtimeAnimatorController = myStateMachines[1];
        ReturnPlayerStats(states[1]);
    }
    private void SkinnyState ()
    {
        myAnimator.runtimeAnimatorController = myStateMachines[2];
        ReturnPlayerStats(states[2]);
    }
    private void MovementAnimationsController()
    {
        myAnimator.SetFloat("moveX", inputReader.GetMovement().x);
        myAnimator.SetFloat("moveY",inputReader.GetMovement().y);
        myAnimator.SetFloat("lastMoveX", inputReader.GetLastMoveDirection().x);
        myAnimator.SetFloat("lastMoveY", inputReader.GetLastMoveDirection().y);
        myAnimator.SetFloat("moveMagnitude", inputReader.GetMovement().magnitude);
    }

    public Atributes ReturnPlayerStats(Atributes stats)
    {
        return currentState = stats;
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

[System.Serializable] 
public class Atributes
{
    public string stateName;
    public float attackPower = 25f;
    public float attackSpeed = 1f;
    public float attackRange = 1f;
    public float moveSpeed = 4f;
}
