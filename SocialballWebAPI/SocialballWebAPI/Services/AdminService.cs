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
            return _context.Users.Include(x => x.UserData).Where(x => x.UserData.UserType != UserType.System).OrderBy(x => x.UserData.UserType).ToList();
        }

    }
}
