using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
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
        public PositionType? Position { get; set; }
        public Guid? TeamId { get; set; }
        public Guid? UserId { get; set; }
        public string Citizenship { get; set; }
        public string Email { get; set; }
        public string TeamName { get; set; }
        public string Image { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<GoalInPlayerDetailsDto> Goals { get; set; }
        public List<GoalInPlayerDetailsDto> Assists { get; set; }
        public object CurrentYearGoalsToChart { get; set; }
    }
}
