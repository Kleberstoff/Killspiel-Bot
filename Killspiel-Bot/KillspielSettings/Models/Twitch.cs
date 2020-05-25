using Newtonsoft.Json;

namespace Killspiel.Settings.Models
{
	public class Twitch
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Username { get; set; } = "username";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string OAuth { get; set; } = "oath:";

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string Channel { get; set; } = "twitch";
	}
}