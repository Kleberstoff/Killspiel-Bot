using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client;

namespace TwitchHandler
{
	internal class GuessManager
	{
		private readonly TwitchClient TwitchClient;
		private readonly Dictionary<string, int> Guesses = new Dictionary<string, int>();
		private bool IsRunning { get; set; } = false;

		public GuessManager(TwitchClient twitchClient)
		{
			TwitchClient = twitchClient;
		}

		public void Start(string channel)
		{
			if (!IsRunning)
				Guesses.Clear();

			IsRunning = true;
			TwitchClient.SendMessage(channel, Messages.GameStarted);
		}

		public void Stop(string channel)
		{
			IsRunning = false;
			TwitchClient.SendMessage(channel, Messages.GameStopped);
		}

		public void Guess(string channel, string user, string[] args)
		{
			if (!IsRunning)
			{
				TwitchClient.SendMessage(channel, Messages.GameInactive);
				return;
			}

			if (args.Length != 1)
			{
				TwitchClient.SendMessage(channel, Messages.InvalidArgCount);
				return;
			}

            if (!int.TryParse(args[0].Trim(), out int guess))
			{
				TwitchClient.SendMessage(channel, Messages.InvalidArg);
				return;
			}

			Guesses[user] = guess;
		}

		public void Resolve(string channel, string[] args)
		{
			if (args.Length == 1 && !IsRunning)
			{
				TwitchClient.SendMessage(channel, Messages.InvalidArgCount);
				return;
			}

			if (IsRunning)
			{
				TwitchClient.SendMessage(channel, Messages.GameActive);
				return;
			}

            if (int.TryParse(args[0].Trim(), out int realKills))
			{

				List<string> winners = Guesses
                    .Where(guess => guess.Value == realKills)
                    .Select(guess => guess.Key)
                    .ToList();

                TwitchClient.SendMessage(channel, Messages.GenerateResultMessage(winners));
			}
		}
	}
}