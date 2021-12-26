using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.DTOs.MatchEvents;
using SocialballWebAPI.DTOs.MatchPlayers;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            return _context.Matches.Where(x => x.IsConfirmed).Include(x => x.MatchPlayers).ThenInclude(x => x.MatchEvents).Include(x => x.MatchPlayers).ThenInclude(x => x.Player).ToList();
        }

        public object GetTeamMatches(Guid teamId)
        {
            return _context.Matches.Where(x => x.IsConfirmed && (x.HomeTeamId == teamId || x.AwayTeamId == teamId)).Include(x => x.MatchPlayers).ThenInclude(x => x.MatchEvents).Include(x => x.MatchPlayers).ThenInclude(x => x.Player).ToList();
        }

        public object GetUnconfirmedMatches(Guid teamId)
        {
            return _context.Matches.Where(x => !x.IsConfirmed && (x.HomeTeamId == teamId || x.AwayTeamId == teamId)).Include(x => x.MatchPlayers).ThenInclude(x => x.MatchEvents).Include(x => x.MatchPlayers).ThenInclude(x => x.Player).Include(x => x.HomeTeam).Include(x => x.AwayTeam).Select(x => new
            {
                x.Id,
                x.HomeTeamId,
                x.HomeTeam,
                x.AwayTeamId,
                x.AwayTeam,
                x.MatchPlayers,
                x.AddedByTeamId,
                x.Stadium,
                x.DateTime,
                IsUnconfirmedByYourTeam = x.AddedByTeamId == teamId ? false : true
            }).ToList();
        }

        public MatchDetailsDto GetMatchDetails(Guid id)
        {
            Match match = _context.Matches
            .Include(x => x.AwayTeam)
                .ThenInclude(x => x.League)
            .Include(x => x.HomeTeam)
                .ThenInclude(x => x.League)
            .Single(x => x.Id == id);

            if (match == null)
            {
                throw new KeyNotFoundException();
            }

            MatchDetailsDto model = _mapper.Map<MatchDetailsDto>(match);
            List<GetMatchPlayerDto> homeMatchPlayers = _context.MatchPlayers.Include(x => x.Player).Where(x => x.MatchId == match.Id && x.TeamId == match.HomeTeamId).Select(x => new GetMatchPlayerDto
            {
                Id = x.Id,
                PlayerId = x.PlayerId,
                FirstName = x.Player.FirstName,
                LastName = x.Player.LastName,
                Position = x.Position,
                Number = x.Number.HasValue ? x.Number.Value : 0
            }).ToList();
            model.HomeMatchGoalkeeper = homeMatchPlayers.Single(x => x.Position == PositionType.Goalkeeper);
            model.HomeMatchDefenders = homeMatchPlayers.Where(x => x.Position == PositionType.Defender).ToList();
            model.HomeMatchMidfielders = homeMatchPlayers.Where(x => x.Position == PositionType.Midfielder).ToList();
            model.HomeMatchStrikers = homeMatchPlayers.Where(x => x.Position == PositionType.Striker).ToList();

            List<GetMatchPlayerDto> awayMatchPlayers = _context.MatchPlayers.Include(x => x.Player).Where(x => x.MatchId == match.Id && x.TeamId == match.AwayTeamId).Select(x => new GetMatchPlayerDto
            {
                Id = x.Id,
                PlayerId = x.PlayerId,
                FirstName = x.Player.FirstName,
                LastName = x.Player.LastName,
                Position = x.Position,
                Number = x.Number.HasValue ? x.Number.Value : 0
            }).ToList();
            model.AwayMatchGoalkeeper = awayMatchPlayers.Single(x => x.Position == PositionType.Goalkeeper);
            model.AwayMatchDefenders = awayMatchPlayers.Where(x => x.Position == PositionType.Defender).ToList();
            model.AwayMatchMidfielders = awayMatchPlayers.Where(x => x.Position == PositionType.Midfielder).ToList();
            model.AwayMatchStrikers = awayMatchPlayers.Where(x => x.Position == PositionType.Striker).ToList();

            model.MatchEvents = _context.MatchEvents.Include(x => x.MatchPlayer).ThenInclude(x => x.Player).Include(x => ((MatchEventGoal)x).AssistPlayer).Where(x => x.MatchPlayer.MatchId == match.Id).Select(x => new GetMatchEventDto
            {
                Id = x.Id,
                TeamId = x.MatchPlayer.TeamId,
                FirstName = x.MatchPlayer.Player.FirstName,
                LastName = x.MatchPlayer.Player.LastName,
                AssistPlayerFirstName = x is MatchEventGoal ? ((MatchEventGoal)x).AssistPlayer.FirstName : "",
                AssistPlayerLastName = x is MatchEventGoal ? ((MatchEventGoal)x).AssistPlayer.LastName : "",
                Minute = x.Minute,
                PenaltyType = x is MatchEventFoul ? ((MatchEventFoul)x).PenaltyType : null,
                MatchEventType = x.MatchEventType
            }).ToList();

            model.HomeTeam.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + model.HomeTeamId;
            model.AwayTeam.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + model.AwayTeamId;

            WebRequest webRequestHomeTeam = WebRequest.Create(model.HomeTeam.Image);
            WebResponse webResponseHomeTeam;
            try
            {
                webResponseHomeTeam = webRequestHomeTeam.GetResponse();
            }
            catch //If exception thrown then couldn't get response from address
            {
                model.HomeTeam.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/defaultTeam";
            }
            WebRequest webRequestAwayTeam = WebRequest.Create(model.AwayTeam.Image);
            WebResponse webResponseAwayTeam;
            try
            {
                webResponseAwayTeam = webRequestAwayTeam.GetResponse();
            }
            catch //If exception thrown then couldn't get response from address
            {
                model.AwayTeam.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/defaultTeam";
            }

            return model;
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
                MatchType = model.MatchType,
                AddedByTeamId = model.AddedByTeamId
            };

            _context.Matches.Add(match);
            _context.SaveChanges();

            foreach (var matchPlayerModel in model.HomeMatchPlayers)
            {
                if (match.HomeTeamId.HasValue)
                {
                    MatchPlayer matchPlayer = new MatchPlayer()
                    {
                        PlayerId = matchPlayerModel.PlayerId,
                        MatchId = match.Id,
                        TeamId = match.HomeTeamId.Value,
                        Position = matchPlayerModel.Position,
                        Number = matchPlayerModel.Number
                    };
                    _context.MatchPlayers.Add(matchPlayer);
                    _context.SaveChanges();
                }
            }

            foreach (var matchPlayerModel in model.AwayMatchPlayers)
            {
                if (match.AwayTeamId.HasValue)
                {
                    MatchPlayer matchPlayer = new MatchPlayer()
                    {
                        PlayerId = matchPlayerModel.PlayerId,
                        MatchId = match.Id,
                        TeamId = match.AwayTeamId.Value,
                        Position = matchPlayerModel.Position,
                        Number = matchPlayerModel.Number
                    };
                    _context.MatchPlayers.Add(matchPlayer);
                    _context.SaveChanges();
                }
            }

            List<MatchPlayer> matchPlayers = _context.MatchPlayers.Where(x => x.MatchId == match.Id).ToList();

            foreach (var eventModel in model.MatchEvents)
            {
                if (eventModel.MatchEventType == MatchEventType.Goal)
                {
                    MatchEventGoal matchEventGoal = new MatchEventGoal()
                    {
                        MatchPlayerId = matchPlayers.Single(x => x.PlayerId == eventModel.PlayerId).Id,
                        Minute = eventModel.Minute,
                        MatchEventType = eventModel.MatchEventType,
                        AssistPlayerId = eventModel.AssistPlayerId
                    };
                    _context.MatchEventGoals.Add(matchEventGoal);
                }

                else if (eventModel.MatchEventType == MatchEventType.Foul)
                {
                    MatchEventFoul matchEventFoul = new MatchEventFoul()
                    {
                        MatchPlayerId = matchPlayers.Single(x => x.PlayerId == eventModel.PlayerId).Id,
                        Minute = eventModel.Minute,
                        MatchEventType = eventModel.MatchEventType,
                        PenaltyType = eventModel.PenaltyType
                    };
                    _context.MatchEventFouls.Add(matchEventFoul);
                }
            }

            _context.SaveChanges();

        }

        public void SendMatchAnswer(MatchAnswerDto model)
        {
            Match match = _context.Matches.Single(x => x.Id == model.Id);
            Team homeTeam = _context.Teams.Single(x => x.Id == match.HomeTeamId);
            Team awayTeam = _context.Teams.Single(x => x.Id == match.AwayTeamId);

            if (model.IsAccepted)
            {
                List<MatchEventGoal> homeTeamGoals = _context.MatchEventGoals.Include(x => x.MatchPlayer).Where(x => x.MatchPlayer.MatchId == match.Id && x.MatchPlayer.TeamId == match.HomeTeamId).ToList();
                List<MatchEventGoal> awayTeamGoals = _context.MatchEventGoals.Where(x => x.MatchPlayer.MatchId == match.Id && x.MatchPlayer.TeamId == match.AwayTeamId).ToList();

                if (homeTeamGoals.Count > awayTeamGoals.Count)
                {
                    homeTeam.Points = homeTeam.Points + 3;
                    _context.Teams.Update(homeTeam);
                }
                else if (homeTeamGoals.Count < awayTeamGoals.Count)
                {
                    awayTeam.Points = awayTeam.Points + 3;
                    _context.Teams.Update(awayTeam);
                }
                else if (homeTeamGoals.Count == awayTeamGoals.Count)
                {
                    homeTeam.Points = homeTeam.Points + 1;
                    awayTeam.Points = awayTeam.Points + 1;
                    _context.Teams.Update(homeTeam);
                    _context.Teams.Update(awayTeam);
                }

                match.IsConfirmed = model.IsAccepted;
            }
            else
            {
                Team team = new Team();
                Team otherTeam = new Team();

                if (awayTeam.Id == model.UserTeamId)
                {
                    team = homeTeam;
                    otherTeam = awayTeam;
                }
                else
                {
                    team = awayTeam;
                    otherTeam = homeTeam;
                }

                Message systemMessage = new Message
                {
                    FromUserId = _context.UserDatas.First(x => x.UserType == UserType.System).UserId,
                    Subject = "Mecz niezaakceptowany",
                    Content = $"Mecz przeciwko drużynie {otherTeam.Name} został odrzucony przez zarząd drużyny przeciwnej.<br/><br/>Ta wiadomość została wygenerowana automatycznie, prosimy na nią nie odpowiadać.",
                    SentOn = DateTime.Now,
                    MessageType = MessageType.System
                };
                _context.Messages.Add(systemMessage);
                _context.SaveChanges();

                List<UserData> teamManagers = _context.UserDatas.Include(x => x.User).ThenInclude(x => x.UserData).Where(x => x.TeamId == team.Id && x.User.UserData.UserType == UserType.Management).ToList();
                foreach (var person in teamManagers)
                {
                    if (!person.UserId.HasValue)
                    {
                        continue;
                    }

                    UserMessage userMessage = new UserMessage()
                    {
                        MessageId = systemMessage.Id,
                        ToUserId = person.UserId.Value
                    };

                    _context.UserMessages.Add(userMessage);
                }

                _context.Matches.Remove(match);
            }

            _context.SaveChanges();
        }
    }
}
