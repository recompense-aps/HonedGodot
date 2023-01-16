using System;
using System.Reflection;
using System.Linq;
using System.Text;
using Godot;
using HonedGodot.Extensions;
using System.Collections.Generic;

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

		public static Action CreateInterval(Node context, float timeoutInSeconds, Action action)
		{
			var timer = new Timer();
			timer.WaitTime = timeoutInSeconds;

			timer.InlineConnect(timer, Constants.Signal_Timer_Timeout, () => 
			{
				timer.Start();
				action();
			});

			context.AddChild(timer);
			timer.Start();

			return () => timer.QueueFree();
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

	public class CommandData
	{
		public string Command { get; set; }
		public string[] Args { get; set; }

		public CommandData(string raw)
		{
			Command = raw.Split(' ')[0];
			Args = raw.Split(' ').Skip(1).ToArray();
		}

		public bool DefaultExecute(Node context)
		{
			if (Command == "_")
			{
				string nodeName = Args[0];
				string methodName = Args[1];

				var node = context.FindNode(nodeName);
				var type = node.GetType();
				var method = type.GetMethod(methodName);

				method.Invoke(node, ConvertArgs(Args.Skip(2)));
			}

			return false;
		}

		private object[] ConvertArgs(IEnumerable<string> args)
		{
			List<object> converted = new List<object>();

			foreach(var arg in args)
			{
				var splits = arg.Split(':');
				var value = splits[0].Trim();
				var type = splits[1].ToLower().Trim();
				object convertedValue = null;

				switch(type)
				{
					case "int":
						convertedValue = Convert.ToInt32(value);
						break;
					case "string":
						convertedValue = Convert.ToString(value);
						break;
					default:
						throw new InvalidOperationException($"Type '{type}' for arg {arg} is invalid");
				}

				converted.Add(convertedValue);
			}

			return converted.ToArray();
		}
	}
}
