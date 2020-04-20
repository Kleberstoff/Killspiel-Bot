using System;
using TwitchLib.Client.Enums;

namespace TwitchHandler
{
	internal class CommandAction
	{
		public Predicate<UserType> Condition { get; }
		public Action<string, string, string[]> Action { get; }

		public CommandAction(Predicate<UserType> condition, Action<string, string, string[]> action)
		{
			Condition = condition;
			Action = action;
		}
	}
}