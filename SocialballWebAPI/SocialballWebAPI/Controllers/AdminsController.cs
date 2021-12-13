using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private IAdminService AdminService;

        public AdminsController(IAdminService service)
        {
            AdminService = service;
        }

        [HttpGet("getUsersList")]
        public ActionResult<IEnumerable<Player>> GetPlayers(Guid? teamId)
        {
            return Ok(AdminService.GetUsers());
        }
    }
}
