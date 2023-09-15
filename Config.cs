using Exiled.API.Interfaces;
using PlayerRoles;
using System.Collections.Generic;

namespace SimpleTextChat
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }

        public List<RoleTypeId> BlacklistedProximityChat { get; set; } = new() { RoleTypeId.Scp079 };
        public int MaxCharacters { get; set; } = 120;
        public float ProximityRange { get; set; } = 8;
    }
}
