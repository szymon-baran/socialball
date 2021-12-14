using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Enums
{
    public enum MatchType
    {
        [Display(Name = "Mecz towarzyski")]
        FriendlyMatch = 0,
        [Display(Name = "Mecz ligowy")]
        LeagueMatch = 1
    }
}
