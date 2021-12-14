using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Enums
{
    public enum MessageType
    {
        [Display(Name = "Prywatna")]
        Private = 0,
        [Display(Name = "Do swojej drużyny")]
        InsideTeam = 1,
        [Display(Name = "Do zarządu innej drużyny")]
        ToOtherTeam = 2
    }
}
