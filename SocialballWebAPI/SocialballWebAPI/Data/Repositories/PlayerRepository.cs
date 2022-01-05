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
    public class PlayerRepository : IPlayerRepository
    {
        private readonly SocialballDBContext _context;

        public PlayerRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public object GetPlayers()
        {
            return _context.Players.Where(x => x.UserType == UserType.Player).ToList();
        }

        public List<Player> GetPlayersInTeam(Guid teamId)
        {
            return _context.Players.Where(x => x.UserType == UserType.Player && x.TeamId == teamId).ToList();
        }

        public List<UserData> GetUserDatasInTeam(Guid teamId)
        {
            return _context.UserDatas.Where(x => x.TeamId == teamId).ToList();
        }

        public List<UserData> GetManagersInTeam(Guid teamId)
        {
            return _context.UserDatas.Where(x => x.TeamId == teamId && x.UserType == UserType.Management).ToList();
        }

        public Player GetPlayerDetails(Guid id)
        {
            return _context.Players.Include(x => x.Team).Include(x => x.User).FirstOrDefault(x => x.Id == id);
        }

        public Player GetPlayerDetailsByUserId(Guid userId)
        {
            return _context.Players.Include(x => x.MatchesPlayer).ThenInclude(x => x.MatchEvents.Where(y => y.MatchEventType == MatchEventType.Goal)).Include(x => x.Team).FirstOrDefault(x => x.UserId == userId);
        }

        public UserData GetUserDataDetails(Guid id)
        {
            return _context.UserDatas.Include(x => x.User).First(x => x.Id == id);
        }

        public UserData GetUserDataDetailsByUserId(Guid userId)
        {
            return _context.UserDatas.Include(x => x.Team).FirstOrDefault(x => x.UserId == userId);
        }

        public UserData GetSystemUserDetails()
        {
            return _context.UserDatas.First(x => x.UserType == UserType.System);
        }

        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public void AddTeamManager(TeamManager teamManager)
        {
            _context.TeamManagers.Add(teamManager);
            _context.SaveChanges();
        }

        public void UpdatePlayer(Player player)
        {
            _context.Players.Update(player);
            _context.SaveChanges();
        }

        public void UpdateUserData(UserData userData)
        {
            _context.UserDatas.Update(userData);
            _context.SaveChanges();
        }
    }
}
