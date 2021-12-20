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
    public class JobAdvertisementService : IJobAdvertisementService
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;
        private IPlayerService PlayerService;

        public JobAdvertisementService(SocialballDBContext context, IMapper mapper, IPlayerService playerService)
        {
            _context = context;
            _mapper = mapper;
            PlayerService = playerService;
        }

        public object GetUserJobAdvertisements(Guid userId)
        {
            GetPlayerDto player = PlayerService.GetPlayerDetailsByUserId(userId);
            return _context.FromTeamJobAdvertisements.Where(x => x.Position == player.Position && x.IsActive == true).Select(x => new
            {
                x.Id,
                x.TeamId,
                x.Earnings,
                x.TrainingSessionsPerWeek,
                x.Location,
                x.JobAdvertisementType,
                IsAlreadyAnswered = x.JobAdvertisementAnswers.Any(y => y is JobAdvertisementUserAnswer ? ((JobAdvertisementUserAnswer)y).UserId == userId && y.IsResponded == false : false)
            }).ToList();
        }

        public object GetTeamJobAdvertisements(Guid userId)
        {
            UserData userData = _context.UserDatas.First(x => x.UserId == userId);
            return _context.FromUserJobAdvertisements.Where(x => x.IsActive == true).Select(x => new
            {
                x.Id,
                x.UserId,
                UserFullName = x.User.UserData.FirstName + " " + x.User.UserData.LastName,
                Username = x.User.Username,
                x.Location,
                x.JobAdvertisementType,
                IsAlreadyAnswered = x.JobAdvertisementAnswers.Any(y => y is JobAdvertisementTeamAnswer ? ((JobAdvertisementTeamAnswer)y).TeamId == userData.TeamId && y.IsResponded == false : false)
            }).ToList();
        }

        public List<FromTeamJobAdvertisement> GetMyTeamJobAdvertisements(Guid userId)
        {
            UserData userData = _context.UserDatas.First(x => x.UserId == userId);
            return _context.FromTeamJobAdvertisements.Where(x => x.TeamId == userData.TeamId).ToList();
        }

        public object GetJobAdvertisementsAnswers(Guid userId)
        {
            UserData userData = _context.UserDatas.Single(x => x.UserId == userId);
            if (userData.UserType == UserType.Management)
            {
                return _context.JobAdvertisementUserAnswers.Where(x => x.TeamId == userData.TeamId || (x.JobAdvertisement is FromTeamJobAdvertisement ? ((FromTeamJobAdvertisement)x.JobAdvertisement).TeamId == userData.TeamId : false)).Select(x => new
                {
                    x.Id,
                    x.JobAdvertisementId,
                    x.JobAdvertisementAnswerType,
                    x.IsResponsePositive,
                    x.Content,
                    x.UserId,
                    x.ResponseContent,
                    x.IsResponded,
                    senderName = x.User.UserData.FirstName + " " + x.User.UserData.LastName
                }).ToList();
            }
            else
            {
                return _context.JobAdvertisementTeamAnswers.Where(x => x.JobAdvertisement is FromUserJobAdvertisement ? ((FromUserJobAdvertisement)x.JobAdvertisement).UserId == userData.UserId : false).Select(x => new
                {
                    x.Id,
                    x.JobAdvertisementId,
                    x.JobAdvertisementAnswerType,
                    x.IsResponsePositive,
                    x.Content,
                    x.TeamId,
                    x.ResponseContent,
                    x.IsResponded,
                    senderName = x.Team.Name
                }).ToList();
            }
        }

        public JobAdvertisementDto GetJobAdvertisementDetails(Guid id)
        {
            JobAdvertisement jobAdvertisement = _context.JobAdvertisements.Single(x => x.Id == id);
            JobAdvertisementDto model = new JobAdvertisementDto()
            {
                Id = jobAdvertisement.Id,
                JobAdvertisementType = jobAdvertisement.JobAdvertisementType,
                Location = jobAdvertisement.Location,
                Content = jobAdvertisement.Content,
                IsActive = jobAdvertisement.IsActive
            };

            switch (model.JobAdvertisementType)
            {
                case JobAdvertisementType.FromTeam:
                    FromTeamJobAdvertisement fromTeam = _context.FromTeamJobAdvertisements.Include(x => x.Team).Single(x => x.Id == id);
                    model.TeamId = fromTeam.TeamId;
                    model.TeamName = fromTeam.Team.Name;
                    model.Earnings = fromTeam.Earnings;
                    model.TrainingSessionsPerWeek = fromTeam.TrainingSessionsPerWeek;
                    model.Position = fromTeam.Position;
                    break;
                case JobAdvertisementType.FromUser:
                    FromUserJobAdvertisement fromUser = _context.FromUserJobAdvertisements.Include(x => x.User).ThenInclude(x => x.UserData).Single(x => x.Id == id);
                    model.UserId = fromUser.UserId;
                    model.UserFullName = fromUser.User.UserData.FirstName + " " + fromUser.User.UserData.LastName;
                    break;
            }

            return model;
        }

        public FromUserJobAdvertisement GetUserJobAdvertisementDetails(Guid userId)
        {
            FromUserJobAdvertisement jobAdvertisement = _context.FromUserJobAdvertisements.FirstOrDefault(x => x.UserId == userId);
            return jobAdvertisement;
        }

        private JobAdvertisementTeamAnswer GetJobAdvertisementTeamAnswerDetails(Guid id)
        {
            JobAdvertisementTeamAnswer answer = _context.JobAdvertisementTeamAnswers.Include(x => x.Team).First(x => x.Id == id);
            return answer;
        }

        private JobAdvertisementUserAnswer GetJobAdvertisementUserAnswerDetails(Guid id)
        {
            JobAdvertisementUserAnswer answer = _context.JobAdvertisementUserAnswers.Include(x => x.User).ThenInclude(x => x.UserData).First(x => x.Id == id);
            return answer;
        }

        public object GetJobAdvertisementAnswerDetails(Guid id)
        {
            JobAdvertisementType answerType = _context.JobAdvertisementAnswers.First(x => x.Id == id).JobAdvertisementAnswerType;
            switch (answerType)
            {
                case JobAdvertisementType.FromTeam:
                    return GetJobAdvertisementTeamAnswerDetails(id);
                case JobAdvertisementType.FromUser:
                    return GetJobAdvertisementUserAnswerDetails(id);
                default:
                    throw new KeyNotFoundException();
            }
        }

        public void UpdateUserJobAdvertisement(UserJobAdvertisementDto model)
        {
            FromUserJobAdvertisement user = _context.FromUserJobAdvertisements.FirstOrDefault(x => x.UserId == model.UserId) ?? new FromUserJobAdvertisement();
            user.JobAdvertisementType = JobAdvertisementType.FromUser;
            user.Location = model.Location;
            user.Content = model.Content;
            user.Earnings = model.Earnings;
            user.UserId = model.UserId;
            user.IsActive = model.IsActive;

            if (user.Id == null || user.Id == Guid.Empty)
            {
                _context.FromUserJobAdvertisements.Add(user);
            }
            else
            {
                _context.FromUserJobAdvertisements.Update(user);
            }

            _context.SaveChanges();
            return;
        }

        public void AddMyTeamJobAdvertisement(TeamJobAdvertisementDto model)
        {
            FromTeamJobAdvertisement jobAdvertisement = new FromTeamJobAdvertisement()
            {
                TeamId = model.TeamId,
                Location = model.Location,
                Content = model.Content,
                Earnings = model.Earnings,
                TrainingSessionsPerWeek = model.TrainingSessionsPerWeek,
                Position = model.Position,
                IsActive = model.IsActive
            };
            _context.FromTeamJobAdvertisements.Add(jobAdvertisement);
            _context.SaveChanges();
        }

        public void EditMyTeamJobAdvertisement(TeamJobAdvertisementDto model)
        {
            FromTeamJobAdvertisement jobAdvertisement = _context.FromTeamJobAdvertisements.Single(x => x.Id == model.Id);
            jobAdvertisement.TeamId = model.TeamId;
            jobAdvertisement.Location = model.Location;
            jobAdvertisement.Content = model.Content;
            jobAdvertisement.Earnings = model.Earnings;
            jobAdvertisement.TrainingSessionsPerWeek = model.TrainingSessionsPerWeek;
            jobAdvertisement.Position = model.Position;
            jobAdvertisement.IsActive = model.IsActive;
            _context.FromTeamJobAdvertisements.Update(jobAdvertisement);
            _context.SaveChanges();
        }

        public void AddJobAdvertisementAnswer(JobAdvertisementAnswerDto model)
        {
            switch (model.JobAdvertisementAnswerType)
            {
                case JobAdvertisementType.FromTeam:
                    UserData userData = _context.UserDatas.Single(x => x.UserId == model.UserId.Value);
                    JobAdvertisementTeamAnswer jobAdvertisementTeam = new JobAdvertisementTeamAnswer()
                    {
                        JobAdvertisementId = model.JobAdvertisementId,
                        JobAdvertisementAnswerType = model.JobAdvertisementAnswerType,
                        Content = model.Content,
                        IsResponded = model.IsResponded,
                        IsResponsePositive = model.IsResponsePositive,
                        ResponseContent = model.ResponseContent,
                        TeamId = userData.TeamId.Value,
                    };
                    _context.JobAdvertisementTeamAnswers.Add(jobAdvertisementTeam);
                    break;
                case JobAdvertisementType.FromUser:
                    JobAdvertisementUserAnswer jobAdvertisementUser = new JobAdvertisementUserAnswer()
                    {
                        JobAdvertisementId = model.JobAdvertisementId,
                        JobAdvertisementAnswerType = model.JobAdvertisementAnswerType,
                        Content = model.Content,
                        IsResponded = model.IsResponded,
                        IsResponsePositive = model.IsResponsePositive,
                        ResponseContent = model.ResponseContent,
                        UserId = model.UserId.Value,
                    };
                    _context.JobAdvertisementUserAnswers.Add(jobAdvertisementUser);
                    break;
            }
            _context.SaveChanges();
            return;
        }

        public void AddResponseToJobAdvertisementAnswer(JobAdvertisementAnswerDto model)
        {
            JobAdvertisementAnswer answer = _context.JobAdvertisementAnswers.Include(x => x.JobAdvertisement).Single(x => x.Id == model.Id);
            answer.IsResponded = true;
            answer.IsResponsePositive = model.IsResponsePositive;
            answer.ResponseContent = model.ResponseContent;
            if (model.IsResponsePositive)
            {
                UserData userData = _context.UserDatas.Single(x => x.UserId == model.UserId);
                if (userData.TeamId == null || userData.TeamId == Guid.Empty)
                {
                    userData.TeamId = model.TeamId;
                    userData.Earnings = answer.JobAdvertisement?.Earnings;
                }
                _context.UserDatas.Update(userData);
                List<FromUserJobAdvertisement> userAdvertisements = _context.FromUserJobAdvertisements.Where(x => x.UserId == userData.UserId).ToList();
                if (userAdvertisements.Count > 0)
                {
                    _context.FromUserJobAdvertisements.RemoveRange(userAdvertisements);
                }
            }
            else
            {
                if (answer.JobAdvertisementAnswerType == JobAdvertisementType.FromUser)
                {
                    JobAdvertisementUserAnswer userAnswer = _context.JobAdvertisementUserAnswers.Include(x => x.User).ThenInclude(x => x.UserData).Single(x => x.Id == model.Id);
                    FromTeamJobAdvertisement jobAdvertisement = _context.FromTeamJobAdvertisements.Include(x => x.Team).Single(x => x.Id == userAnswer.JobAdvertisementId);
                    Message systemMessage = new Message
                    {
                        FromUserId = _context.UserDatas.First(x => x.UserType == UserType.System).UserId,
                        Subject = "Odpowiedź odrzucona",
                        Content = $"Twoja odpowiedź na ogłoszenie drużyny {jobAdvertisement.Team.Name ?? ""} została odrzucona.<br/><br/>Ta wiadomość została wygenerowana automatycznie, prosimy na nią nie odpowiadać.",
                        SentOn = DateTime.Now,
                        MessageType = MessageType.System
                    };
                    _context.Messages.Add(systemMessage);
                    _context.SaveChanges();

                    UserMessage userMessage = new UserMessage()
                    {
                        MessageId = systemMessage.Id,
                        ToUserId = userAnswer.UserId
                    };

                    _context.UserMessages.Add(userMessage);
                }
                else if (answer.JobAdvertisementAnswerType == JobAdvertisementType.FromTeam)
                {
                    JobAdvertisementTeamAnswer teamAnswer = _context.JobAdvertisementTeamAnswers.Include(x => x.Team).Single(x => x.Id == model.Id);
                    FromUserJobAdvertisement jobAdvertisement = _context.FromUserJobAdvertisements.Include(x => x.User).ThenInclude(x => x.UserData).Single(x => x.Id == teamAnswer.JobAdvertisementId);
                    Message systemMessage = new Message
                    {
                        FromUserId = _context.UserDatas.First(x => x.UserType == UserType.System).UserId,
                        Subject = "Odpowiedź odrzucona",
                        Content = $"Odpowiedź Twojej drużyny na ogłoszenie zawodnika {jobAdvertisement.User.UserData.FirstName ?? ""} {jobAdvertisement.User.UserData.LastName ?? ""} została odrzucona.<br/><br/>Ta wiadomość została wygenerowana automatycznie, prosimy na nią nie odpowiadać.",
                        SentOn = DateTime.Now,
                        MessageType = MessageType.System
                    };
                    _context.Messages.Add(systemMessage);
                    _context.SaveChanges();

                    List<UserData> teamManagers = _context.UserDatas.Include(x => x.User).ThenInclude(x => x.UserData).Where(x => x.TeamId == teamAnswer.TeamId && x.User.UserData.UserType == UserType.Management).ToList();
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
                }
            }
            _context.JobAdvertisementAnswers.Update(answer);
            _context.SaveChanges();
        }

    }
}
