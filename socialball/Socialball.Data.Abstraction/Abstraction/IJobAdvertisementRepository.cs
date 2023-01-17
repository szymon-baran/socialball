using SocialballWebAPI.Enums;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Abstraction
{
    public interface IJobAdvertisementRepository
    {
        List<FromTeamJobAdvertisement> GetFromTeamJobAdvertisementsByPosition(PositionType? positionType);
        List<FromTeamJobAdvertisement> GetFromTeamJobAdvertisementsByTeamId(Guid? teamId);
        List<FromUserJobAdvertisement> GetFromUserJobAdvertisements();
        List<FromUserJobAdvertisement> GetFromUserJobAdvertisementsByUserId(Guid userId);
        JobAdvertisement GetJobAdvertisementDetails(Guid id);
        FromTeamJobAdvertisement GetFromTeamJobAdvertisementDetails(Guid id);
        FromUserJobAdvertisement GetFromUserJobAdvertisementDetails(Guid id);
        FromUserJobAdvertisement GetFromUserJobAdvertisementDetailsByUserId(Guid userId);
        void AddFromUserJobAdvertisement(FromUserJobAdvertisement fromUserJobAdvertisement);
        void UpdateFromUserJobAdvertisement(FromUserJobAdvertisement fromUserJobAdvertisement);
        void AddFromTeamJobAdvertisement(FromTeamJobAdvertisement fromTeamJobAdvertisement);
        void EditFromTeamJobAdvertisement(FromTeamJobAdvertisement fromTeamJobAdvertisement);
        void RemoveFromUserJobAdvertisementsRange(List<FromUserJobAdvertisement> fromUserJobAdvertisements);
    }
}
