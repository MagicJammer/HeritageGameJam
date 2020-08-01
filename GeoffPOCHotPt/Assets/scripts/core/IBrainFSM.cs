using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class IBrainFSM : MonoBehaviour
{
    Dictionary<Type, IState> _states = new Dictionary<Type, IState>();
    Type _currentState;
    IState _existing;

    public void RegisterState(IState state)
    {
        _states[state.GetType()] = state;
    }

    public bool ChangeState(Type stateType, params object[] args)
    {
        if (_currentState == stateType)
            return false;

        IState newState;
        if (!_states.TryGetValue(stateType, out newState))
            return false;

        IState currentState;
        if (_currentState != null && _states.TryGetValue(_currentState, out currentState))
            currentState.OnStateExit();

        _currentState = stateType;
        newState.OnStateEnter(args);
        _existing = newState;
        return true;
    }

    public void UpdateBrain()
    {
        _existing.OnStateUpdate();
        //Debug.Log(_existing);
    }
}
public abstract class IState
{
    public IBrainFSM Brain;
    public IState(IBrainFSM brain)
    {
        Brain = brain;
    }
    public abstract void OnStateEnter(object[] args);
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}