using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public abstract class JobAdvertisement
    {
        public JobAdvertisement()
        {
            JobAdvertisementAnswers = new HashSet<JobAdvertisementAnswer>();
        }

        public Guid Id { get; set; }
        public JobAdvertisementType JobAdvertisementType { get; set; }
        public string Location { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public int? Earnings { get; set; }

        public virtual ICollection<JobAdvertisementAnswer> JobAdvertisementAnswers { get; set; }
    }
}
