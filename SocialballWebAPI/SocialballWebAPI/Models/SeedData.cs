using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialballWebAPI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialballWebAPI.Models
{
    public class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            var dbContext = provider.GetRequiredService<SocialballDBContext>();
            await dbContext.Database.MigrateAsync();

            if (!await dbContext.Leagues.AnyAsync())
            {
                await dbContext.Leagues.AddRangeAsync(
                new League()
                {
                    Id = Guid.NewGuid(),
                    Name = "4 liga świętokrzyska",
                    Country = "Polska",
                    Ranking = 4
                },
                new League()
                {
                    Id = Guid.NewGuid(),
                    Name = "Świętokrzyska klasa okręgowa",
                    Country = "Polska",
                    Ranking = 5
                },
                new League()
                {
                    Id = Guid.NewGuid(),
                    Name = "Kielecka klasa A",
                    Country = "Polska",
                    Ranking = 6
                }
                );
                await dbContext.SaveChangesAsync();
            }
            if (!await dbContext.Teams.AnyAsync())
            {
                await dbContext.Teams.AddRangeAsync(
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "FC Kielce",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 4).Id,
                    Points = 36,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "MKS Jędrzejów",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 4).Id,
                    Points = 17,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Zachód Busko-Zdrój",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 4).Id,
                    Points = 21,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Polonia Staszów",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 4).Id,
                    Points = 5,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Stal Pińczów",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 4).Id,
                    Points = 8,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "MKS Ostrowiec Świętokrzyski",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 4).Id,
                    Points = 30,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "KS Skarżysko-Kamienna",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 5).Id,
                    Points = 12,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Silnica Kielce",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 5).Id,
                    Points = 29,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Lech Daleszyce",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 5).Id,
                    Points = 27,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "PKS Borków",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 5).Id,
                    Points = 8,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Czarni Strawczyn",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 5).Id,
                    Points = 11,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Słabi Kielce",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 6).Id,
                    Points = 0,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Sparta Końskie",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 6).Id,
                    Points = 15,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Wojownicy Morawica",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 6).Id,
                    Points = 22,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "Planeta Starachowice",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 6).Id,
                    Points = 6,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.NewGuid(),
                    Name = "MKS Chęciny",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 6).Id,
                    Points = 11,
                    IsActive = true
                }
                );
                await dbContext.SaveChangesAsync();
            }
            if (!await dbContext.Users.AnyAsync())
            {
                await dbContext.Users.AddRangeAsync(
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    Password = "$2a$12$X1.ol7hzKMckfmNoD.SypOhAHyMY0YI0bTlmK8d7.xP.uvJ3txpp6"
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "system",
                    Password = "$2a$12$YswdpjKMlfq.uDwMeutIR.14fKKijtcrOE5AfTL8Sgfl/XbhLGzVK"
                }
                );
                await dbContext.SaveChangesAsync();
            }
            if (!await dbContext.UserDatas.AnyAsync())
            {
                await dbContext.UserDatas.AddRangeAsync(
                new AdminUser()
                {
                    Id = Guid.NewGuid(),
                    UserType = UserType.Admin,
                    FirstName = "",
                    LastName = "Administrator",
                    UserId = dbContext.Users.Single(x => x.Username == "admin").Id,
                },
                new SystemUser()
                {
                    Id = Guid.NewGuid(),
                    UserType = UserType.System,
                    FirstName = "",
                    LastName = "System",
                    UserId = dbContext.Users.Single(x => x.Username == "system").Id,
                }
                );
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
