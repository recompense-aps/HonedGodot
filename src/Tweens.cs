using System;
using System.Threading.Tasks;
using Godot;

namespace HonedGodot
{
	public static class Tweens
	{
		public static async Task SimpleFadeIn(Node scene, CanvasItem target, float time, float startAlpha = 0, float endAlpha = 1)
		{
			Tween t = Setup(scene);

			t.InterpolateProperty(target, "modulate", new Color(target.Modulate, startAlpha), new Color(target.Modulate.r, target.Modulate.g, target.Modulate.b, endAlpha), time, Tween.TransitionType.Linear);

			await Execute(t);
		}

		public static async Task SimpleFadeOut(Node scene, CanvasItem target, float time, float alpha = 0)
		{
			Tween t = Setup(scene);

			t.InterpolateProperty(target, "modulate", target.Modulate, new Color(target.Modulate, alpha), time, Tween.TransitionType.Linear);

			await Execute(t);
		}

		public static async Task SimpleColorSwitch(Node scene, CanvasItem target, float time, Color color)
		{
			Tween t = Setup(scene);

			t.InterpolateProperty(target, "modulate", target.Modulate, color, time, Tween.TransitionType.Linear);

			await Execute(t);
		}

		public static async Task SimpleMove(Node scene, Node2D target, Vector2 newPosition, float time)
		{
			Tween t = Setup(scene);

			t.InterpolateProperty(target, "position", target.Position, newPosition, time, Tween.TransitionType.Linear);

			await Execute(t);
		}

		private static Tween Setup(Node scene)
		{
			Tween t = new Tween();
			scene.AddChild(t);

			return t;
		}

		private static async Task Execute(Tween t)
		{
			t.Start();

			await t.ToSignal(t, "tween_completed");
			t.QueueFree();
		}
	}
}