using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace TwitchHandler
{
	public class Handler
    {

        const string command = "!killspiel ";

        class CommandAction
        {
            public Predicate<UserType> Condition { get; set; }
            public Action<string, string, string[]> Action { get; set; }
        }

        private readonly TwitchClient twitchClient;
        private readonly GuessManager guessManager;
        private readonly Dictionary<string, CommandAction> actions;

        Handler(string username, string accessToken, string channel)
        {
            ConnectionCredentials credentials = new ConnectionCredentials(username, accessToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            twitchClient = new TwitchClient(customClient);
            twitchClient.Initialize(credentials, channel);
            twitchClient.OnMessageReceived += Client_OnMessageReceived;

            guessManager = new GuessManager(twitchClient);

            actions = new Dictionary<string, CommandAction> {
                ["start"] = new CommandAction { Condition = type => type >= UserType.Moderator, Action = guessManager.Start},
                ["stop"] = new CommandAction { Condition = type => type >= UserType.Moderator, Action = guessManager.Stop },
                ["tipp"] = new CommandAction { Condition = type => true, Action = guessManager.Guess },
                ["kills"] = new CommandAction { Condition = type => type >= UserType.Moderator, Action = guessManager.Resolve },
            };

            twitchClient.Connect();
        }

        private void HandleCommand(string channel, string username, UserType userType, string command)
        {
            string[] args = command.Split(" ");
            if (args.Length > 0)
            {
                CommandAction action;
                if (actions.TryGetValue(args[0], out action))
                {
                    if (action.Condition(userType))
                    {
                        action.Action(channel, username, args.Skip(1).ToArray());
                    }
                    else
                    {
                        twitchClient.SendMessage(channel, "Keine Berechtigung!");
                    }
                }
                else
                {
                    twitchClient.SendMessage(channel, "Ungültiger Befehl!");
                }
            }
            else
            {
                twitchClient.SendMessage(channel, "Ungültiger Befehl!");
            }
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message;
            var channel = e.ChatMessage.Channel;
            var username = e.ChatMessage.Username;
            var userType = e.ChatMessage.UserType;
            if (message.StartsWith(command))
            {
                HandleCommand(channel, username, userType, message.Substring(command.Length));
            }
        }

    }
}