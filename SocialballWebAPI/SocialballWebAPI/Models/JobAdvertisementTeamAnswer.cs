﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class JobAdvertisementTeamAnswer : JobAdvertisementAnswer
    {
        public Guid TeamId { get; set; }
        public Team Team { get; set; }
    }
}