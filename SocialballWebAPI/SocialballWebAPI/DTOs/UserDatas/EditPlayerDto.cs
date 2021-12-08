using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class EditPlayerDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PositionType? Position { get; set; }
        public string Citizenship { get; set; }
        public string Email { get; set; }
        public byte[] Image { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
