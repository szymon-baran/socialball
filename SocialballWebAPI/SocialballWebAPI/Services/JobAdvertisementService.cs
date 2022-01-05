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
        private IPlayerService _playerService;
        private IJobAdvertisementRepository _jobAdvertisementRepository;
        private IPlayerRepository _playerRepository;
        private IJobAdvertisementAnswerRepository _jobAdvertisementAnswerRepository;
        private IMessageRepository _messageRepository;

        public JobAdvertisementService(IPlayerService playerService, IJobAdvertisementRepository jobAdvertisementRepository, IPlayerRepository playerRepository, IJobAdvertisementAnswerRepository jobAdvertisementAnswerRepository, IMessageRepository messageRepository)
        {
            _playerService = playerService;
            _jobAdvertisementRepository = jobAdvertisementRepository;
            _jobAdvertisementAnswerRepository = jobAdvertisementAnswerRepository;
            _playerRepository = playerRepository;
            _messageRepository = messageRepository;
        }

        public object GetUserJobAdvertisements(Guid userId)
        {
            GetPlayerDto player = _playerService.GetPlayerDetailsByUserId(userId);
            return _jobAdvertisementRepository.GetFromTeamJobAdvertisementsByPosition(player.Position).Select(x => new
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
            UserData userData = _playerRepository.GetUserDataDetailsByUserId(userId);
            return _jobAdvertisementRepository.GetFromUserJobAdvertisements().Select(x => new
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
            UserData userData = _playerRepository.GetUserDataDetailsByUserId(userId);
            return _jobAdvertisementRepository.GetFromTeamJobAdvertisementsByTeamId(userData.TeamId);
        }

        public object GetJobAdvertisementsAnswers(Guid userId)
        {
            UserData userData = _playerRepository.GetUserDataDetailsByUserId(userId);
            if (userData.UserType == UserType.Management)
            {
                return _jobAdvertisementAnswerRepository.GetJobAdvertisementUserAnswersByTeam(userData.TeamId).Select(x => new
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
                return _jobAdvertisementAnswerRepository.GetJobAdvertisementTeamAnswersByUser(userData.UserId).Select(x => new
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
            JobAdvertisement jobAdvertisement = _jobAdvertisementRepository.GetJobAdvertisementDetails(id);
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
                    FromTeamJobAdvertisement fromTeam = _jobAdvertisementRepository.GetFromTeamJobAdvertisementDetails(id);
                    model.TeamId = fromTeam.TeamId;
                    model.TeamName = fromTeam.Team.Name;
                    model.Earnings = fromTeam.Earnings;
                    model.TrainingSessionsPerWeek = fromTeam.TrainingSessionsPerWeek;
                    model.Position = fromTeam.Position;
                    break;
                case JobAdvertisementType.FromUser:
                    FromUserJobAdvertisement fromUser = _jobAdvertisementRepository.GetFromUserJobAdvertisementDetails(id);
                    model.UserId = fromUser.UserId;
                    model.UserFullName = fromUser.User.UserData.FirstName + " " + fromUser.User.UserData.LastName;
                    break;
            }

            return model;
        }

        public FromUserJobAdvertisement GetUserJobAdvertisementDetails(Guid userId)
        {
            FromUserJobAdvertisement jobAdvertisement = _jobAdvertisementRepository.GetFromUserJobAdvertisementDetailsByUserId(userId);
            return jobAdvertisement;
        }

        private JobAdvertisementTeamAnswer GetJobAdvertisementTeamAnswerDetails(Guid id)
        {
            JobAdvertisementTeamAnswer answer = _jobAdvertisementAnswerRepository.GetJobAdvertisementTeamAnswerDetails(id);
            return answer;
        }

        private JobAdvertisementUserAnswer GetJobAdvertisementUserAnswerDetails(Guid id)
        {
            JobAdvertisementUserAnswer answer = _jobAdvertisementAnswerRepository.GetJobAdvertisementUserAnswerDetails(id);
            return answer;
        }

        public object GetJobAdvertisementAnswerDetails(Guid id)
        {
            JobAdvertisementType answerType = _jobAdvertisementAnswerRepository.GetJobAdvertisementAnswerDetails(id).JobAdvertisementAnswerType;
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
            FromUserJobAdvertisement user = _jobAdvertisementRepository.GetFromUserJobAdvertisementDetailsByUserId(model.UserId) ?? new FromUserJobAdvertisement();
            user.JobAdvertisementType = JobAdvertisementType.FromUser;
            user.Location = model.Location;
            user.Content = model.Content;
            user.Earnings = model.Earnings;
            user.UserId = model.UserId;
            user.IsActive = model.IsActive;

            if (user.Id == null || user.Id == Guid.Empty)
            {
                _jobAdvertisementRepository.AddFromUserJobAdvertisement(user);
            }
            else
            {
                _jobAdvertisementRepository.UpdateFromUserJobAdvertisement(user);
            }

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
            _jobAdvertisementRepository.AddFromTeamJobAdvertisement(jobAdvertisement);
        }

        public void EditMyTeamJobAdvertisement(TeamJobAdvertisementDto model)
        {
            FromTeamJobAdvertisement jobAdvertisement = _jobAdvertisementRepository.GetFromTeamJobAdvertisementDetails(model.Id.Value);
            jobAdvertisement.TeamId = model.TeamId;
            jobAdvertisement.Location = model.Location;
            jobAdvertisement.Content = model.Content;
            jobAdvertisement.Earnings = model.Earnings;
            jobAdvertisement.TrainingSessionsPerWeek = model.TrainingSessionsPerWeek;
            jobAdvertisement.Position = model.Position;
            jobAdvertisement.IsActive = model.IsActive;
            _jobAdvertisementRepository.EditFromTeamJobAdvertisement(jobAdvertisement);
        }

        public void AddJobAdvertisementAnswer(JobAdvertisementAnswerDto model)
        {
            switch (model.JobAdvertisementAnswerType)
            {
                case JobAdvertisementType.FromTeam:
                    UserData userData = _playerRepository.GetUserDataDetailsByUserId(model.UserId.Value);
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
                    _jobAdvertisementAnswerRepository.AddJobAdvertisementTeamAnswer(jobAdvertisementTeam);
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
                    _jobAdvertisementAnswerRepository.AddJobAdvertisementUserAnswer(jobAdvertisementUser);
                    break;
            }

            return;
        }

        public void AddResponseToJobAdvertisementAnswer(JobAdvertisementAnswerDto model)
        {
            JobAdvertisementAnswer answer = _jobAdvertisementAnswerRepository.GetJobAdvertisementAnswerDetails(model.Id.Value);
            answer.IsResponded = true;
            answer.IsResponsePositive = model.IsResponsePositive;
            answer.ResponseContent = model.ResponseContent;
            if (model.IsResponsePositive)
            {
                if (model.UserId.HasValue)
                {
                    UserData userData = _playerRepository.GetUserDataDetailsByUserId(model.UserId.Value);
                    if (userData.TeamId == null || userData.TeamId == Guid.Empty)
                    {
                        userData.TeamId = model.TeamId;
                        userData.Earnings = answer.JobAdvertisement?.Earnings;
                    }
                    _playerRepository.UpdateUserData(userData);
                    List<FromUserJobAdvertisement> userAdvertisements = _jobAdvertisementRepository.GetFromUserJobAdvertisementsByUserId(userData.UserId.Value);
                    if (userAdvertisements.Count > 0)
                    {
                        _jobAdvertisementRepository.RemoveFromUserJobAdvertisementsRange(userAdvertisements);
                    }
                }
            }
            else
            {
                if (answer.JobAdvertisementAnswerType == JobAdvertisementType.FromUser)
                {
                    JobAdvertisementUserAnswer userAnswer = _jobAdvertisementAnswerRepository.GetJobAdvertisementUserAnswerDetails(model.Id.Value);
                    FromTeamJobAdvertisement jobAdvertisement = _jobAdvertisementRepository.GetFromTeamJobAdvertisementDetails(userAnswer.JobAdvertisementId.Value);
                    Message systemMessage = new Message
                    {
                        FromUserId = _playerRepository.GetSystemUserDetails().UserId,
                        Subject = "Odpowiedź odrzucona",
                        Content = $"Twoja odpowiedź na ogłoszenie drużyny {jobAdvertisement.Team.Name ?? ""} została odrzucona.<br/><br/>Ta wiadomość została wygenerowana automatycznie, prosimy na nią nie odpowiadać.",
                        SentOn = DateTime.Now,
                        MessageType = MessageType.System
                    };
                    _messageRepository.AddMessage(systemMessage);

                    UserMessage userMessage = new UserMessage()
                    {
                        MessageId = systemMessage.Id,
                        ToUserId = userAnswer.UserId
                    };
                    _messageRepository.AddUserMessage(userMessage);
                }
                else if (answer.JobAdvertisementAnswerType == JobAdvertisementType.FromTeam)
                {
                    JobAdvertisementTeamAnswer teamAnswer = _jobAdvertisementAnswerRepository.GetJobAdvertisementTeamAnswerDetails(model.Id.Value);
                    FromUserJobAdvertisement jobAdvertisement = _jobAdvertisementRepository.GetFromUserJobAdvertisementDetails(teamAnswer.JobAdvertisementId.Value);
                    Message systemMessage = new Message
                    {
                        FromUserId = _playerRepository.GetSystemUserDetails().UserId,
                        Subject = "Odpowiedź odrzucona",
                        Content = $"Odpowiedź Twojej drużyny na ogłoszenie zawodnika {jobAdvertisement.User.UserData.FirstName ?? ""} {jobAdvertisement.User.UserData.LastName ?? ""} została odrzucona.<br/><br/>Ta wiadomość została wygenerowana automatycznie, prosimy na nią nie odpowiadać.",
                        SentOn = DateTime.Now,
                        MessageType = MessageType.System
                    };
                    _messageRepository.AddMessage(systemMessage);

                    if (teamAnswer.TeamId.HasValue)
                    {
                        List<UserData> teamManagers = _playerRepository.GetManagersInTeam(teamAnswer.TeamId.Value);
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
                    }
                }
            }
            _jobAdvertisementAnswerRepository.UpdateJobAdvertisementAnswer(answer);
        }

    }
}
