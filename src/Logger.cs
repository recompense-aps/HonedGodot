using System.Collections.Generic;
using Godot;

namespace HonedGodot
{
	public enum HonedGodotLogTag { ErrorHandling, SceneLoader, Tagging }

	static class Logging
	{
		public static bool Enabled { get; set; }

		private static Logger<HonedGodotLogTag> internalLogger = new Logger<HonedGodotLogTag>("hg");

		public static void Log(object what, HonedGodotLogTag tag)
		{
			internalLogger.Log(what, tag);
		}
	}

	public class Logger<T>
	{
		private HashSet<T> disabledTags = new HashSet<T>();
		private string Name { get; set; }

		public Logger(string name)
		{
			Name = name;
		}

		public void Log(object what, T tag)
		{
			if (disabledTags.Contains(tag))
				return;

			GD.Print($"[{Name}] [{tag}] {what}");
		}

		public void Disable(params T[] tags)
		{
			foreach(var tag in tags)
				disabledTags.Add(tag);
		}

		public void Enable(params T[] tags)
		{
			foreach(var tag in tags)
				disabledTags.Remove(tag);
		}
	}
}