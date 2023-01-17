using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Enums
{
    public enum MatchEventType
    {
        [Display(Name = "Gol")]
        Goal = 1,
        [Display(Name = "Faul")]
        Foul = 2,
    }

}
