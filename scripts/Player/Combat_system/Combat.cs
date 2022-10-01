using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICombat 
{
    public void Enter()
    {
        Debug.Log("Enter (CombatState)");
    }

    public void Combat()
    {
        Debug.Log("Combat (CombatState)");
    }

    public void Exit()
    {
        Debug.Log("Exit (CombatState)");
    }
}
