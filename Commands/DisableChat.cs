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
    public class DisableChat : ICommand
    {
        public string Command => "disablechat";

        public string[] Aliases => Array.Empty<string>();

        public string Description => "Fully disables text chat.";


        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender)
            {
                response = "No";
                return false;
            }

            Player player = Player.Get(sender);

            if (!Plugin.Instance.DisabledTextChat.Contains(player.UserId))
            {
                Plugin.Instance.DisabledTextChat.Remove(player.UserId);

                response = "Re-enabled text chat!";
                return true;
            }

            Plugin.Instance.DisabledTextChat.Add(player.UserId);
            response = "Disabled text chat!";
            return true;
        }
    }
}

// please note that all of this is old code, didnt bother for shit to fix it
// literally just ctrl c ctrl v