﻿using AutoMapper;
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
    public class MatchService : IMatchService
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;

        public MatchService(SocialballDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public object GetAllMatches()
        {
            return _context.Matches.Where(x => x.IsConfirmed).Include(x => x.MatchEvents).ThenInclude(x => x.Player).ToList();
        }

        public object GetTeamMatches(Guid teamId)
        {
            return _context.Matches.Where(x => x.IsConfirmed && (x.HomeTeamId == teamId || x.AwayTeamId == teamId)).Include(x => x.MatchEvents).ThenInclude(x => x.Player).ToList();
        }

        public object GetUnconfirmedMatches(Guid teamId)
        {
            return _context.Matches.Where(x => !x.IsConfirmed && (x.HomeTeamId == teamId || x.AwayTeamId == teamId)).Include(x => x.MatchEvents).ThenInclude(x => x.Player).Include(x => x.HomeTeam).Include(x => x.AwayTeam).Select(x => new
            {
                x.Id,
                x.HomeTeamId,
                x.HomeTeam,
                x.AwayTeamId,
                x.AwayTeam,
                x.MatchEvents,
                x.AddedByTeamId,
                x.Stadium,
                x.DateTime,
                IsUnconfirmedByYourTeam = x.AddedByTeamId == teamId ? false : true
            }).ToList();
        }

        public Match GetMatchDetails(Guid id)
        {
            Match match = _context.Matches
                    .Include(x => x.MatchEvents)
                        .ThenInclude(x => (x as MatchEventGoal).AssistPlayer)
                    .Include(x => x.MatchEvents)
                        .ThenInclude(x => x.Player)
                    .Include(x => x.AwayTeam)
                        .ThenInclude(x => x.League)
                    .Include(x => x.HomeTeam)
                        .ThenInclude(x => x.League)
                .FirstOrDefault(x => x.Id == id);

            if (match == null)
            {
                throw new KeyNotFoundException();
            }

            return match;
        }

        public void AddMatch(MatchDto model)
        {
            Match match = new Match()
            {
                HomeTeamId = model.HomeTeamId,
                AwayTeamId = model.AwayTeamId,
                Stadium = model.Stadium,
                DateTime = model.DateTime.AddHours(1),
                IsConfirmed = false,
                AddedByTeamId = model.AddedByTeamId
            };

            _context.Matches.Add(match);
            _context.SaveChanges();

            foreach (var eventModel in model.MatchEvents)
            {
                switch (eventModel.MatchEventType)
                {
                    case MatchEventType.Goal:
                        MatchEventGoal matchEventGoal = new MatchEventGoal()
                        {
                            MatchId = match.Id,
                            PlayerId = eventModel.PlayerId,
                            Minute = eventModel.Minute,
                            MatchEventType = eventModel.MatchEventType,
                            AssistPlayerId = eventModel.AssistPlayerId
                        };
                        _context.MatchEventGoals.Add(matchEventGoal);
                        break;
                    case MatchEventType.Foul:
                        MatchEventFoul matchEventFoul = new MatchEventFoul()
                        {
                            MatchId = match.Id,
                            PlayerId = eventModel.PlayerId,
                            Minute = eventModel.Minute,
                            MatchEventType = eventModel.MatchEventType,
                            PenaltyType = eventModel.PenaltyType
                        };
                        _context.MatchEventFouls.Add(matchEventFoul);
                        break;
                }
            }
            _context.SaveChanges();
        }

        public void SendMatchAnswer(MatchAnswerDto model)
        {
            Match match = _context.Matches.Single(x => x.Id == model.Id);

            if (model.IsAccepted)
            {
                match.IsConfirmed = model.IsAccepted;
            } 
            else
            {
                _context.Matches.Remove(match);
            }

            _context.SaveChanges();
        }
    }
}
