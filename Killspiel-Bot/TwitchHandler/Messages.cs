using System.Collections.Generic;

namespace TwitchHandler
{
	public static class Messages
	{
		public const string InvalidArgCount = "Ungültige Argumentzahl!";
		public const string InvalidArg = "Ungültiges Argumen!";
		public const string GameActive = "Killspiel läuft noch!";
		public const string GameInactive = "Killspiel ist nicht aktiv!";
		public const string GameStarted = "Tipps werden jetzt angenommen!";
		public const string GameStopped = "Tipps werden nicht mehr angenmommen!";
		public const string InvalidCommand = "Ungültiger Befehl!";
		public const string InsufficientPermissions = "Keine Berechtigung!";
		public const string Winners = "Gewonnen haben: ";
		public const string NoWinners = "Niemand hat gewonnen! rvnxmaOof";

		public static string GenerateResultMessage(List<string> winners)
		{
			if (winners.Count == 0)
				return NoWinners;

			return Winners + string.Join(", ", winners);
		}
	}
}