using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSkills : MonoBehaviour
{
    private PlayerStateMachineSwitcher stateChecker;
    private InputReader inputReader;
    private Rigidbody2D rb;

    [Header("Dash")]
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration =1f;
    [SerializeField] float dashCooldown =1f;

    [Header("Slam")]
    [SerializeField] float slamSpeed = 10f;
    [SerializeField] float slamDuration =1f;
    [SerializeField] float slamCooldown =1f;
    public bool isDashing;
    public bool isSlaming;
    private void Awake() {
        inputReader = GetComponent<InputReader>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start() {
        stateChecker = GetComponent<PlayerStateMachineSwitcher>();
    }

    private void Update()
    {

    }

    public void SpecialAttack()
    {
        switch (stateChecker.currentState.stateName)
        {
            case "StartState": 
                break;
            case "StrongState":
                StartCoroutine(BodySlam());
                break;
            case "SkinnyState":
                StartCoroutine(Dash());
                break;
        }  
    }
    public void NormalAttack()
    {
        switch (stateChecker.currentState.stateName)
        {
            case "StartState":
                KnifeAttack();
                break;
            case "StrongState":
                Stomp();
                break;
            case "SkinnyState":
                Scratch();
                break;
        } 
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        GetComponent<Animator>().SetTrigger("dash");
        rb.velocity = new Vector2(inputReader.GetMovement().x * dashSpeed, inputReader.GetMovement().y * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }
    private IEnumerator BodySlam()
    {
        isSlaming = true;
        GetComponent<Animator>().SetTrigger("bodySlam");
        rb.velocity = new Vector2(inputReader.GetMovement().x * slamSpeed, inputReader.GetMovement().y * slamSpeed);
        yield return new WaitForSeconds(slamDuration);
        GetComponent<Animator>().ResetTrigger("bodySlam");
        isSlaming = false;
    }

    #region // startState Skills
    private void KnifeAttack()
    {
        GetComponent<Animator>().SetTrigger("attack");
    }

    #endregion

    #region //skinnyState Skills 
    private void Scratch()
    {
        GetComponent<Animator>().SetTrigger("slash");
    }
    #endregion

    #region //strongState skills
    private void Stomp()
    {
        GetComponent<Animator>().SetTrigger("stomp");
    }

    #endregion

    public void DamageArea()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, (stateChecker.currentState.attackRange));

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<EnemyHealth>().TakingDamage(stateChecker.currentState.attackPower);  
        }    
    }
    public void SwordSound ()
    {
/*         _audio.clip = null;
        _audio.clip = SwoordSound;
        _audio.Play(); */
    }
}


