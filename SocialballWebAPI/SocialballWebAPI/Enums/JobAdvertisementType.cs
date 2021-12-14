using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Enums
{
    public enum JobAdvertisementType
    {
        [Display(Name = "Od drużyny")]
        FromTeam = 1,
        [Display(Name = "Od użytkownika")]
        FromUser = 2
    }
}
