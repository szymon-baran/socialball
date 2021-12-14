using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Enums
{
    public enum PenaltyType
    {
        [Display(Name = "Brak")]
        None = 0,
        [Display(Name = "Żółta kartka")]
        YellowCard = 1,
        [Display(Name = "Czerwona kartka")]
        RedCard = 2,
    }

}
