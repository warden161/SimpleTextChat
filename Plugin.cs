using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTextChat
{
    public class Plugin : Plugin<Config>
    {
        public override string Author { get; } = "warden161";
        public override string Name { get; } = "Simple Text Chat";
        public override Version Version { get; } = new(0, 1, 0);
        public override Version RequiredExiledVersion { get; } = new(8, 2, 1);

        public static Plugin Instance;
        public Dictionary<string, List<string>> MutedPlayers = new();
        public List<string> DisabledTextChat = new();

        public override void OnEnabled()
            => Instance = this;

        public bool HasPlayerMuted(Player ply, Player target)
            => DisabledTextChat.Contains(ply.UserId) || (MutedPlayers.TryGetValue(ply.UserId, out var players) && players.Any(x => x == target.UserId));
    }
}
