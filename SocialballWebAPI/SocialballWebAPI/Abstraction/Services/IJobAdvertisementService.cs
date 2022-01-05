using SocialballWebAPI.DTOs;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IJobAdvertisementService
    {
        object GetUserJobAdvertisements(Guid userId);
        object GetTeamJobAdvertisements(Guid userId);
        List<FromTeamJobAdvertisement> GetMyTeamJobAdvertisements(Guid userId);
        object GetJobAdvertisementsAnswers(Guid userId);
        JobAdvertisementDto GetJobAdvertisementDetails(Guid id);
        FromUserJobAdvertisement GetUserJobAdvertisementDetails(Guid userId);
        object GetJobAdvertisementAnswerDetails(Guid id);
        void UpdateUserJobAdvertisement(UserJobAdvertisementDto model);
        void AddMyTeamJobAdvertisement(TeamJobAdvertisementDto model);
        void EditMyTeamJobAdvertisement(TeamJobAdvertisementDto model);
        void AddJobAdvertisementAnswer(JobAdvertisementAnswerDto model);
        void AddResponseToJobAdvertisementAnswer(JobAdvertisementAnswerDto model);
    }
}
