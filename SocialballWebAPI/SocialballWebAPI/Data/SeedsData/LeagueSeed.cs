using Microsoft.EntityFrameworkCore;
using SocialballWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Data.SeedsData
{
    public static class LeagueSeed
    {
        public static void SeedLeagues(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<League>().HasData(
                new League
                {
                    Id = Guid.NewGuid(),
                    Name = "4 liga",
                    Country = "Polska",
                    Ranking = 4
                },
                new League
                {
                    Id = Guid.NewGuid(),
                    Name = "Świętokrzyska klasa okręgowa",
                    Country = "Polska",
                    Ranking = 5
                },
                new League
                {
                    Id = Guid.NewGuid(),
                    Name = "Kielecka klasa A",
                    Country = "Polska",
                    Ranking = 6
                },
                new League
                {
                    Id = Guid.NewGuid(),
                    Name = "Kielecka klasa B",
                    Country = "Polska",
                    Ranking = 7
                }
            );
        }
    }
}
