using Godot;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace HonedGodot
{
	public class LoadScene : System.Attribute
	{
		public string Path { get; private set; }

		public LoadScene(string path)
		{
			Path = path;
		}
	}

	public static class SceneLoader
	{
		private static Dictionary<string, PackedScene> scenes = new Dictionary<string, PackedScene>();

		public static T Instance<T>(string name) where T:Node
		{
			return scenes[name].Instance<T>();
		}

		public static T Instance<T>() where T:Node
		{
			var type = typeof(T);
			return scenes[type.Name].Instance<T>();
		}

		public static void Load()
		{
			var attributeType = typeof(LoadScene);

			var types = Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(x => x.GetCustomAttribute(attributeType) != null);

			foreach(var type in types)
			{
				var name = type.Name;
				var path = (type.GetCustomAttribute(attributeType) as LoadScene).Path;

				scenes[name] = GD.Load<PackedScene>(path);

				Logging.Log($"Loaded {path} as {name}", HonedGodotLogTag.SceneLoader);
			}
		}
	}
}