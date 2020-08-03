using System;
using System.Collections.Generic;
using UnityEngine;
//namespace Deus
//{
    public abstract class FiniteStateMachine<S> : MonoBehaviour where S : struct, Enum
  {
        Dictionary<S, Deus_State> _states = new Dictionary<S, Deus_State>();
        S _currentStateTag;
        public S CurrentStateType { get { return _currentStateTag; } }
        public abstract S UnassignedType { get; }
        Deus_State CurrentState
        {
            get
            {
                if (_states.TryGetValue(_currentStateTag, out Deus_State currentState))
                    return currentState;
                else
                    return null;
            }
        }
        public void RegisterState(Deus_State state)
        {
            _states[state.StateType] = state;
        }
        bool _changingState = false;
        public bool ChangeState(S NextState, params object[] args)
        {
            if (NextState.CompareTo(UnassignedType) == 0)
                return false;
            if (_changingState)
                return false;
            if (_currentStateTag.CompareTo(NextState) == 0)
                return false;
            if (!_states.TryGetValue(NextState, out Deus_State newState))
                return false;
            _changingState = true;
            Deus_State currentState = CurrentState;
            if (currentState != null)
                currentState.OnStateExit(NextState, args);
            newState.OnStateEnter(_currentStateTag, args);
            _currentStateTag = NextState;
            _changingState = false;
            return true;
        }
        public virtual void UpdateMachine()
        {
            var current = CurrentState;
            if(current!=null)
                current.OnStateUpdate();
        }

        public bool HasState(S key)
        {
            return _states.ContainsKey(key);
        }
        public void SendMessageToBrain(int msgtype, params object[] args)
        {
            CurrentState.OnReceiveMessage(msgtype, args);
        }
    public abstract class Deus_State
    {
        public FiniteStateMachine<S> Machine;
        public Deus_State(FiniteStateMachine<S> machine)
        {
            Machine = machine;
        }
        public abstract S StateType { get; }
        public abstract void OnStateEnter(S prevStateType, object[] args);
        public abstract void OnStateUpdate();
        public abstract void OnStateExit(S newStateType, object[] arg);
        public abstract void OnReceiveMessage(int msgtype, object[] args);
    }
    public abstract class SI_State<U> : Deus_State where U : FiniteStateMachine<S>
    {
        public U user;
        public SI_State(U brain) : base(brain) { }
    }
  }
//}