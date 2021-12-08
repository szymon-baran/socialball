using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class UserJobAdvertisementDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
