using Godot;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

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

	/// <summary>
	/// Used to handle preloading scenes whose scripts use the <c>LoadScene</c> attribute.
	/// Scenes can be loaded synchronously or asynchronously via <c>Load</c> and <c>LoadAsync</c>.
	/// Load methods should only be called once.
	/// </summary>
	public static class SceneLoader
	{
		public class SceneLoadStep
		{
			public float Progress { get; internal set; }
		}

		private static Dictionary<string, PackedScene> scenes = new Dictionary<string, PackedScene>();
		private static Type attributeType { get; } = typeof(LoadScene);

		/// <summary>
		/// Creates a new instance of a preloaded scene as the specified type.
		/// </summary>
		/// <typeparam name="T">The type associated with the loaded scene</typeparam>
		/// <returns></returns>
		public static T Instance<T>() where T:Node
		{
			var type = typeof(T);
			var name = type.Name;

			if (!scenes.ContainsKey(name))
				throw new ArgumentException($"Could not find scene with id '{name}'");

			return scenes[name].Instance<T>();
		}

		/// <summary>
		/// Synchronously finds every type with the <c>LoadScene</c> attribute and loads a packed
		/// scene using the type's name as the id for instancing.
		/// </summary>
		public static void Load()
		{
			var types = GetTypes();
			
			foreach(var type in types)
				LoadStep(type);
		}

		/// <summary>
		/// Asynchronously finds every type with the <c>LoadScene</c> attribute and loads a packed
		/// scene using the type's name as the id for instancing.
		/// </summary>
		public static async Task LoadAsync(Action<SceneLoadStep> loadCallback = null)
		{
			var types = GetTypes();
			var loads = new List<Action>();
			float loaded = 0;

			foreach(var type in types)
			{
				loads.Add(() => 
				{
					LoadStep(type, loadCallback, loaded, types.Count());
					loaded++;
				});
			}

			await Task.WhenAll(
				loads.Select(x => Task.Run(x))
			);
		}

		private static void LoadStep(Type type, Action<SceneLoadStep> loadCallback = null, float? loaded = null, float? total = null)
		{
			var name = type.Name;
			var path = (type.GetCustomAttribute(attributeType) as LoadScene).Path;

			scenes[name] = GD.Load<PackedScene>(path) ?? 
				throw new ArgumentException($"Could not load scene for '{name}'. Path '{path}' could not be found");

			Logging.Log($"Loaded {path} as {name}", HonedGodotLogTag.SceneLoader);

			loadCallback?.Invoke(
				new SceneLoadStep()
				{
					Progress = loaded.Value / total.Value
				}
			);
		}

		private static IEnumerable<Type> GetTypes()
		{
			var attributeType = typeof(LoadScene);

			return Assembly
				.GetExecutingAssembly()
				.GetTypes()
				.Where(x => x.GetCustomAttribute(attributeType) != null);
		}
	}
}