using System.Reflection;
using System.Linq;
using System.Text;
using Godot;

namespace HonedGodot
{
	public static partial class HG
	{
		public const string VERSION = "0.0.0";
		public const string GODOT_VERSION = "3.5.1";
		public static string GodotEngineVersion => $"{Engine.GetVersionInfo()["major"]}.{Engine.GetVersionInfo()["minor"]}.{Engine.GetVersionInfo()["patch"]}";

		static HG()
		{
			if (GodotEngineVersion != GODOT_VERSION)
			{
				GD.PrintErr($"Honed Godot - Godot version mismatch. Expected '{GODOT_VERSION}' got '{GodotEngineVersion}'");
			}
		}

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

		public static void GenerateInputMapConstants(string fileName = "Inputs")
		{
			string header = $"// WARNING: this file is auto-generated. Do not make any direct edits\npublic class {fileName}\n{{\n";
			string footer = "\n}";
			StringBuilder file = new StringBuilder(header);

			foreach(var action in InputMap.GetActions())
			{
				file.AppendLine($"\tpublic const string {action} = \"{action}\";");
			}

			file.AppendLine(footer);

			System.IO.File.WriteAllText($"./{fileName}.cs", file.ToString());

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
