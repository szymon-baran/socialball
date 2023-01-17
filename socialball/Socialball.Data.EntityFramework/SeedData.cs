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
                    Id = Guid.Parse("9fa33622-933d-4938-8ad2-fe74ede64df8"),
                    Name = "Wilki Kielce",
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
                    Points = 5,
                    IsActive = true
                },
                new Team()
                {
                    Id = Guid.Parse("d7001540-02e8-48d0-94bf-06cc6484d1d1"),
                    Name = "Gwiazda Staszów",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 4).Id,
                    Points = 30,
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
                    Name = "KS Ostrowiec Świętokrzyski",
                    LeagueId = dbContext.Leagues.Single(x => x.Country == "Polska" && x.Ranking == 4).Id,
                    Points = 21,
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
                    Password = "$2a$12$X1.ol7hzKMckfmNoD.SypOhAHyMY0YI0bTlmK8d7.xP.uvJ3txpp6",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "system",
                    Password = "$2a$12$YswdpjKMlfq.uDwMeutIR.14fKKijtcrOE5AfTL8Sgfl/XbhLGzVK",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "kkowal",
                    Password = "$2a$12$qMqcqttFFEjwHRtBmfZeVux/xcYOwgIabVYGmDtm9Dyf2trJoEGaq",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "jnowak",
                    Password = "$2a$12$DrkrqobbU.oA2lr8Xa9Bi.ARRcF8IjvegpWC.coe/EPdeBTnr2Onu",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "nzuk",
                    Password = "$2a$12$X54DkDuZJGgh0fYlDGjhreoAWeHd9s9GFldIPCjdzKBwHXMUuYeL2",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "inowacki",
                    Password = "$2a$12$9K18WQ.W1j7uiJNeUQfQouCrS834I.mmBqCIWq/.eHDwvrWA4z.hS",
                    IsActive = true
                },
                // generic players
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "mprzykladowski",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "kwojownik",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "romarinho",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "lpopovic",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "skowalski",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "mbarometr",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "spluco",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "mmane",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "reast",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "dmichalski",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "trezerwa",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "sdelavega",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "crodriguez",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "jbartkowiak",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "kborowka",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "bfernandes",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "pbielecki",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "kkwiatkowski",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "jbob",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "nsavic",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "mnoric",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "ggundmundson",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "sbarylka",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "wokno",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "cnowy",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "pchudy",
                    Password = "$2a$12$mvMytnJMEFFzR2rK/G0vwOLZ27JF8ozdNfJpf7GQhSTDqGkcT3yBe",
                    IsActive = true
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
                },
                new TeamManager()
                {
                    Id = Guid.Parse("47e0ed3c-94ae-4f51-9fdc-268cb599cb2c"),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Management,
                    FirstName = "Krzysztof",
                    LastName = "Kowal",
                    Citizenship = "Polska",
                    UserId = dbContext.Users.Single(x => x.Username == "kkowal").Id,
                },
                new TeamManager()
                {
                    Id = Guid.Parse("b55c0916-44d9-4bfd-9e59-051810a8b446"),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Management,
                    FirstName = "Jan",
                    LastName = "Nowak",
                    UserId = dbContext.Users.Single(x => x.Username == "jnowak").Id,
                    Citizenship = "Polska",
                },
                new Player()
                {
                    Id = Guid.Parse("72642d18-9c98-4bc6-a9ae-5c8e4206ff0a"),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Norbert",
                    LastName = "Żuk",
                    UserId = dbContext.Users.Single(x => x.Username == "nzuk").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Striker,
                    Earnings = 3800,
                    DateOfBirth = new DateTime(1990, 5, 14, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.Parse("ae673d14-4350-4b2e-b15d-c7a620f5abfe"),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Igor",
                    LastName = "Nowacki",
                    UserId = dbContext.Users.Single(x => x.Username == "inowacki").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Midfielder,
                    Earnings = 1600,
                    DateOfBirth = new DateTime(1998, 8, 24, 12, 0, 0)
                },
                // generic players
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Mariusz",
                    LastName = "Przykładowski",
                    UserId = dbContext.Users.Single(x => x.Username == "mprzykladowski").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Defender,
                    Earnings = 2100,
                    DateOfBirth = new DateTime(1998, 11, 22, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Karol",
                    LastName = "Wojownik",
                    UserId = dbContext.Users.Single(x => x.Username == "kwojownik").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Defender,
                    Earnings = 3400,
                    DateOfBirth = new DateTime(1986, 2, 12, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "",
                    LastName = "Romarinho",
                    UserId = dbContext.Users.Single(x => x.Username == "romarinho").Id,
                    Citizenship = "Brazylia",
                    Position = (int)PositionType.Midfielder,
                    Earnings = 1800,
                    DateOfBirth = new DateTime(1999, 7, 17, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Luka",
                    LastName = "Popović",
                    UserId = dbContext.Users.Single(x => x.Username == "lpopovic").Id,
                    Citizenship = "Chorwacja",
                    Position = (int)PositionType.Goalkeeper,
                    Earnings = 1700,
                    DateOfBirth = new DateTime(2000, 11, 24, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Steve",
                    LastName = "Kowalski",
                    UserId = dbContext.Users.Single(x => x.Username == "skowalski").Id,
                    Citizenship = "USA",
                    Position = (int)PositionType.Striker,
                    Earnings = 2500,
                    DateOfBirth = new DateTime(1991, 7, 11, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Marian",
                    LastName = "Barometr",
                    UserId = dbContext.Users.Single(x => x.Username == "mbarometr").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Defender,
                    Earnings = 2100,
                    IsInjuredUntil = new DateTime(2022, 9, 15, 12, 0, 0),
                    DateOfBirth = new DateTime(1992, 9, 15, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Sebastian",
                    LastName = "Płuco",
                    UserId = dbContext.Users.Single(x => x.Username == "spluco").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Midfielder,
                    Earnings = 1200,
                    DateOfBirth = new DateTime(1996, 1, 11, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Michael",
                    LastName = "Mane",
                    UserId = dbContext.Users.Single(x => x.Username == "mmane").Id,
                    Citizenship = "Senegal",
                    Position = (int)PositionType.Striker,
                    Earnings = 1600,
                    DateOfBirth = new DateTime(1997, 2, 15, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Richard",
                    LastName = "East",
                    UserId = dbContext.Users.Single(x => x.Username == "reast").Id,
                    Citizenship = "USA",
                    Position = (int)PositionType.Defender,
                    Earnings = 1600,
                    DateOfBirth = new DateTime(1982, 4, 21, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Dariusz",
                    LastName = "Michalski",
                    UserId = dbContext.Users.Single(x => x.Username == "dmichalski").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Defender,
                    Earnings = 2000,
                    DateOfBirth = new DateTime(1991, 7, 2, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Tomasz",
                    LastName = "Rezerwa",
                    UserId = dbContext.Users.Single(x => x.Username == "trezerwa").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Goalkeeper,
                    Earnings = 800,
                    DateOfBirth = new DateTime(2001, 2, 17, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Sergio",
                    LastName = "de la Vega",
                    UserId = dbContext.Users.Single(x => x.Username == "sdelavega").Id,
                    Citizenship = "Hiszpania",
                    Position = (int)PositionType.Midfielder,
                    Earnings = 2900,
                    DateOfBirth = new DateTime(1998, 4, 12, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Wilki Kielce" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Carlos",
                    LastName = "Rodriguez",
                    UserId = dbContext.Users.Single(x => x.Username == "crodriguez").Id,
                    Citizenship = "Hiszpania",
                    Position = (int)PositionType.Midfielder,
                    Earnings = 2700,
                    DateOfBirth = new DateTime(1998, 9, 29, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Jan",
                    LastName = "Bartkowiak",
                    UserId = dbContext.Users.Single(x => x.Username == "jbartkowiak").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Goalkeeper,
                    Earnings = 1500,
                    DateOfBirth = new DateTime(2001, 2, 17, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Krzysztof",
                    LastName = "Borówka",
                    UserId = dbContext.Users.Single(x => x.Username == "kborowka").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Defender,
                    Earnings = 1900,
                    DateOfBirth = new DateTime(1982, 11, 25, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Bas",
                    LastName = "Fernandes",
                    UserId = dbContext.Users.Single(x => x.Username == "bfernandes").Id,
                    Citizenship = "Portugalia",
                    Position = (int)PositionType.Midfielder,
                    Earnings = 2100,
                    DateOfBirth = new DateTime(1994, 5, 11, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Piotr",
                    LastName = "Bielecki",
                    UserId = dbContext.Users.Single(x => x.Username == "pbielecki").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Defender,
                    Earnings = 1700,
                    DateOfBirth = new DateTime(1992, 2, 5, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Krzysztof",
                    LastName = "Kwiatkowski",
                    UserId = dbContext.Users.Single(x => x.Username == "kkwiatkowski").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Midfielder,
                    Earnings = 1800,
                    DateOfBirth = new DateTime(1993, 5, 3, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Jan",
                    LastName = "Bób",
                    UserId = dbContext.Users.Single(x => x.Username == "jbob").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Striker,
                    Earnings = 2500,
                    DateOfBirth = new DateTime(1997, 2, 11, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Nemanja",
                    LastName = "Savić",
                    UserId = dbContext.Users.Single(x => x.Username == "nsavic").Id,
                    Citizenship = "Serbia",
                    Position = (int)PositionType.Striker,
                    Earnings = 2300,
                    DateOfBirth = new DateTime(1996, 1, 17, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Mario",
                    LastName = "Norić",
                    UserId = dbContext.Users.Single(x => x.Username == "mnoric").Id,
                    Citizenship = "Chorwacja",
                    Position = (int)PositionType.Defender,
                    Earnings = 1400,
                    DateOfBirth = new DateTime(2000, 4, 12, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Gylfi",
                    LastName = "Gundmundson",
                    UserId = dbContext.Users.Single(x => x.Username == "ggundmundson").Id,
                    Citizenship = "Islandia",
                    Position = (int)PositionType.Defender,
                    Earnings = 2500,
                    DateOfBirth = new DateTime(1989, 12, 5, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Sebastian",
                    LastName = "Baryłka",
                    UserId = dbContext.Users.Single(x => x.Username == "sbarylka").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Striker,
                    Earnings = 1300,
                    IsInjuredUntil = new DateTime(2022, 6, 6, 12, 0, 0),
                    DateOfBirth = new DateTime(2001, 3, 11, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Witold",
                    LastName = "Okno",
                    UserId = dbContext.Users.Single(x => x.Username == "wokno").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Defender,
                    Earnings = 1600,
                    DateOfBirth = new DateTime(1990, 5, 3, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Czesław",
                    LastName = "Nowy",
                    UserId = dbContext.Users.Single(x => x.Username == "cnowy").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Striker,
                    Earnings = 800,
                    DateOfBirth = new DateTime(2001, 9, 12, 12, 0, 0)
                },
                new Player()
                {
                    Id = Guid.NewGuid(),
                    TeamId = dbContext.Teams.First(x => x.Name == "Gwiazda Staszów" && x.IsActive).Id,
                    UserType = UserType.Player,
                    FirstName = "Paweł",
                    LastName = "Chudy",
                    UserId = dbContext.Users.Single(x => x.Username == "pchudy").Id,
                    Citizenship = "Polska",
                    Position = (int)PositionType.Defender,
                    Earnings = 1450,
                    DateOfBirth = new DateTime(1994, 3, 19, 12, 0, 0)
                }
                );
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
