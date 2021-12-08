using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs
{
    public class MatchAnswerDto
    {
        public Guid Id { get; set; }
        public bool IsAccepted { get; set; }
    }
}
