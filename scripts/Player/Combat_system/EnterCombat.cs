using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person
{
    public Animator _anim;

    public Person(Animator animator) { _anim = animator; }
}
public class IEnterCombat 
{
    public Animator _anim;
    
    public void Enter()
    {
        Debug.Log("Enter (EnterCombatState)");
        _anim.SetTrigger("Attack_One");
    }

    public void Combat()
    {
        Debug.Log("Combat (EnterCombatState)");
    }

    public void Exit()
    {
        Debug.Log("Exit (EnterCombatState)");
    }
}
