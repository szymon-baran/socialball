using SocialballWebAPI.DTOs;
using SocialballWebAPI.Models;
using SocialballWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IMatchService
    {
        object GetAllMatches();
        object GetTeamMatches(Guid teamId);
        Match GetMatchDetails(Guid id);
    }
}
