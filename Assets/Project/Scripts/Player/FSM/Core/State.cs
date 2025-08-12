using UnityEngine;

namespace FSM
{
    public abstract class State<T> : ScriptableObject
    {
        protected T context;
        protected StateMachine<T> stateMachine;

        public void SetContextAndStateMachine(T context, StateMachine<T> stateMachine)
        {
            this.context = context;
            this.stateMachine = stateMachine;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Tick() { }
        public virtual void FixedTick() { }
    }
}