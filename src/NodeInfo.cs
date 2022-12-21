using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

namespace HonedGodot
{
	public class NodeInfo : Node2D
	{
		public static NodeInfo Add<T>(T node, params Func<string>[] infoCallbacks) where T:Node2D
		{
			var info = new NodeInfo();

			info.info = infoCallbacks.ToList();
			info.anchor = node;

			node.GetTree().Root.CallDeferred("add_child", info);

			return info;
		}

		private List<Func<string>> info = new List<Func<string>>();
		private Label label = new Label();
		private Node2D anchor;
		private Vector2 position = Vector2.Zero;

		public override void _Ready()
		{
			label.Modulate = Colors.Red;
			AddChild(label);
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
	}
}