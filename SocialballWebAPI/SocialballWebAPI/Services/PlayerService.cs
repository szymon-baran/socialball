using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService UserService;

        public PlayerService(SocialballDBContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            UserService = userService;
        }

        public object GetPlayers()
        {
            return _context.Players.Where(x => x.UserType == UserType.Zawodnik).ToList();
        }

        public object GetPlayersByTeamId(Guid teamId)
        {
            return _context.Players.Where(x => x.UserType == UserType.Zawodnik && x.TeamId == teamId).ToList();
        }

        public GetPlayerDto GetPlayerDetails(Guid id)
        {
            Player player = _context.Players.Include(x => x.MatchEvents.Where(y => y.MatchEventType == MatchEventType.Goal)).FirstOrDefault(x => x.Id == id);
            if (player == null)
            {
                throw new KeyNotFoundException();
            }
            GetPlayerDto model = _mapper.Map<GetPlayerDto>(player);

            return model;
        }

        public GetPlayerDto GetPlayerDetailsByUserId(Guid userId)
        {
            Player player = _context.Players.Include(x => x.MatchEvents.Where(y => y.MatchEventType == MatchEventType.Goal)).FirstOrDefault(x => x.UserId == userId);
            if (player == null)
            {
                throw new KeyNotFoundException();
            }
            GetPlayerDto model = _mapper.Map<GetPlayerDto>(player);

            return model;
        }

        public UserData GetUserDataByUserId(Guid userId)
        {
            UserData userData = _context.UserDatas.FirstOrDefault(x => x.UserId == userId);
            if (userData == null)
            {
                throw new KeyNotFoundException();
            }
            return userData;
        }

        public void AddPlayer(RegisterPlayerDto playerModel)
        {
            Guid userId = UserService.AddUserAccountForNewPlayer(playerModel);

            Player player = new Player()
            {
                FirstName = playerModel.FirstName,
                LastName = playerModel.LastName,
                Position = playerModel.Position,
                TeamId = playerModel.TeamId,
                Citizenship = playerModel.Citizenship,
                UserId = userId,
                UserType = UserType.Zawodnik
            };
            _context.Players.Add(player);
            _context.SaveChanges();

            // TODO:
            //if (playerModel.AddJobAdvertisement == true) 
            //{ 
            //    FromUserJobAdvertisement jobAdvertisement = new FromUserJobAdvertisement()
            //    {
            //        JobAdvertisementType = JobAdvertisementType.FromUser,
            //    };                
            //}
        }

        public bool CheckUsernameUniqueness(string username)
        {
            return !_context.Users.Any(x => x.Username == username);
        }

    }
}
