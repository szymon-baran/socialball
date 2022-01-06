using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class JobAdvertisementAnswerRepository : IJobAdvertisementAnswerRepository
    {
        private readonly SocialballDBContext _context;

        public JobAdvertisementAnswerRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public List<JobAdvertisementUserAnswer> GetJobAdvertisementUserAnswersByTeam(Guid? teamId)
        {
            return _context.JobAdvertisementUserAnswers.Include(x => x.User).ThenInclude(y => y.UserData).Where(x => x.TeamId == teamId || (x.JobAdvertisement is FromTeamJobAdvertisement ? ((FromTeamJobAdvertisement)x.JobAdvertisement).TeamId == teamId : false)).ToList();
        }

        public List<JobAdvertisementTeamAnswer> GetJobAdvertisementTeamAnswersByUser(Guid? userId)
        {
            return _context.JobAdvertisementTeamAnswers.Where(x => x.JobAdvertisement is FromUserJobAdvertisement ? ((FromUserJobAdvertisement)x.JobAdvertisement).UserId == userId : false).ToList();
        }

        public JobAdvertisementAnswer GetJobAdvertisementAnswerDetails(Guid id)
        {
            JobAdvertisementAnswer jobAdvertisementAnswer = _context.JobAdvertisementAnswers.First(x => x.Id == id);
            if (jobAdvertisementAnswer.JobAdvertisementId.HasValue)
            {
                return _context.JobAdvertisementAnswers.Include(x => x.JobAdvertisement).First(x => x.Id == id);
            }
            else
            {
                return jobAdvertisementAnswer;
            }
        }

        public JobAdvertisementTeamAnswer GetJobAdvertisementTeamAnswerDetails(Guid id)
        {
            return _context.JobAdvertisementTeamAnswers.Include(x => x.Team).First(x => x.Id == id);
        }

        public JobAdvertisementUserAnswer GetJobAdvertisementUserAnswerDetails(Guid id)
        {
            return _context.JobAdvertisementUserAnswers.Include(x => x.User).ThenInclude(x => x.UserData).First(x => x.Id == id);
        }

        public void AddJobAdvertisementUserAnswer(JobAdvertisementUserAnswer jobAdvertisementUserAnswer)
        {
            _context.JobAdvertisementUserAnswers.Add(jobAdvertisementUserAnswer);
            _context.SaveChanges();
        }

        public void AddJobAdvertisementTeamAnswer(JobAdvertisementTeamAnswer jobAdvertisementTeamAnswer)
        {
            _context.JobAdvertisementTeamAnswers.Add(jobAdvertisementTeamAnswer);
            _context.SaveChanges();
        }

        public void UpdateJobAdvertisementAnswer(JobAdvertisementAnswer jobAdvertisementAnswer)
        {
            _context.JobAdvertisementAnswers.Update(jobAdvertisementAnswer);
            _context.SaveChanges();
        }
    }
}
