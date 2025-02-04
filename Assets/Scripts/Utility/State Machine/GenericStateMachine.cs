using System;
using System.Collections.Generic;
using System.Linq;

namespace ServiceLocator.Utility
{
    public class GenericStateMachine<T, U> where T : class, IStateOwner<T> where U : Enum
    {
        protected T owner;
        protected IState<T, U> currentState;
        protected Dictionary<U, IState<T, U>> States = new Dictionary<U, IState<T, U>>();

        public GenericStateMachine(T _owner) => owner = _owner;

        public void Update() => currentState?.Update();
        public void FixedUpdate() => currentState?.FixedUpdate();

        public U GetCurrentState()
        {
            return States.Keys.FirstOrDefault(key => States[key] == currentState);
        }

        protected void ChangeState(IState<T, U> _newState)
        {
            currentState?.OnStateExit();
            currentState = _newState;
            currentState?.OnStateEnter();
        }

        public void ChangeState(U _newState) => ChangeState(States[_newState]);

        protected void SetOwner()
        {
            foreach (IState<T, U> _state in States.Values)
            {
                _state.Owner = owner;
            }
        }
    }
}