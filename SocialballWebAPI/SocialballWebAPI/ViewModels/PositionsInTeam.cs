using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.ViewModels
{
    public partial class PositionsInTeam
    {
        public PositionType Position { get; set; }
        public int NumberOfPlayers { get; set; }
    }

}
