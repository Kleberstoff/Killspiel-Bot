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
		private static readonly string Command = "!killspiel ";

		private readonly TwitchClient TwitchClient;
		private readonly GuessManager GuessManager;

		private readonly Dictionary<string, CommandAction> Actions;

		public Handler(string username, string accessToken, string channel)
		{
			ConnectionCredentials credentials = new ConnectionCredentials(username, accessToken);
			ClientOptions clientOptions = new ClientOptions
			{
				MessagesAllowedInPeriod = 750,
				ThrottlingPeriod = TimeSpan.FromSeconds(30)
			};

			WebSocketClient customClient = new WebSocketClient(clientOptions);

			TwitchClient = new TwitchClient(customClient);
			TwitchClient.Initialize(credentials, channel);

			TwitchClient.OnMessageReceived += Client_OnMessageReceived;

			GuessManager = new GuessManager(TwitchClient);

			Actions = new Dictionary<string, CommandAction>
			{
				["start"] = new CommandAction(IsModOrAbove, (ch, _, __) => GuessManager.Start(ch)),
				["stop"] = new CommandAction(IsModOrAbove, (ch, _, __) => GuessManager.Stop(ch)),
				["tipp"] = new CommandAction(type => true, (ch, user, args) => GuessManager.Guess(ch, user, args)),
				["kills"] = new CommandAction(IsModOrAbove, (ch, _, args) => GuessManager.Resolve(ch, args)),
			};
		}

		private static bool IsModOrAbove(UserType type)
			=> type >= UserType.Moderator;

		public void Connect()
			=> TwitchClient.Connect();

		private void HandleCommand(string channel, string username, UserType userType, string command)
		{
			string[] args = command.Split(" ");

			if (args.Length == 0)
				return;

            if (!Actions.TryGetValue(args[0], out CommandAction action))
				return;

			if (!action.Condition(userType))
			{
				TwitchClient.SendMessage(channel, Messages.InsufficientPermissions);
				return;
			}

			action.Action(channel, username, args.Skip(1).ToArray());
		}

		private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
		{
			string message = e.ChatMessage.Message;
			string channel = e.ChatMessage.Channel;
			string username = e.ChatMessage.Username;
			UserType userType = e.ChatMessage.UserType;

			if (message.StartsWith(Command))
				HandleCommand(channel, username, userType, message.Substring(Command.Length));
		}
	}
}