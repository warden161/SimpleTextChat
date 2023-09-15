using System;
using System.Linq;
using System.Collections.Generic;

using CommandSystem;
using RemoteAdmin;
using UnityEngine;
using PlayerStatsSystem;
using Player = Exiled.API.Features.Player;
using Exiled.API.Enums;
using Exiled.API.Features;

namespace SimpleTextChat.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ScpChat : ICommand
    {
        public string Command => "scpchat";

        public string[] Aliases => new string[] { "schat" };

        public string Description => "Sends a message.";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender)
            {
                response = "Must send from client";
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Command usage: .scpchat <message>";
                return false;
            }

            var message = string.Join(" ", arguments);
            Player player = Player.Get(sender);

            if (player.IsMuted)
            {
                response = "You are muted.";
                return false;
            }

            if (!player.IsScp)
            {
                response = "You're not an SCP.";
                return false;
            }

            float duration = 3;
            if (message.Length > Plugin.Instance.Config.MaxCharacters)
            {
                response = $"Too long message (exceeded {Plugin.Instance.Config.MaxCharacters} characters).";
                return false;
            }

            if (message.Length > 30)
                duration = message.Length / 10;

            var display = $"<b><color={player.Role.Color.ToHex()}>{player.Nickname}</color></b> (<color={player.Role.Color.ToHex()}>{player.Role.Name}</color>)\n";

            foreach (Player scp in Player.Get(Side.Scp))
            {
                if (Plugin.Instance.HasPlayerMuted(scp, player))
                    continue;

                scp.ShowHint($"[<b><color=red>SCP Chat</color></b>]\n{display}\n{message.Replace("<size", "")}\n<size=15>You can mute a player using <color=red>.mute playername</color> in your console or <color=red>.disablechat</color></size>", duration);
            }

            response = "Sent!";
            return true;
        }
    }
}

// please note that all of this is old code, didnt bother for shit to fix it
// literally just ctrl c ctrl v