using System;
using Godot;

namespace HonedGodot
{
	public static class ErrorHandling
	{
		public static Exception CrashError { get; private set; }
		public static string ErrorScreenPath { get; private set; }
		private static Node Context { get; set; }
		private static bool configured = false;

		static ErrorHandling()
		{
			Logging.Log("Initialized Error Handling", HonedGodotLogTag.ErrorHandling);

			GD.UnhandledException += (sender, args) => 
			{
				CrashError = args.Exception;
				GoToErrorScreen();
				
				Logging.Log($"handled error: {CrashError.Message}", HonedGodotLogTag.ErrorHandling);
			};
		}

		public static void Configure(string errorScreenPath, Node context)
		{
			if (configured)
				return;

			configured = true;

			ErrorScreenPath = errorScreenPath;
			Context = context;
		}

		public static void FatalError(Exception e)
		{
			CrashError = e;
			GoToErrorScreen();
		}

		private static void GoToErrorScreen()
		{
			if (ErrorScreenPath != null && Context != null)
			{
				Context.GetTree().ChangeScene(ErrorScreenPath);
			}
		}
	}
}