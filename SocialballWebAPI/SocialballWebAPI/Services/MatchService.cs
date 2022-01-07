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
        private readonly IMapper _mapper;
        private readonly IMatchRepository _matchRepository;
        private readonly IMatchPlayerRepository _matchPlayerRepository;
        private readonly IMatchEventRepository _matchEventRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IMessageRepository _messageRepository;

        public MatchService(IMapper mapper, IMatchRepository matchRepository, IMatchPlayerRepository matchPlayerRepository, IMatchEventRepository matchEventRepository, ITeamRepository teamRepository, IPlayerRepository playerRepository, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _matchRepository = matchRepository;
            _matchPlayerRepository = matchPlayerRepository;
            _matchEventRepository = matchEventRepository;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _messageRepository = messageRepository;
        }

        public object GetAllMatches()
        {
            return _matchRepository.GetConfirmedMatches();
        }

        public object GetTeamMatches(Guid teamId)
        {
            return _matchRepository.GetConfirmedTeamMatches(teamId);
        }

        public object GetUnconfirmedMatches(Guid teamId)
        {
            return _matchRepository.GetUnconfirmedTeamMatches(teamId).Select(x => new
            {
                x.Id,
                x.HomeTeamId,
                x.AwayTeamId,
                x.MatchPlayers,
                x.AddedByTeamId,
                x.Stadium,
                x.DateTime,
                IsUnconfirmedByYourTeam = x.AddedByTeamId == teamId ? false : true
            }).ToList();
        }

        public MatchDetailsDto GetMatchDetails(Guid id)
        {
            Match match = _matchRepository.GetMatchDetails(id);

            if (match == null)
            {
                throw new KeyNotFoundException();
            }

            MatchDetailsDto model = _mapper.Map<MatchDetailsDto>(match);
            if (match.HomeTeamId.HasValue && match.AwayTeamId.HasValue)
            {
                List<GetMatchPlayerDto> homeMatchPlayers = _matchPlayerRepository.GetMatchPlayersInMatchByTeam(match.Id, match.HomeTeamId.Value).Select(x => new GetMatchPlayerDto
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

                List<GetMatchPlayerDto> awayMatchPlayers = _matchPlayerRepository.GetMatchPlayersInMatchByTeam(match.Id, match.AwayTeamId.Value).Select(x => new GetMatchPlayerDto
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

                model.MatchEvents = _matchEventRepository.GetMatchEventsInMatch(match.Id).Select(x => new GetMatchEventDto
                {
                    Id = x.Id,
                    TeamId = x.MatchPlayer.TeamId,
                    FirstName = x.MatchPlayer.Player.FirstName,
                    LastName = x.MatchPlayer.Player.LastName,
                    AssistPlayerFirstName = x is MatchEventGoal ? (((MatchEventGoal)x).AssistPlayer != null ? ((MatchEventGoal)x).AssistPlayer.FirstName : "") : "",
                    AssistPlayerLastName = x is MatchEventGoal ? (((MatchEventGoal)x).AssistPlayer != null ? ((MatchEventGoal)x).AssistPlayer.LastName : "") : "",
                    Minute = x.Minute,
                    PenaltyType = x is MatchEventFoul ? (((MatchEventFoul)x).PenaltyType != null ? ((MatchEventFoul)x).PenaltyType : null) : null,
                    MatchEventType = x.MatchEventType
                }).ToList();

                model.HomeTeam.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + model.HomeTeamId;
                model.AwayTeam.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + model.AwayTeamId;
            }
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

            _matchRepository.AddMatch(match);

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
                    _matchPlayerRepository.AddMatchPlayer(matchPlayer);
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
                    _matchPlayerRepository.AddMatchPlayer(matchPlayer);
                }
            }

            List<MatchPlayer> matchPlayers = _matchPlayerRepository.GetMatchPlayersInMatch(match.Id);

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
                    _matchEventRepository.AddMatchEventGoal(matchEventGoal);
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
                    _matchEventRepository.AddMatchEventFoul(matchEventFoul);
                }
            }
        }

        public void SendMatchAnswer(MatchAnswerDto model)
        {
            Match match = _matchRepository.GetMatchDetails(model.Id);
            if (!match.HomeTeamId.HasValue || !match.AwayTeamId.HasValue)
            {
                throw new NullReferenceException();
            }
            Team homeTeam = _teamRepository.GetTeamDetails(match.HomeTeamId.Value);
            Team awayTeam = _teamRepository.GetTeamDetails(match.AwayTeamId.Value);

            if (model.IsAccepted)
            {
                List<MatchEventGoal> homeTeamGoals = _matchEventRepository.GetGoalsInMatchByTeam(match.Id, match.HomeTeamId.Value);
                List<MatchEventGoal> awayTeamGoals = _matchEventRepository.GetGoalsInMatchByTeam(match.Id, match.AwayTeamId.Value);

                if (homeTeamGoals.Count > awayTeamGoals.Count)
                {
                    homeTeam.Points = homeTeam.Points + 3;
                    _teamRepository.UpdateTeam(homeTeam);
                }
                else if (homeTeamGoals.Count < awayTeamGoals.Count)
                {
                    awayTeam.Points = awayTeam.Points + 3;
                    _teamRepository.UpdateTeam(awayTeam);
                }
                else if (homeTeamGoals.Count == awayTeamGoals.Count)
                {
                    homeTeam.Points = homeTeam.Points + 1;
                    awayTeam.Points = awayTeam.Points + 1;
                    _teamRepository.UpdateTeam(homeTeam);
                    _teamRepository.UpdateTeam(awayTeam);
                }

                match.IsConfirmed = true;
                _matchRepository.UpdateMatch(match);
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
                    FromUserId = _playerRepository.GetSystemUserDetails().UserId,
                    Subject = "Mecz niezaakceptowany",
                    Content = $"Mecz przeciwko drużynie {otherTeam.Name} został odrzucony przez zarząd drużyny przeciwnej.<br/><br/>Ta wiadomość została wygenerowana automatycznie, prosimy na nią nie odpowiadać.",
                    SentOn = DateTime.Now,
                    MessageType = MessageType.System
                };
                _messageRepository.AddMessage(systemMessage);

                List<UserData> teamManagers = _playerRepository.GetManagersInTeam(team.Id);
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
                    _messageRepository.AddUserMessage(userMessage);
                }
                _matchRepository.RemoveMatch(match);
            }
        }
    }
}
