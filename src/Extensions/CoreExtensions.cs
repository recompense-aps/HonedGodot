using System;
using HonedGodot;

namespace HonedGodot.Extensions
{
	public static class CoreExtensions
	{
		public static Action Throttle(this Action action, long delay)
		{
			var throttled = HG.ThrottledAction.Create(action, delay);

			return throttled.Execute;
		}
	}
}