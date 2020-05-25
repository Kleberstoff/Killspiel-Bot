using Newtonsoft.Json;

namespace Killspiel.Settings.Models
{
	public class Messages
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string InvalidArgCount { get; set; } = "Ungültige Argumentzahl!";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string InvalidArg { get; set; } = "Ungültiges Argument!";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string GameActive { get; set; } = "Killspiel läuft noch!";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string GameInactive { get; set; } = "Killspiel ist nicht aktiv!";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string GameStarted { get; set; } = "Tipps werden jetzt angenommen!";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string GameStopped { get; set; } = "Tipps werden nicht mehr angenmommen!";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string InvalidCommand { get; set; } = "Ungültiger Befehl!";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string InsufficientPermissions { get; set; } = "Keine Berechtigung!";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Winners { get; set; } = "Gewonnen haben: ";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string NoWinners { get; set; } = "Niemand hat gewonnen! rvnxmaOof";
	}
}