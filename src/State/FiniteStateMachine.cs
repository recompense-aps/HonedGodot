using System;
using System.Linq;

namespace HonedGodot.State
{
	public class FiniteStateMachine
	{
		public string CurrentStateName => currentState?.GetType()?.Name;
		public State[] States { get; private set; }
		private State currentState = null;

		public FiniteStateMachine(params State[] states)
		{
			States = states;
			
			foreach(var state in States)
				state.Machine = this;
		}

		public State Execute()
		{
			currentState?.Execute();
			return currentState;
		}

		public State Switch<T>() where T:State
		{
			var newState = States.SingleOrDefault(x => x is T);

			if (newState == null)
				throw new Exception($"Could not find state {typeof(T).Name}");

			currentState?.End();
			newState.Start();
			currentState = newState;

			return currentState;
		}

		public abstract class State
		{
			public FiniteStateMachine Machine { get; internal set; }
			public virtual void Start() {}
			public virtual void End() {}
			public virtual void Execute() {}
		}
	}
}

