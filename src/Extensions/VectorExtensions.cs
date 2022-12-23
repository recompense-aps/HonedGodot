using Godot;
namespace HonedGodot.Extensions
{
	public static class VectorExtensions
	{
		public static Vector2 FromRotation(float rotation)
		{
			return new Vector2(
				Mathf.Cos(rotation),
				Mathf.Sin(rotation)
			);
		}
	}
}