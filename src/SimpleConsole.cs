using System;
using System.Collections.Generic;
using Godot;

namespace HonedGodot
{
	public class SimpleConsole : TextEdit
    {
        public static SimpleConsole Instance { get; private set; }
        private static Dictionary<string, Action<SimpleConsole, List<string>>> commands = new Dictionary<string, Action<SimpleConsole, List<string>>>();

        public static SimpleConsole Open(Node anchor, int width = 256, int height = 256)
        {
            if (Instance != null)
                return Instance;

            Instance = new SimpleConsole();
            Instance.RectSize = new Vector2(width, height);
            anchor.AddChild(Instance);

            return Instance;
        }

        public static void Close()
        {
            Instance?.QueueFree();
            Instance = null;
        }
    }
}