using System;
using System.Collections.Generic;

namespace HonedGodot
{
	public class FiniteStateMachine<T> where T:Enum
	{
		public class State
		{
			public Action<FiniteStateMachine<T>> Start { get; private set; }
            public Action<FiniteStateMachine<T>> Execute { get; private set; }
            public Action<FiniteStateMachine<T>> End { get; private set; }

            public State WithStart(Action<FiniteStateMachine<T>> start)
            {
                Start = start;
                return this;
            }

            public State WithExecute(Action<FiniteStateMachine<T>> execute)
            {
                Execute = execute;
                return this;
            }

            public State WithEnd(Action<FiniteStateMachine<T>> end)
            {
                End = end;
                return this;
            }
		}

		public State CurrentState { get; private set; }

		private Dictionary<T, State> states = new Dictionary<T, State>();

		public void Execute()
		{
			CurrentState?.Execute?.Invoke(this);
		}

		public void Switch(T stateId)
        {
            if (!states.ContainsKey(stateId))
                throw new ArgumentException($"state '{stateId}' is not a valid state");

            var newState = states[stateId];

            if (newState == null)
                throw new ArgumentException($"state '{stateId}' is null");

            CurrentState?.End?.Invoke(this);
            CurrentState = newState;
            CurrentState.Start?.Invoke(this);
        }

        public State Compose(T stateId)
        {
            var state = new State();
            states[stateId] = state;
            return state;
        }
	} 
}