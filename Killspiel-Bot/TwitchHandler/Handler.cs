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

        const string Command = "!killspiel ";

        class CommandAction
        {
            public Predicate<UserType> Condition { get; }
            public Action<string, string, string[]> Action { get; }



            public CommandAction(Predicate<UserType> condition, Action<string, string, string[]> action)
            {
                Condition = condition;
                Action = action;
            }
        }

        private readonly TwitchClient twitchClient;
        private readonly GuessManager guessManager;
        private readonly Dictionary<string, CommandAction> actions;

        public Handler(string username, string accessToken, string channel)
        {
            var credentials = new ConnectionCredentials(username, accessToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var customClient = new WebSocketClient(clientOptions);
            twitchClient = new TwitchClient(customClient);
            twitchClient.Initialize(credentials, channel);
            twitchClient.OnMessageReceived += Client_OnMessageReceived;

            guessManager = new GuessManager(twitchClient);

            actions = new Dictionary<string, CommandAction> {
                ["start"] = new CommandAction(IsModOrMore, (ch, _, __) => guessManager.Start(ch)),
                ["stop"] = new CommandAction(IsModOrMore, (ch, _, __) => guessManager.Stop(ch) ),
                ["tipp"] = new CommandAction(type => true, guessManager.Guess ),
                ["kills"] = new CommandAction(IsModOrMore, (ch, _, args) => guessManager.Resolve(ch, args)),
            };

        }

        private static bool IsModOrMore(UserType type)
        {
            return type >= UserType.Moderator;
        }

        public void Connect()
        {
            twitchClient.Connect();
        }

        private void HandleCommand(string channel, string username, UserType userType, string command)
        {
            var args = command.Split(" ");
            if (args.Length > 0)
            {
                if (actions.TryGetValue(args[0], out var action))
                {
                    if (action.Condition(userType))
                    {
                        action.Action(channel, username, args.Skip(1).ToArray());
                    }
                    else
                    {
                        twitchClient.SendMessage(channel, Messages.InsufficientPermissions);
                    }
                }
                else
                {
                    twitchClient.SendMessage(channel, Messages.InvalidCommand);
                }
            }
            else
            {
                twitchClient.SendMessage(channel, Messages.InvalidCommand);
            }
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message;
            var channel = e.ChatMessage.Channel;
            var username = e.ChatMessage.Username;
            var userType = e.ChatMessage.UserType;
            if (message.StartsWith(Command))
            {
                HandleCommand(channel, username, userType, message.Substring(Command.Length));
            }
        }

    }
}