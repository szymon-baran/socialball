using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public abstract class UserData
    {
        public Guid Id { get; set; }
        public UserType UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? TeamId { get; set; }
        public string Citizenship { get; set; }
        public int? Earnings { get; set; }
        public Guid? UserId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public virtual Team Team { get; set; }
        public virtual User User { get; set; }
    }
}
