using System;
using System.Collections.Generic;

namespace HonedGodot.State
{
	public class ComposableFiniteStateMachine<T> where T:Enum
	{
		public class State
		{
			public T Id { get; set; }
			public Action<ComposableFiniteStateMachine<T>> Start { get; private set; }
            public Action<ComposableFiniteStateMachine<T>> Execute { get; private set; }
            public Action<ComposableFiniteStateMachine<T>> End { get; private set; }

            public State WithStart(Action<ComposableFiniteStateMachine<T>> start)
            {
                Start = start;
                return this;
            }

            public State WithExecute(Action<ComposableFiniteStateMachine<T>> execute)
            {
                Execute = execute;
                return this;
            }

            public State WithEnd(Action<ComposableFiniteStateMachine<T>> end)
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
			state.Id = stateId;
            states[stateId] = state;
            return state;
        }
	} 
}