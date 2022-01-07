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
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;

        public AdminService(IUserRepository userRepository, ITeamRepository teamRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
        }

        private bool HasImage(string name)
        {
            WebRequest webRequest = WebRequest.Create(name);
            WebResponse webResponse;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch //If exception thrown then couldn't get response from address
            {
                return false;
            }

            return true;
        }

        public object GetUsers()
        {
            return _userRepository.AdminGetUsers().Select(x => new
            {
                x.Id,
                Name = $"{x.UserData.LastName} {x.UserData.FirstName}",
                x.Username,
                x.UserData.UserType,
                x.IsActive,
                UserDataId = x.UserData.Id,
                x.Email
                //HasImage = HasImage("https://socialball-avatars.s3.eu-central-1.amazonaws.com/" + x.UserData.Id.ToString())
            }).ToList();

        }

        public object GetTeams(Guid? leagueId)
        {
            return _teamRepository.AdminGetTeamsByLeague(leagueId);
        }

        public void EditTeam(AdminEditTeamDto model)
        {
            Team team = _teamRepository.GetTeamDetails(model.Id);
            team.Name = model.Name;
            team.LeagueId = model.LeagueId;
            team.IsActive = model.IsActive;

            _teamRepository.UpdateTeam(team);
        }

        public void TeamDeleteAdmin(Guid id)
        {
            Team team = _teamRepository.GetTeamDetails(id);

            _teamRepository.RemoveTeam(team);
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
            User user = _userRepository.GetUserDetails(id);
            user.IsActive = false;
            _userRepository.UpdateUser(user);
        }

        public void UnbanUser(Guid id)
        {
            User user = _userRepository.GetUserDetails(id);
            user.IsActive = true;
            _userRepository.UpdateUser(user);
        }
    }
}
