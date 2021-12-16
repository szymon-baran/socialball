using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.DTOs.Teams;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialballWebAPI.Services
{
    public class TeamService : ITeamService
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService UserService;

        public TeamService(SocialballDBContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            UserService = userService;
        }

        public object GetTeams()
        {
            return _context.Teams.Include(x => x.League).Where(x => x.IsActive).ToList();
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

        public void AddTeam(AddTeamDto model)
        {
            Guid userId = UserService.AddUserAccountForNewTeamManager(model);

            Team team = new Team()
            {
                Name = model.Name,
                LeagueId = model.LeagueId,
                IsActive = false
            };
            _context.Teams.Add(team);
            _context.SaveChanges();

            TeamManager teamManager = new TeamManager()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth.AddHours(12),
                UserId = userId,
                UserType = UserType.Management,
                TeamId = team.Id
            };
            _context.TeamManagers.Add(teamManager);
            _context.SaveChanges();

            if (model.Image != null && model.Image.Length > 0)
            {
                var stream = new MemoryStream(model.Image);
                string fileName = team.Id.ToString();

                UploadImage(stream, fileName);
            }
        }


        public object GetTeamsByLeague(Guid leagueId)
        {
            return _context.Teams.Include(x => x.League).Where(x => x.IsActive && x.LeagueId == leagueId).ToList();
        }

        public List<SelectList> GetTeamsToSelectList()
        {
            return _context.Teams.Where(x => x.IsActive).Select(x => new SelectList { Id = x.Id, Name = x.Name }).ToList();
        }

        public List<League> GetLeaguesToLookup()
        {
            return _context.Leagues.OrderBy(x => x.Ranking).ToList();
        }

        public List<PositionsInTeam> GetTeamsToChart(Guid teamId)
        {
            List<PositionsInTeam> positionsInTeams = new List<PositionsInTeam>();

            for (PositionType p = 0; p <= PositionType.Striker; p++)
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

            model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + team.Id;

            WebRequest webRequest = WebRequest.Create(model.Image);
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch //If exception thrown then couldn't get response from address
            {
                model.Image = "https://socialball-avatars.s3.eu-central-1.amazonaws.com/defaultTeam";
            }

            return model;
        }
    }
}
