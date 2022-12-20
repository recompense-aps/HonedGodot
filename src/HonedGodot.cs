using System.Reflection;
using System.Linq;
using Godot;

namespace HonedGodot
{
	public static partial class HG
	{
		public const string VERSION = "0.0.0";
		public const string GODOT_VERSION = "3.5.1";

		public static T GetNodes<T>(T node) where T:Node
		{
			var nodeType = node.GetType();
			var props = nodeType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			foreach(var prop in props)
			{
				var attribute = prop.GetCustomAttribute(typeof(GetNode));

				if (attribute != null)
				{
					var converted = attribute as GetNode;
					var value = node.GetNode(converted.Path);

					prop.SetValue(node, value);
				}
			}

			return node;
		}
	}

	public class GetNode : System.Attribute
	{
		public string Path { get; private set; }

		public GetNode(string path)
		{
			Path = path;
		}
	}
}
