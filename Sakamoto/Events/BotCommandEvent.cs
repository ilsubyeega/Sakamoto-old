using HOPEless.Bancho;
using HOPEless.Bancho.Objects;
using Sakamoto.Manager;
using Sakamoto.Objects;
using Sakamoto.Util;
using System;

namespace Sakamoto.Events
{
	public static class BotCommandEvent
	{
		private static Random rnd = new Random();
		public static void Handle(User u, BanchoChatMessage message)
		{
			string msg = message.Message.Substring(1);
			string command = msg.Split(' ')[0].ToLower();
			string arg = msg.Split(' ').Length > 1 ? msg.Remove(0, msg.IndexOf(' ') + 1) : "";
			// Debug
			Console.WriteLine($"{u.username} issued command: {command} {arg}");

			Type class_type = Type.GetType($"Sakamoto.Events.BotCommandEvent");
			if (class_type == null || class_type.GetMethod(command) == null)
				return;

			Tuple<string, bool> result = (Tuple<string, bool>)class_type.GetMethod(command).Invoke(null, new object[] { u, arg });
			if (result.Item2 == true)
			{
				if (result.Item1.StartsWith("#"))
					ChatManager.SendMessage(message.Channel, u, result.Item1);
				return;
			}
			u.AddQueue(new BanchoPacket(
				PacketType.ServerChatMessage,
				new BanchoChatMessage(
					"Sakamoto",
					result.Item1,
					message.Channel, 2)
				)
			);
			return;
		}
		// The below is command.
		public static Tuple<string, bool> hi(User u, string arg) => hello(u, arg);
		public static Tuple<string, bool> 안녕(User u, string arg) => hello(u, arg);
		public static Tuple<string, bool> hello(User u, string arg)
		{
			return new Tuple<string, bool>($"Hello, {u.username}!", false);
		}
		public static Tuple<string, bool> roll(User u, string arg) => dice(u, arg);
		public static Tuple<string, bool> dice(User u, string arg)
		{
			ulong a = 100;
			if (arg != null)
				try
				{
					a = ulong.Parse(arg);
				}
				catch (OverflowException)
				{
					a = ulong.MaxValue;
				}
				catch (Exception) { }
			return new Tuple<string, bool>($"You rolled {rnd.NextULong(0, a)}", false);
		}
	}
}
