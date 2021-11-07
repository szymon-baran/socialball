using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SocialballWebAPI.Enums;

#nullable disable

namespace SocialballWebAPI.Models
{
    public partial class SocialballDBContext : DbContext
    {
        public SocialballDBContext()
        {
        }

        public SocialballDBContext(DbContextOptions<SocialballDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserData> UserDatas { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<TeamManager> TeamManagers { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<League> Leagues { get; set; }
        public virtual DbSet<MatchEvent> MatchEvents { get; set; }
        public virtual DbSet<MatchEventFoul> MatchEventFouls { get; set; }
        public virtual DbSet<MatchEventGoal> MatchEventGoals { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<UserMessage> UserMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=SocialballDB ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>(entity =>
            {
                entity.ToTable("Matches");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.AwayTeam)
                    .WithMany(p => p.MatchAwayTeams)
                    .HasForeignKey(d => d.AwayTeamId)
                    .HasConstraintName("FK_Matches_Teams1");

                entity.HasOne(d => d.HomeTeam)
                    .WithMany(p => p.MatchHomeTeams)
                    .HasForeignKey(d => d.HomeTeamId)
                    .HasConstraintName("FK_Matches_Teams");
            });

            modelBuilder.Entity<UserData>(entity =>
            {
                entity.ToTable("UserDatas");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasDiscriminator(b => b.UserType)
                    .HasValue<UserData>(0)
                    .HasValue<Player>(UserType.Zawodnik)
                    .HasValue<TeamManager>(UserType.Sztab);

                entity.Property(e => e.Citizenship)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.UserDatas)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_UserDatas_Teams");
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("UserDatas");
            });

            modelBuilder.Entity<TeamManager>(entity =>
            {
                entity.ToTable("UserDatas");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Email)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.UserData)
                    .WithOne(p => p.User)
                    .HasForeignKey<UserData>(d => d.UserId)
                    .HasConstraintName("FK_Users_UserDatas");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Teams");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.League)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.LeagueId)
                    .HasConstraintName("FK_Teams_Leagues");
            });

            modelBuilder.Entity<League>(entity =>
            {
                entity.ToTable("Leagues");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<MatchEvent>(entity =>
            {
                entity.ToTable("MatchEvents");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasDiscriminator(b => b.MatchEventType)
                    .HasValue<MatchEvent>(0)
                    .HasValue<MatchEventGoal>(MatchEventType.Goal)
                    .HasValue<MatchEventFoul>(MatchEventType.Foul);

                entity.HasOne(d => d.Player)
                    .WithMany(p => p.MatchEvents)
                    .HasForeignKey(d => d.PlayerId)
                    .HasConstraintName("FK_MatchGoals_Players1")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MatchEventGoal>(entity =>
            {
                entity.ToTable("MatchEvents");

                entity.HasOne(d => d.AssistPlayer)
                    .WithMany(p => p.MatchGoalsAssisted)
                    .HasForeignKey(d => d.AssistPlayerId)
                    .HasConstraintName("FK_MatchGoals_Players2")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<MatchEventFoul>(entity =>
            {
                entity.ToTable("MatchEvents");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Messages");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Subject)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.SentMessages)
                    .HasForeignKey(d => d.FromUserId)
                    .HasConstraintName("FK_Messages_Users");
            });

            modelBuilder.Entity<UserMessage>(entity =>
            {
                entity.ToTable("UserMessages");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ReceivedMessages)
                    .HasForeignKey(d => d.ToUserId)
                    .HasConstraintName("FK_UserMessages_Users");

                entity.HasOne(d => d.Message)
                    .WithMany(p => p.MessageRecipients)
                    .HasForeignKey(d => d.MessageId)
                    .HasConstraintName("FK_UserMessages_Messages");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
