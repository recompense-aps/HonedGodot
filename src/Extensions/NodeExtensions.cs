using System;
using Godot;

namespace HonedGodot.Extensions
{
	public static class NodeExtensions
	{
		public static T As<T>(this Node n) where T:class
		{
			return n as T;
		}
		
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

		public static void ReConnect(this Node node, string signal, Node source, string handler, Godot.Collections.Array binds = null)
		{
			if (node.IsConnected(signal, source, handler))
				node.Disconnect(signal, source, handler);
				
			node.Connect(signal, source, handler, binds);
		}

		public static void InlineCallDeffered(this Node context, Action action)
		{
			var obj = new DefferedObject();
			obj.Action = action;

			obj.CallDeferred(nameof(DefferedObject.Execute));
		}

		public static void Tag<T>(this Node node, T data)
		{
			var tag = new IdTag<T>();
			tag.Data = data;

			node.AddChild(tag);
		}

		public static IdTag<T> GetTag<T>(this Node node)
		{
			foreach(var child in node.GetChildren())
			{
				if (child is IdTag<T>)
					return child as IdTag<T>;
			}

			return null;
		}

		public static bool EquallyTagged<T>(this Node node, Node otherNode)
		{
			var tag1 = node?.GetTag<T>();
			var tag2 = otherNode?.GetTag<T>();

			if (tag1 == null || tag2 == null) return false;

			return tag1.SameTag(tag2);
		}

		public static void SetZIndexLayer<T>(this Node2D node, T layer) where T:Enum
		{
			node.ZIndex = (int)(object)layer;
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
			});

			start();

			node.AddChild(timer);
			timer.Start();

			return () => locked;
		}

		private class DefferedObject : Godot.Object
		{
			public Action Action { get; set; }

			public void Execute()
			{
				Action();
			}
		}

	}
}