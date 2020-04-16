using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client;

namespace TwitchHandler
{
    class GuessManager
    {
        private readonly Dictionary<string, int> guesses = new Dictionary<string, int>();
        private bool running;
        private readonly TwitchClient twitchClient;

        public GuessManager(TwitchClient twitchClient)
        {
            this.twitchClient = twitchClient;
        }

        public void Start(string channel) {
            if (!running) {
                guesses.Clear();
            }
            running = true;
            Console.WriteLine("Awaiting guesses...");
            twitchClient.SendMessage(channel, Messages.GameStarted);
        }

        public void Stop(string channel) {
            running = false;
            Console.WriteLine("Guess window closed!");
            twitchClient.SendMessage(channel, Messages.GameStopped);
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
                    twitchClient.SendMessage(channel, Messages.GameInactive);
                }
            }
            else
            {
                twitchClient.SendMessage(channel, Messages.InvalidArgCount);
            }
        }

        public void Resolve(string channel, string[] args) {
            if (args.Length == 1 && !running)
            {
                if (!running)
                {
                    var realKills = int.Parse(args[0].Trim());
                    var winners = guesses
                        .Where(guess => guess.Value == realKills)
                        .Select(guess => guess.Key)
                        .ToList();

                    twitchClient.SendMessage(channel, Messages.Winners(winners));
                }
                else
                {
                    twitchClient.SendMessage(channel, Messages.GameActive);
                }
            }
            else
            {
                twitchClient.SendMessage(channel, Messages.InvalidArgCount);
            }
        }
    }
}
