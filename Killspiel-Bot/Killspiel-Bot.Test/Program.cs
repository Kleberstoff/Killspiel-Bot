using RiotSharp.Misc;
using System;
using System.Threading.Tasks;

namespace KillspielBot.Test
{
	internal class Program
	{
		private static string APIKey = "RGAPI-";

		private static async Task Main()
		{
			LeagueHandler.Handler LeagueHandler = new LeagueHandler.Handler();
			TwitchHandler.Handler TwitchHandler = new TwitchHandler.Handler();

			await LeagueHandler.Initialize(APIKey);

			var v = await LeagueHandler.CheckIfPlaysChampion(Region.Euw, "Kleberstoff", "Yasuo");
		}
	}
}