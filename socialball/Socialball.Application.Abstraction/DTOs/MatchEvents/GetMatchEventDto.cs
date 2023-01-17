using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.DTOs.MatchEvents
{
    public class GetMatchEventDto
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AssistPlayerFirstName { get; set; }
        public string AssistPlayerLastName { get; set; }
        public int Minute { get; set; }
        public MatchEventType MatchEventType { get; set; }
        public PenaltyType? PenaltyType { get; set; }
    }
}
