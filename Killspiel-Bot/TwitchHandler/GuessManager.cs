using System.Collections.Generic;
using System.Linq;

namespace TwitchHandler
{
	internal class GuessManager
	{
		private readonly Dictionary<string, int> Guesses = new Dictionary<string, int>();
		private bool IsRunning { get; set; } = false;


		public string Start()
		{
			if (!IsRunning)
				Guesses.Clear();

			IsRunning = true;
			return Messages.GameStarted;
		}

		public string Stop()
		{
			IsRunning = false;
			return Messages.GameStopped;
		}

		public string Guess(string user, int guess)
		{
			if (!IsRunning)
			{
				return Messages.GameInactive;
			}

			Guesses[user] = guess;
            return null;
        }

		public string Resolve(int realKills)
		{
            if (IsRunning)
			{
				return Messages.GameActive;
			}


			List<string> winners = Guesses
                .Where(guess => guess.Value == realKills)
                .Select(guess => guess.Key)
                .ToList();

            return  Messages.GenerateResultMessage(winners);
		}
	}
}