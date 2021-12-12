using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models.Configuration
{
    public static class LeagueConfiguration
    {
        public static void Seed(EntityTypeBuilder<League> builder)
        {
            builder.HasData(
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
