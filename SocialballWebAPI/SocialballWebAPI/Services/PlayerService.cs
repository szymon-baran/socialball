using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SocialballWebAPI.Extensions;

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
            return _context.Players.Where(x => x.UserType == UserType.Player).ToList();
        }

        public object GetPlayersByTeamId(Guid teamId)
        {
            return _context.Players.Where(x => x.UserType == UserType.Player && x.TeamId == teamId).Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.Position,
                x.TeamId,
                x.Citizenship,
                x.Earnings,
                IsInjured = x.IsInjuredUntil > DateTime.Now ? true : false,
                x.IsInjuredUntil
            }).ToList();
        }

        public GetPlayerDto GetPlayerDetails(Guid id)
        {
            Player player = _context.Players.Include(x => x.Team).Include(x => x.User).FirstOrDefault(x => x.Id == id);
            if (player == null)
            {
                throw new KeyNotFoundException();
            }
            GetPlayerDto model = _mapper.Map<GetPlayerDto>(player);
            if (player.Team != null)
            {
                model.TeamName = player.Team.Name;
            }
            if (player.User.Email != null)
            {
                model.Email = player.User.Email;
            }
            model.Goals = _context.MatchEventGoals
                .Include(x => x.MatchPlayer)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.HomeTeam)
                .Include(x => x.MatchPlayer)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.AwayTeam)
                .Include(x => x.AssistPlayer)
                .Where(x => x.MatchPlayer.PlayerId == player.Id && x.MatchEventType == MatchEventType.Goal && x.MatchPlayer.Match.IsConfirmed).Select(x => new GoalInPlayerDetailsDto
                {
                    Id = x.Id,
                    Minute = x.Minute,
                    MatchId = x.MatchPlayer.MatchId,
                    MatchBetween = x.MatchPlayer.Match.HomeTeam.Name + " - " + x.MatchPlayer.Match.AwayTeam.Name,
                    DateTime = x.MatchPlayer.Match.DateTime,
                    GoalAssistPlayerName = x.AssistPlayer != null ? (x.AssistPlayer.FirstName + " " + x.AssistPlayer.LastName) : null
                }).OrderByDescending(x => x.DateTime).ToList();
            model.Assists = _context.MatchEventGoals
                .Include(x => x.MatchPlayer)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.HomeTeam)
                .Include(x => x.MatchPlayer)
                    .ThenInclude(x => x.Match)
                        .ThenInclude(x => x.AwayTeam)
                .Include(x => x.AssistPlayer)
                .Where(x => x.AssistPlayerId == player.Id && x.MatchEventType == MatchEventType.Goal && x.MatchPlayer.Match.IsConfirmed).Select(x => new GoalInPlayerDetailsDto
                {
                    Id = x.Id,
                    Minute = x.Minute,
                    MatchId = x.MatchPlayer.MatchId,
                    MatchBetween = x.MatchPlayer.Match.HomeTeam.Name + " - " + x.MatchPlayer.Match.AwayTeam.Name,
                    DateTime = x.MatchPlayer.Match.DateTime,
                    GoalScorerName = x.MatchPlayer.Player.FirstName + " " + x.MatchPlayer.Player.LastName
                }).OrderByDescending(x => x.DateTime).ToList();

            var lookup = _context.MatchEventGoals.Include(x => x.MatchPlayer).ThenInclude(x => x.Match).Where(x => x.MatchPlayer.PlayerId == player.Id && x.MatchPlayer.Match.DateTime.Year == DateTime.Today.Year && x.MatchEventType == MatchEventType.Goal && x.MatchPlayer.Match.IsConfirmed).ToLookup(y => y.MatchPlayer.Match.DateTime.Month);

            model.CurrentYearGoalsToChart =
                from month in Enumerable.Range(1, 12)
                select new
                {
                    Month = month,
                    Number = lookup[month].Count()
                };

            model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + player.Id;

            WebRequest webRequest = WebRequest.Create(model.Image);
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch //If exception thrown then couldn't get response from address
            {
                model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/default";
            }

            return model;
        }

        public GetUserDataDto GetUserDataDetails(Guid id)
        {
            UserData userData = _context.UserDatas
                .Include(x => x.User)
            .FirstOrDefault(x => x.Id == id);
            if (userData == null)
            {
                throw new KeyNotFoundException();
            }
            GetUserDataDto model = _mapper.Map<GetUserDataDto>(userData);
            if (userData.Team != null)
            {
                model.TeamName = userData.Team.Name;
            }
            if (userData.User.Email != null)
            {
                model.Email = userData.User.Email;
            }

            model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + userData.Id;

            WebRequest webRequest = WebRequest.Create(model.Image);
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch //If exception thrown then couldn't get response from address
            {
                model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/default";
            }

            return model;
        }

        public GetPlayerDto GetPlayerDetailsByUserId(Guid userId)
        {
            Player player = _context.Players.Include(x => x.MatchesPlayer).ThenInclude(x => x.MatchEvents.Where(y => y.MatchEventType == MatchEventType.Goal)).Include(x => x.Team).FirstOrDefault(x => x.UserId == userId);
            if (player == null)
            {
                throw new KeyNotFoundException();
            }
            GetPlayerDto model = _mapper.Map<GetPlayerDto>(player);

            model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + player.Id;

            WebRequest webRequest = WebRequest.Create(model.Image);
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch //If exception thrown then couldn't get response from address
            {
                model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/default";
            }

            return model;
        }

        public GetUserDataDto GetUserDataByUserId(Guid userId)
        {
            UserData userData = _context.UserDatas.Include(x => x.Team).FirstOrDefault(x => x.UserId == userId);
            if (userData == null)
            {
                throw new KeyNotFoundException();
            }
            GetUserDataDto model = _mapper.Map<GetUserDataDto>(userData);

            model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + userData.Id;

            WebRequest webRequest = WebRequest.Create(model.Image);
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch //If exception thrown then couldn't get response from address
            {
                model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/default";
            }

            return model;
        }

        public Guid? GetUserTeamId(Guid userId)
        {
            UserData userData = _context.UserDatas.FirstOrDefault(x => x.UserId == userId);
            if (userData == null)
            {
                throw new KeyNotFoundException();
            }
            return userData.TeamId.HasValue ? userData.TeamId.Value : null;
        }

        public List<SelectList> GetPlayersByTeamToLookup(Guid teamId)
        {
            List<SelectList> players = _context.Players.Where(x => x.TeamId == teamId).OrderBy(x => x.Position).Select(x => new SelectList
            {
                Id = x.Id,
                Name = $"{x.FirstName} {x.LastName} ({EnumExtensions.GetEnumText<PositionType>(x.Position)})"
            }).ToList();

            return players;
        }

        private async Task UploadImage(MemoryStream stream, string fileName)
        {
            var credentials = new BasicAWSCredentials("AKIA5F2IVKSIURBFRAWY", "9AZQ5nS99OObdEDAs46eWJYjma5aT5yxX9r/urb0");
            var config = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.EUCentral1
            };

            using var client = new AmazonS3Client(credentials, config);
            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = "socialball-avatars",
                Key = fileName
            };

            client.DeleteObjectAsync(request).Wait();

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = "socialball-avatars",
                CannedACL = S3CannedACL.PublicRead
            };

            var fileTransferUtility = new TransferUtility(client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }

        public void AddPlayer(RegisterPlayerDto playerModel)
        {
            Guid userId = UserService.AddUserAccountForNewPlayer(playerModel);

            Player player = new Player()
            {
                FirstName = playerModel.FirstName,
                LastName = playerModel.LastName,
                Position = playerModel.Position,
                Citizenship = playerModel.Citizenship,
                DateOfBirth = playerModel.DateOfBirth.AddHours(12),
                UserId = userId,
                UserType = UserType.Player,
            };

            _context.Players.Add(player);
            if (playerModel.TeamId.HasValue)
            {
                JobAdvertisementUserAnswer jobAdvertisementUserAnswer = new JobAdvertisementUserAnswer()
                {
                    JobAdvertisementAnswerType = JobAdvertisementType.FromUser,
                    IsResponsePositive = false,
                    Content = $"Użytkownik {player.FirstName} {player.LastName} zaznaczył przy rejestracji przynależność do Twojej drużyny. Jego wybór wymaga potwierdzenia ze strony zarządu klubu.",
                    TeamId = playerModel.TeamId,
                    UserId = userId,
                    IsResponded = false,
                };
                _context.JobAdvertisementUserAnswers.Add(jobAdvertisementUserAnswer);
            }
            _context.SaveChanges();

            if (playerModel.Image != null && playerModel.Image.Length > 0)
            {
                var stream = new MemoryStream(playerModel.Image);
                string fileName = player.Id.ToString();

                UploadImage(stream, fileName);
            }
        }

        public void EditUserData(EditPlayerDto model)
        {
            UserData userData = _context.UserDatas.Include(x => x.User).Single(x => x.Id == model.Id);
            userData.FirstName = model.FirstName;
            userData.LastName = model.LastName;
            userData.DateOfBirth = model.DateOfBirth;
            userData.Citizenship = model.Citizenship;
            userData.Earnings = model.Earnings != null ? model.Earnings : userData.Earnings;
            userData.User.Email = model.Email;

            if (model.Image != null && model.Image.Length > 0)
            {
                var stream = new MemoryStream(model.Image);
                string fileName = model.Id.ToString();

                UploadImage(stream, fileName);
            }

            if (userData.UserType == UserType.Player)
            {
                ((Player)userData).Position = (int?)model.Position;
                _context.Players.Update((Player)userData);
            }
            else
            {
                _context.UserDatas.Update(userData);
            }
            _context.SaveChanges();

        }

        public void AddEditPlayerInjury(PlayerInjuryDto model)
        {
            Player player = _context.Players.Single(x => x.Id == model.Id);
            player.IsInjuredUntil = model.IsInjuredUntil.AddHours(1);
            _context.Players.Update(player);
            _context.SaveChanges();
        }

        public void KickPlayerOutOfTeam(Guid id)
        {
            Player player = _context.Players.Single(x => x.Id == id);
            player.TeamId = null;
            _context.Players.Update(player);
            _context.SaveChanges();
        }

        public bool CheckUsernameUniqueness(string username)
        {
            return !_context.Users.Any(x => x.Username == username);
        }

    }
}
