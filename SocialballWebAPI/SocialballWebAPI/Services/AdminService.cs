using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.DTOs.Admins;
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
    public class AdminService : IAdminService
    {
        private readonly SocialballDBContext _context;
        private readonly IMapper _mapper;

        public AdminService(SocialballDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public object GetUsers()
        {
            return _context.Users.Include(x => x.UserData).Where(x => x.UserData.UserType != UserType.System).OrderBy(x => x.UserData.UserType).Select(x => new
            {
                x.Id,
                Name = $"{x.UserData.LastName} {x.UserData.FirstName}",
                x.Username,
                x.UserData.UserType,
                x.IsActive,
                UserDataId = x.UserData.Id,
                x.Email
            }).ToList();
        }

        public object GetTeams(Guid? leagueId)
        {
            return _context.Teams.Where(x => !leagueId.HasValue || x.LeagueId == leagueId).ToList();
        }

        public void EditTeam(AdminEditTeamDto model)
        {
            Team team = _context.Teams.Single(x => x.Id == model.Id);
            team.Name = model.Name;
            team.LeagueId = model.LeagueId;
            team.IsActive = model.IsActive;

            _context.Teams.Update(team);
            _context.SaveChanges();
        }

        public void TeamDeleteAdmin(Guid id)
        {
            Team team = _context.Teams.Single(x => x.Id == id);
            _context.Teams.Remove(team);
            _context.SaveChanges();
        }

        public void TeamImageDeleteAdmin(Guid id)
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
                Key = id.ToString()
            };

            client.DeleteObjectAsync(request).Wait();
        }

        public void BanUser(Guid id)
        {
            User user = _context.Users.Single(x => x.Id == id);
            user.IsActive = false;
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void UnbanUser(Guid id)
        {
            User user = _context.Users.Single(x => x.Id == id);
            user.IsActive = true;
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
