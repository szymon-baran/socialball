using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.DTOs;
using SocialballWebAPI.Enums;
using SocialballWebAPI.Extensions;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobAdvertisementsController : ControllerBase
    {
        private IJobAdvertisementService JobAdvertisementService;
        private ITeamService TeamService;

        public JobAdvertisementsController(IJobAdvertisementService jobAdvertisementService, ITeamService teamService)
        {
            JobAdvertisementService = jobAdvertisementService;
            TeamService = teamService;
        }

        [HttpGet("getUserJobAdvertisements")]
        public ActionResult<IEnumerable<FromTeamJobAdvertisement>> GetUserJobAdvertisements(Guid userId)
        {
            return Ok(JobAdvertisementService.GetUserJobAdvertisements(userId));
        }

        [HttpGet("getTeamJobAdvertisements")]
        public ActionResult GetTeamJobAdvertisements(Guid userId)
        {
            return Ok(JobAdvertisementService.GetTeamJobAdvertisements(userId));
        }

        [HttpGet("getMyTeamJobAdvertisements")]
        public ActionResult GetMyTeamJobAdvertisements(Guid userId)
        {
            return Ok(JobAdvertisementService.GetMyTeamJobAdvertisements(userId));
        }

        [HttpGet("getJobAdvertisementsAnswers")]
        public ActionResult GetJobAdvertisementsAnswers(Guid userId)
        {
            return Ok(JobAdvertisementService.GetJobAdvertisementsAnswers(userId));
        }

        [HttpGet("details")]
        public ActionResult<JobAdvertisementDto> Details(Guid id)
        {
            return Ok(JobAdvertisementService.GetJobAdvertisementDetails(id));
        }

        [HttpGet("getUserJobAdvertisementDetails")]
        public ActionResult<FromUserJobAdvertisement> GetUserJobAdvertisementDetails(Guid userId)
        {
            return Ok(JobAdvertisementService.GetUserJobAdvertisementDetails(userId));
        }

        [HttpGet("getJobAdvertisementAnswerDetails")]
        public ActionResult GetJobAdvertisementAnswerDetails(Guid id)
        {
            return Ok(JobAdvertisementService.GetJobAdvertisementAnswerDetails(id));
        }


        [HttpPost("updateUserJobAdvertisement")]
        public ActionResult UpdateUserJobAdvertisement([FromBody] UserJobAdvertisementDto model)
        {
            JobAdvertisementService.UpdateUserJobAdvertisement(model);

            return Ok();
        }

        [HttpPost("addMyTeamJobAdvertisement")]
        public ActionResult AddMyTeamJobAdvertisement([FromBody] TeamJobAdvertisementDto model)
        {
            JobAdvertisementService.AddMyTeamJobAdvertisement(model);

            return Ok();
        }

        [HttpPost("editMyTeamJobAdvertisement")]
        public ActionResult EditMyTeamJobAdvertisement([FromBody] TeamJobAdvertisementDto model)
        {
            JobAdvertisementService.EditMyTeamJobAdvertisement(model);

            return Ok();
        }

        [HttpPost("addJobAdvertisementAnswer")]
        public ActionResult AddJobAdvertisementAnswer([FromBody] JobAdvertisementAnswerDto model)
        {
            JobAdvertisementService.AddJobAdvertisementAnswer(model);
            return Ok();
        }

        [HttpPost("updateJobAdvertisementAnswer")]
        public ActionResult UpdateJobAdvertisementAnswer([FromBody] JobAdvertisementAnswerDto model)
        {
            JobAdvertisementService.AddResponseToJobAdvertisementAnswer(model);
            return Ok();
        }

        [HttpGet("getTeamsToLookup")]
        public ActionResult GetTeamsToLookup()
        {
            return Ok(TeamService.GetTeamsToSelectList());
        }

        [HttpGet("getPositionsToLookup")]
        public ActionResult GetPositionsToLookup()
        {
            return Ok(EnumExtensions.GetValues<PositionType>());
        }
    }
}
