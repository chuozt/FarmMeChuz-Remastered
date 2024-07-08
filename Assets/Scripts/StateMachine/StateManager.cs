using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> CurrentState;

    protected bool IsTransitioningState = false;

    void Start()
    {
        CurrentState.EnterState();
    }

    void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if(!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
            CurrentState.UpdateState();
        else if(!IsTransitioningState)
            TransitToState(nextStateKey);
    }

    public void TransitToState(EState stateKey)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[stateKey];
        CurrentState.EnterState();
        IsTransitioningState = false;
    }

    void OnTriggerEnter2D(Collider2D col){}
    void OnTriggerStay2D(Collider2D col){}
    void OnTriggerExit2D(Collider2D col){}
}
