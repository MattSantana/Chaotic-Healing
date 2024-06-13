using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSkills : MonoBehaviour
{
    private PlayerStateMachineSwitcher stateChecker;

    private void Start() {
        stateChecker = GetComponent<PlayerStateMachineSwitcher>();
    }

    private void Update()
    {
        switch (stateChecker.currentState.stateName)
        {
            case "StartState":
                
                break;
            case "StrongState":
                
                break;
            case "SkinnyState":
                
                break;
        }
    }

    private void Dash()
    {

    }

    #region // startState Skills
    private void KnifeAttack()
    {

    }

    #endregion

    #region //skinnyState Skills 
    private void DashSkill()
    {

    }

    private void Scratch()
    {

    }
    #endregion

    #region //strongState skills
    private void Stomp()
    {

    }

    private void BodySlam()
    {

    }
    #endregion

}


