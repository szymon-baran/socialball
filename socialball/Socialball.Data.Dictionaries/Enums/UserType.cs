using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Enums
{
    public enum UserType
    {
        [Display(Name = "Zawodnik")]
        Player = 1,
        [Display(Name = "Sztab")]
        Management = 2,
        [Display(Name = "Administrator")]
        Admin = 10,
        [Display(Name = "System")]
        System = 999
    }
}
