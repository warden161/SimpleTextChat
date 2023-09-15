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
using Exiled.API.Features.Roles;

namespace SimpleTextChat.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ProximityChat : ICommand
    {
        public string Command => "chat";

        public string[] Aliases => new string[] { "proximitychat" };

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
                response = "Command usage: .chat <message>";
                return false;
            }

            var message = string.Join(" ", arguments);
            Player chatter = Player.Get(sender);
            float duration = 3;

            if (chatter.IsMuted)
            {
                response = "You are muted.";
                return false;
            }

            if (message.Length > Plugin.Instance.Config.MaxCharacters)
            {
                response = $"Too long message (exceeded {Plugin.Instance.Config.MaxCharacters} characters).";
                return false;
            }

            if (message.Length > 30)
                duration = message.Length / 10;

            if (!chatter.IsAlive)
            {
                foreach (Player player in Player.List.Where(x => x.IsDead))
                {
                    if (Plugin.Instance.HasPlayerMuted(player, chatter))

                    player.Broadcast((ushort)duration, message: $"<size=25>[<b><color=orange>Spectator Chat</color></b>] <b><color=yellow>{chatter.Nickname}</color></b></size>\n<size=35>{message.Replace("<size", "")}</size>\n\n<size=15>You can mute a player using <color=red>.mute playername</color> in your console or <color=red>.disablechat</color></size>");
                }

                response = "Sent to spectator chat!";
                return false;
            }

            var display = $"<color={chatter.Role.Color.ToHex()}>{chatter.Nickname}</color>";

            if (chatter.Role.Type == PlayerRoles.RoleTypeId.Scp939)
                display = $"<color=#FF8E00FF>{chatter.Nickname}</color>";

            foreach (Player player in Player.List)
            {
                if (Plugin.Instance.HasPlayerMuted(player, chatter))
                    continue;

                if (MeetsProximityCondition(player, chatter) || (player.Role.Is(out SpectatorRole role) && role.SpectatedPlayer != null && MeetsProximityCondition(role.SpectatedPlayer, chatter)))
                    player.ShowHint($"[<b><color=yellow>Proximity Chat</color></b>] <b>{display}</b>\n{message.Replace("<size", "")}\n\n<size=15>You can mute a player using <color=red>.mute playername</color> in your console or <color=red>.disablechat</color></size>", duration);
            }

            response = "Sent!";
            return true;
        }

        public bool MeetsProximityCondition(Player player, Player chatter)
            => player.IsAlive && Vector3.Distance(player.Position, chatter.Position) < 8;
    }
}
// please note that all of this is old code
// i would never write something like this
// literally just ctrl c ctrl v