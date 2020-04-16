using Newtonsoft.Json;
using System.Collections.Generic;

namespace RiotSharp.Endpoints.ChampionEndpoint
{
	internal class ChampionList
	{
		/// <summary>
		/// List of Champions.
		/// </summary>
		[JsonProperty("champions")]
		public List<Champion> Champions { get; set; }
	}
}