using Newtonsoft.Json.Linq;
using RiotSharp;
using RiotSharp.Endpoints.MatchEndpoint;
using RiotSharp.Endpoints.SpectatorEndpoint;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Misc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LeagueHandler
{
	public class Handler
	{
		private static readonly string VersionJson = "https://ddragon.leagueoflegends.com/api/versions.json";
		private static string ChampionJson = "https://ddragon.leagueoflegends.com/cdn/{0}/data/en_US/champion.json";

		private static string CurrentVersion = string.Empty;

		private static readonly Dictionary<string, long> Champions = new Dictionary<string, long>();

		private static RiotApi API = default;

		public async Task Initialize(string key)
		{
			CurrentVersion = await GetCurrentVersion();
			ChampionJson = ChampionJson.Replace("{0}", CurrentVersion);

			await GenerateChampionList();

#if DEBUG
			API = RiotApi.GetDevelopmentInstance(key);
#else
			API = RiotApi.GetInstance(key, 20, 100);
#endif
		}

		public async Task<bool> CheckIfPlaysChampion(Region region, string summonerName, string championName)
		{
			Summoner summoner = await GetSummonerByName(region, summonerName);

			if (summoner is null)
				return false;

			CurrentGame match = await GetCurrentMatchId(region, summoner.Id);

			if (match is null)
				return false;

			CurrentGameParticipant participant = match.Participants.SingleOrDefault(x => x.SummonerId == summoner.Id);

			Champions.TryGetValue(championName, out long championId);

			if (participant.ChampionId == championId)
				return true;

			return false;
		}

		public async Task<Summoner> GetSummonerByName(Region region, string summonerName)
		{
			try
			{
				return await API.Summoner.GetSummonerByNameAsync(region, summonerName);
			}
			catch (RiotSharpException e)
			{
				// TODO: Logging
				return default;
			}
		}

		public async Task<Match> GetMatchByMatchId(Region region, long matchId)
		{
			try
			{
				return await API.Match.GetMatchAsync(region, matchId);
			}
			catch (RiotSharpException e)
			{
				// TODO: Logging
				return default;
			}
		}

		public async Task<CurrentGame> GetCurrentMatchId(Region region, string summonerId)
		{
			try
			{
				return await API.Spectator.GetCurrentGameAsync(region, summonerId);
			}
			catch (RiotSharpException e)
			{
				// TODO: Logging
				return default;
			}
		}

		// TODO: Rewrite this POS

		public async Task<string> GetCurrentVersion()
		{
			using HttpClient httpClient = new HttpClient();

			HttpResponseMessage result = await httpClient.GetAsync(VersionJson);

			string res = await result.Content.ReadAsStringAsync();
			JArray versionJson = JArray.Parse(res);

			return versionJson.First.ToString();
		}

		public async Task GenerateChampionList()
		{
			using HttpClient httpClient = new HttpClient();

			HttpResponseMessage result = await httpClient.GetAsync(ChampionJson);

			string res = await result.Content.ReadAsStringAsync();
			JObject championJson = JObject.Parse(res);

			foreach (JToken token in championJson["data"].Values())
				Champions.Add(token["name"].ToString(), long.Parse(token["key"].ToString()));
		}
	}
}