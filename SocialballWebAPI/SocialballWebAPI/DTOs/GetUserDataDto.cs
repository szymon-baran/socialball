using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class GetUserDataDto
    {
        public Guid Id { get; set; }
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? TeamId { get; set; }
        public string Citizenship { get; set; }
        public Guid? UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Image { get; set; }
        public virtual Team Team { get; set; }
    }
}
