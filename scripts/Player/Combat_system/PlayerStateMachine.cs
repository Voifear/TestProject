using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Dictionary<Type, IState> behaviorsMap;
    private IState BehaviourCurrent;
    public Animator _anim;

    private void Start()
    {
        Person _person = new Person(_anim);
        this.InitBehaviours();
        this.SetBehavoiurByDefault();
    }

    private void InitBehaviours()
    {
        Person _person = new Person(_anim);
        this.behaviorsMap = new Dictionary<Type, IState>();

        //this.behaviorsMap[typeof(IEnterCombat)] = new IEnterCombat();
        //this.behaviorsMap[typeof(ICombat)] = new ICombat();
        //this.behaviorsMap[typeof(IExitCombat)] = new IExitCombat();
    }

    private void SetBehavoiur(IState newBehaviour)
    {
        if (this.BehaviourCurrent != null)
        {
            this.BehaviourCurrent.Exit();
        }
        this.BehaviourCurrent = newBehaviour;
        this.BehaviourCurrent.Enter();
    }

    private void SetBehavoiurByDefault()
    {
        //this.SetEnterCombat();
    }
  

    private IState GetBehavior<T>() where T : IState
    {
        var type = typeof(T);
        return this.behaviorsMap[type];
    }

    /*private void Update()
    {
        if(this.BehaviourCurrent != null)
        {
            this.BehaviourCurrent.Combat();
        }
    }*/

    /*private void SetEnterCombat()
    {
        var combat = this.GetBehavior<IEnterCombat>();
        this.SetBehavoiur(combat);
    }

    private void SetCombat()
    {
        var combat = this.GetBehavior<ICombat>();
        this.SetBehavoiur(combat);
    }

    private void SetExitCombat()
    {
        var combat = this.GetBehavior<IExitCombat>();
        this.SetBehavoiur(combat);
    }*/
}
