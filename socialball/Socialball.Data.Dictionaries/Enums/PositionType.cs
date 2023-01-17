using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Enums
{
    public enum PositionType
    {
        [Display(Name = "Bramkarz")]
        Goalkeeper = 0,
        [Display(Name = "Obrońca")]
        Defender = 1,
        [Display(Name = "Pomocnik")]
        Midfielder = 2,
        [Display(Name = "Napastnik")]
        Striker = 3
    }

}
