using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Helpers;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using SocialballWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SocialballWebAPI.Abstraction;

namespace SocialballWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService UserService;

        public UsersController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpPost("login")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = UserService.Authenticate(model);

            if (response == null)
                return Ok(new Exception("Nazwa użytkownika lub hasło jest niepoprawne!"));

            return Ok(response);
        }
    }
}
