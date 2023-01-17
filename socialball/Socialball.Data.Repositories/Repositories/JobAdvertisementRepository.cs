using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class JobAdvertisementRepository : IJobAdvertisementRepository
    {
        private readonly SocialballDBContext _context;

        public JobAdvertisementRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public List<FromTeamJobAdvertisement> GetFromTeamJobAdvertisementsByPosition(PositionType? positionType)
        {
            return _context.FromTeamJobAdvertisements.Include(x => x.JobAdvertisementAnswers).Where(x => x.Position == positionType && x.IsActive == true).ToList();
        }

        public List<FromTeamJobAdvertisement> GetFromTeamJobAdvertisementsByTeamId(Guid? teamId)
        {
            return _context.FromTeamJobAdvertisements.Where(x => x.TeamId == teamId).ToList();
        }

        public List<FromUserJobAdvertisement> GetFromUserJobAdvertisements()
        {
            return _context.FromUserJobAdvertisements.Include(x => x.User).ThenInclude(x => x.UserData).Include(x => x.JobAdvertisementAnswers).Where(x => x.IsActive == true).ToList();
        }

        public List<FromUserJobAdvertisement> GetFromUserJobAdvertisementsByUserId(Guid userId)
        {
            return _context.FromUserJobAdvertisements.Where(x => x.UserId == userId).ToList();
        }

        public JobAdvertisement GetJobAdvertisementDetails(Guid id)
        {
            return _context.JobAdvertisements.Single(x => x.Id == id);
        }

        public FromTeamJobAdvertisement GetFromTeamJobAdvertisementDetails(Guid id)
        {
            return _context.FromTeamJobAdvertisements.Include(x => x.Team).Single(x => x.Id == id);
        }

        public FromUserJobAdvertisement GetFromUserJobAdvertisementDetails(Guid id)
        {
            return _context.FromUserJobAdvertisements.Include(x => x.User).ThenInclude(x => x.UserData).Single(x => x.Id == id);
        }

        public FromUserJobAdvertisement GetFromUserJobAdvertisementDetailsByUserId(Guid userId)
        {
            return _context.FromUserJobAdvertisements.Include(x => x.User).ThenInclude(x => x.UserData).SingleOrDefault(x => x.UserId == userId) ?? new FromUserJobAdvertisement();
        }

        public void AddFromUserJobAdvertisement(FromUserJobAdvertisement fromUserJobAdvertisement)
        {
            _context.FromUserJobAdvertisements.Add(fromUserJobAdvertisement);
            _context.SaveChanges();
        }

        public void UpdateFromUserJobAdvertisement(FromUserJobAdvertisement fromUserJobAdvertisement)
        {
            _context.FromUserJobAdvertisements.Update(fromUserJobAdvertisement);
            _context.SaveChanges();
        }

        public void AddFromTeamJobAdvertisement(FromTeamJobAdvertisement fromTeamJobAdvertisement)
        {
            _context.FromTeamJobAdvertisements.Add(fromTeamJobAdvertisement);
            _context.SaveChanges();
        }

        public void EditFromTeamJobAdvertisement(FromTeamJobAdvertisement fromTeamJobAdvertisement)
        {
            _context.FromTeamJobAdvertisements.Update(fromTeamJobAdvertisement);
            _context.SaveChanges();
        }

        public void RemoveFromUserJobAdvertisementsRange(List<FromUserJobAdvertisement> fromUserJobAdvertisements)
        {
            _context.FromUserJobAdvertisements.RemoveRange(fromUserJobAdvertisements);
            _context.SaveChanges();
        }

    }
}
