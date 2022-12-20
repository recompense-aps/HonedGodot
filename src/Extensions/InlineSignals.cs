using Godot;
using System;

namespace HonedGodot.Extensions
{
	public static class InlineSignals
	{
		public static T Connect<T>(Node context, Node source, string signal, T inline) where T:Node
		{
			context.AddChild(inline);
			source.Connect(signal, inline, "Execute");

			return inline;
		}
	}

	public class InlineSignal : Node
	{
		private Action action;

		public InlineSignal(Action handler)
		{
			action = handler;
		}

		public void Execute()
		{
			action();
		}
	}

	public class InlineSignal<T> : Node
	{
		private Action<T> action;

		public InlineSignal(Action<T> handler)
		{
			action = handler;
		}

		public void Execute(T arg1)
		{
			action(arg1);
		}
	}

	public class InlineSignal<T1, T2> : Node
	{
		private Action<T1, T2> action;

		public InlineSignal(Action<T1, T2> handler)
		{
			action = handler;
		}

		public void Execute(T1 arg1, T2 arg2)
		{
			action(arg1, arg2);
		}
	}

	public class InlineSignal<T1, T2, T3> : Node
	{
		private Action<T1, T2, T3> action;

		public InlineSignal(Action<T1, T2, T3> handler)
		{
			action = handler;
		}

		public void Execute(T1 arg1, T2 arg2, T3 arg3)
		{
			action(arg1, arg2, arg3);
		}
	}

	public class InlineSignal<T1, T2, T3, T4> : Node
	{
		private Action<T1, T2, T3, T4> action;

		public InlineSignal(Action<T1, T2, T3, T4> handler)
		{
			action = handler;
		}

		public void Execute(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			action(arg1, arg2, arg3, arg4);
		}
	}

	public class InlineSignal<T1, T2, T3, T4, T5> : Node
	{
		private Action<T1, T2, T3, T4, T5> action;

		public InlineSignal(Action<T1, T2, T3, T4, T5> handler)
		{
			action = handler;
		}

		public void Execute(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			action(arg1, arg2, arg3, arg4, arg5);
		}
	}
}