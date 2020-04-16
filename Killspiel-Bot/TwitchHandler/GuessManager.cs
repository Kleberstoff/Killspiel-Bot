using System;
using System.Collections.Generic;
using TwitchLib.Client;

namespace TwitchHandler
{
    class GuessManager
    {
        private readonly Dictionary<string, int> guesses = new Dictionary<string, int>();
        private bool running = false;
        private readonly TwitchClient twitchClient;

        public GuessManager(TwitchClient twitchClient)
        {
            this.twitchClient = twitchClient;
        }

        public void Start(string channel, string user, string[] args) {
            if (!running) {
                guesses.Clear();
            }
            running = true;
            Console.WriteLine("Awaiting guesses...");
            twitchClient.SendMessage(channel, "Tipps werden jetzt angenommen!");
        }

        public void Stop(string channel, string user, string[] args) {
            running = false;
            Console.WriteLine("Guess window closed!");
            twitchClient.SendMessage(channel, "Tipps werden nicht mehr angenmommen!");
        }

        public void Guess(string channel, string user, string[] args) {
            if (args.Length == 1)
            {
                if (running)
                {
                    guesses[user] = int.Parse(args[0].Trim());
                    Console.WriteLine($"User {user} guessed {guesses[user]}");
                }
                else
                {
                    twitchClient.SendMessage(channel, "Killspiel ist nicht aktiv!");
                }
            }
            else
            {
                twitchClient.SendMessage(channel, "Ungültige Argumentzahl!");
            }
        }

        public void Resolve(string channel, string user, string[] args) {
            if (args.Length == 1 && !running)
            {
                if (!running)
                {
                    int realKills = int.Parse(args[0].Trim());
                    List<string> winners = new List<string>();
                    foreach (var guess in guesses)
                    {
                        if (guess.Value == realKills)
                        {
                            winners.Add(guess.Key);
                        }
                    }
                    twitchClient.SendMessage(channel, "Gewonnen haben: " + string.Join(", ", winners));
                }
                else
                {
                    twitchClient.SendMessage(channel, "Killspiel läuft noch!");
                }
            }
            else
            {
                twitchClient.SendMessage(channel, "Ungültige Argumentzahl!");
            }
        }

    }
}
