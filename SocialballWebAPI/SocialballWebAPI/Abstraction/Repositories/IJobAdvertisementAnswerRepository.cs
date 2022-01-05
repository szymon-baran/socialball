using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IJobAdvertisementAnswerRepository
    {
        List<JobAdvertisementUserAnswer> GetJobAdvertisementUserAnswersByTeam(Guid? teamId);
        List<JobAdvertisementTeamAnswer> GetJobAdvertisementTeamAnswersByUser(Guid? userId);
        JobAdvertisementAnswer GetJobAdvertisementAnswerDetails(Guid id);
        JobAdvertisementTeamAnswer GetJobAdvertisementTeamAnswerDetails(Guid id);
        JobAdvertisementUserAnswer GetJobAdvertisementUserAnswerDetails(Guid id);
        void AddJobAdvertisementUserAnswer(JobAdvertisementUserAnswer jobAdvertisementUserAnswer);
        void AddJobAdvertisementTeamAnswer(JobAdvertisementTeamAnswer jobAdvertisementTeamAnswer);
        void UpdateJobAdvertisementAnswer(JobAdvertisementAnswer jobAdvertisementAnswer);
    }
}
