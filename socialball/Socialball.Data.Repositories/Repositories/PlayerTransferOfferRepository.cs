using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.Repositories
{
    public class PlayerTransferOfferRepository : IPlayerTransferOfferRepository
    {
        private readonly SocialballDBContext _context;

        public PlayerTransferOfferRepository(SocialballDBContext context)
        {
            _context = context;
        }

        public List<PlayerTransferOffer> GetToTeamTransferOffers(Guid toTeamId)
        {
            return _context.PlayerTransferOffers.Where(x => x.ToTeamId == toTeamId).Include(x => x.Player).Include(x => x.FromTeam).Include(x => x.ToTeam).ToList();
        }

        public List<PlayerTransferOffer> GetFromTeamTransferOffers(Guid fromTeamId)
        {
            return _context.PlayerTransferOffers.Where(x => x.FromTeamId == fromTeamId).Include(x => x.Player).Include(x => x.FromTeam).Include(x => x.ToTeam).ToList();
        }

        public List<PlayerTransferOffer> GetPlayerTransferOffers(Guid playerId)
        {
            return _context.PlayerTransferOffers.Where(x => x.PlayerId == playerId).Include(x => x.Player).Include(x => x.FromTeam).Include(x => x.ToTeam).ToList();
        }

        public PlayerTransferOffer GetPlayerTransferOfferDetails(Guid id)
        {
            return _context.PlayerTransferOffers.Include(x => x.FromTeam).Include(x => x.ToTeam).Include(x => x.Player).Single(x => x.Id == id);
        }

        public void AddPlayerTransferOffer(PlayerTransferOffer playerTransferOffer)
        {
            _context.PlayerTransferOffers.Add(playerTransferOffer);
            _context.SaveChanges();
        }

        public void UpdatePlayerTransferOffer(PlayerTransferOffer playerTransferOffer)
        {
            _context.PlayerTransferOffers.Update(playerTransferOffer);
            _context.SaveChanges();
        }

        public void RemovePlayerTransferOffer(PlayerTransferOffer playerTransferOffer)
        {
            _context.PlayerTransferOffers.Remove(playerTransferOffer);
            _context.SaveChanges();
        }

    }
}
