using System;

namespace FSM
{
    public class StateMachine<T>
    {
        private State<T> currentState;
        private T context;

        public State<PlayerContext> CurrentState { get; internal set; }

        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;
            ChangeState(initialState);
        }

        public void ChangeState(State<T> newState)
        {
            if (currentState == newState) return;

            if (currentState != null)
                currentState.Exit(); 
            
            currentState = newState; 

            if (currentState != null)
            {
                currentState.SetContextAndStateMachine(context, this);
                currentState.Enter();
            }
        }

        public void Tick()
        {
            if (currentState != null)
                currentState.Tick();
        }

        public void FixedTick()
        {
            if (currentState != null)
                currentState.FixedTick();
        }
    }
}