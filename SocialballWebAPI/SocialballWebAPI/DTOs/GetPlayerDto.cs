﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class GetPlayerDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Position { get; set; }
        public Guid? TeamId { get; set; }
        public string Citizenship { get; set; }
        public string Email { get; set; }
    }
}