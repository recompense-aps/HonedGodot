using HonedGodot.Extensions;
using System;

namespace HonedGodot
{
	public static partial class HG
	{
		public class ThrottledAction
		{
			private Action action;
			
			private long delay = 0;
			private long? lastRun = null;

			public static ThrottledAction Create(Action action, long delayMiliseconds)
			{
				return new ThrottledAction()
				{
					action = action,
					delay = delayMiliseconds * 10_000
				};
			}

			public void Execute()
			{
				if (lastRun == null || DateTime.Now.Ticks - lastRun.Value >= delay)
				{
					action();
					lastRun = DateTime.Now.Ticks;
				}	
			}
		}
	}
}