using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Services
{
    public class TeamService : ITeamService
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;

        public TeamService(SocialballDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public object GetTeams()
        {
            return _context.Teams.Include(x => x.League).ToList();
        }

        public List<SelectList> GetTeamsToSelectList()
        {
            return _context.Teams.Select(x => new SelectList { Id = x.Id, Name = x.Name }).ToList();
        }

        public List<League> GetLeaguesToLookup()
        {
            return _context.Leagues.ToList();
        }

        public List<PositionsInTeam> GetTeamsToChart(Guid teamId)
        {
            List<PositionsInTeam> positionsInTeams = new List<PositionsInTeam>();

            for (PositionType p = 0; p <= PositionType.Napastnik; p++)
            {
                positionsInTeams.Add(new PositionsInTeam
                {
                    Position = p,
                    NumberOfPlayers = _context.Players.Where(x => x.TeamId == teamId && x.Position == (int)p).Count()
                });
            }

            return positionsInTeams;
        }

        public TeamDto GetTeamDetails(Guid id)
        {
            Team team = _context.Teams.Include(x => x.League).FirstOrDefault(x => x.Id == id);
            if (team == null)
            {
                throw new KeyNotFoundException();
            }

            TeamDto model = _mapper.Map<TeamDto>(team);
            if (team.League != null)
            {
                model.LeagueName = team.League.Name;
            }

            return model;
        }
    }
}
