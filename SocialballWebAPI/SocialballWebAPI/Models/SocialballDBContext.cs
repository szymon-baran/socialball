using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<League> Leagues { get; set; }
        public virtual DbSet<Goal> Goals { get; set; }

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

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Players");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

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

                entity.Property(e => e.Email)
                    .IsUnicode(false);

                entity.Property(e => e.LoginPassword)
                    .IsUnicode(false);

                entity.Property(e => e.LoginUsername)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.HasOne(d => d.Team)
                    .WithMany(p => p.Players)
                    .HasForeignKey(d => d.TeamId)
                    .HasConstraintName("FK_Players_Teams");
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

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.ToTable("Goals");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.Scorer)
                    .WithMany(p => p.GoalScorers)
                    .HasForeignKey(d => d.ScorerId)
                    .HasConstraintName("FK_Goals_Players1");

                entity.HasOne(d => d.AssistPlayer)
                    .WithMany(p => p.GoalAssistPlayers)
                    .HasForeignKey(d => d.AssistPlayerId)
                    .HasConstraintName("FK_Goals_Players");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
