using Newtonsoft.Json;

namespace Killspiel.Settings.Models
{
	public class League
	{
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
		public string LeagueAPIKey { get; set; } = "RGAPI-";
	}
}