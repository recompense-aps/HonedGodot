using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using HonedGodot.Extensions;

namespace HonedGodot
{
	public class NodeInfo : Node2D
	{
		public static NodeInfo Create(Node2D anchor)
		{
			var info = new NodeInfo();

			info.anchor = anchor;

			var root = anchor.GetTree().Root;

			root.InlineCallDeffered(() => root.AddChild(info));
			anchor.InlineConnect(anchor, Constants.Signal_Node_TreeExiting, () => info.QueueFree());

			return info;
		}

		public NodeInfo Add(params Func<string>[] infoCallbacks)
		{
			info.AddRange(infoCallbacks.ToList());

			return this;
		}

		public NodeInfo AddForCollisionLayerManagement<T>(CollisionLayerManager<T> manager) where T:Enum
		{
			return Add(
				() => $"layers: {string.Join(",", manager.GetLayers(anchor as CollisionObject2D))}",
				() => $"masks: {string.Join(",", manager.GetMasks(anchor as CollisionObject2D))}"
			);
		}

		private List<Func<string>> info = new List<Func<string>>();
		private Label label = new Label();
		private Node2D anchor;
		private Vector2 position = Vector2.Zero;

		public override void _Ready()
		{
			label.Modulate = Colors.Red;
			AddChild(label);

			AddToGroup(nameof(NodeInfo));
		}

		public override void _Process(float delta)
		{
			label.Text = string.Join("\n", info.Select(x => x()));

			GlobalPosition = anchor.GlobalPosition + position;
		}

		public NodeInfo WithPosition(Vector2 pos)
		{
			position = pos;
			return this;
		}

		public NodeInfo WithGetters(params Func<string>[] infoCallbacks)
		{
			info.AddRange(infoCallbacks);
			return this;
		}
	}
}