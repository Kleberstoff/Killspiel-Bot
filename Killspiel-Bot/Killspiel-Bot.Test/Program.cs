using System;
using System.Threading.Tasks;

namespace KillspielBot.Test
{
	internal class Program
	{
		private static readonly string LeagueAPIKey = "RGAPI-";

		private static readonly string TwitchUsername = "";
		private static readonly string TwitchPassword = "";
		private static readonly string TwitchChannel = "rvnxmango";

		private static async Task Main()
		{
			LeagueHandler.Handler LeagueHandler = new LeagueHandler.Handler(LeagueAPIKey);
			await LeagueHandler.Initialize();

			TwitchHandler.Handler TwitchHandler = new TwitchHandler.Handler(TwitchUsername, TwitchPassword, TwitchChannel);
			TwitchHandler.Connect();

			Console.ReadKey();
		}
	}
}