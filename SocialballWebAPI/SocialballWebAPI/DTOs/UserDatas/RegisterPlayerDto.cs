using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class RegisterPlayerDto
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Position { get; set; }
        public Guid? TeamId { get; set; }
        public string Citizenship { get; set; }
        public string Email { get; set; }
        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] Image { get; set; }
    }
}
