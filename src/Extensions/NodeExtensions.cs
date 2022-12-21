using System;
using Godot;

namespace HonedGodot.Extensions
{
	public static class NodeExtensions
	{
		public static InlineSignal InlineConnect(this Node context, Node source, string signal, Action handler)
		{
			return InlineSignals.Connect(context, source, signal, new InlineSignal(handler));
		}

		public static InlineSignal<T> InlineConnect<T>(this Node context, Node source, string signal, Action<T> handler)
		{
			return InlineSignals.Connect(context, source, signal, new InlineSignal<T>(handler));
		}

		public static InlineSignal<T1, T2> InlineConnect<T1, T2>(this Node context, Node source, string signal, Action<T1, T2> handler)
		{
			return InlineSignals.Connect(context, source, signal, new InlineSignal<T1, T2>(handler));
		}

		public static InlineSignal<T1, T2, T3> InlineConnect<T1, T2, T3>(this Node context, Node source, string signal, Action<T1, T2, T3> handler)
		{
			return InlineSignals.Connect(context, source, signal, new InlineSignal<T1, T2, T3>(handler));
		}

		public static InlineSignal<T1, T2, T3, T4> InlineConnect<T1, T2, T3, T4>(this Node context, Node source, string signal, Action<T1, T2, T3, T4> handler)
		{
			return InlineSignals.Connect(context, source, signal, new InlineSignal<T1, T2, T3, T4>(handler));
		}

		public static InlineSignal<T1, T2, T3, T4, T5> InlineConnect<T1, T2, T3, T4, T5>(this Node context, Node source, string signal, Action<T1, T2, T3, T4, T5> handler)
		{
			return InlineSignals.Connect(context, source, signal, new InlineSignal<T1, T2, T3, T4, T5>(handler));
		}

		public static Func<bool> TemporaryEffect<T>(this T node, Action start, Action end, float time) where T:Node
		{
			var timer = new Timer();
			timer.WaitTime = time;
			bool locked = false;

			node.InlineConnect(timer, Constants.Signal_Timer_Timeout, () => 
			{
				timer.QueueFree();
				end();
				locked = true;
				GD.Print("cleared timer");
			});

			start();

			node.AddChild(timer);
			timer.Start();

			return () => locked;
		}

	}
}