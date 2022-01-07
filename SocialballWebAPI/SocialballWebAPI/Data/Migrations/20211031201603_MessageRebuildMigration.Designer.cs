﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialballWebAPI.Models;

namespace SocialballWebAPI.Migrations
{
    [DbContext(typeof(SocialballDBContext))]
    [Migration("20211031201603_MessageRebuildMigration")]
    partial class MessageRebuildMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Polish_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SocialballWebAPI.Models.League", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Ranking")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid?>("AwayTeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("HomeTeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Stadium")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("HomeTeamId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.MatchEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<int>("MatchEventType")
                        .HasColumnType("int");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Minute")
                        .HasColumnType("int");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.ToTable("MatchEvents");

                    b.HasDiscriminator<int>("MatchEventType").HasValue(0);
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("FromUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("FromUserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Citizenship")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<int?>("Position")
                        .HasColumnType("int");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<Guid?>("LeagueId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<string>("Email")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Password")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("varchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.UserMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ToUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.HasIndex("ToUserId");

                    b.ToTable("UserMessages");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.MatchEventFoul", b =>
                {
                    b.HasBaseType("SocialballWebAPI.Models.MatchEvent");

                    b.Property<int?>("PenaltyType")
                        .HasColumnType("int");

                    b.ToTable("MatchEvents");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("SocialballWebAPI.Models.MatchEventGoal", b =>
                {
                    b.HasBaseType("SocialballWebAPI.Models.MatchEvent");

                    b.Property<Guid?>("AssistPlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("AssistPlayerId");

                    b.ToTable("MatchEvents");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Match", b =>
                {
                    b.HasOne("SocialballWebAPI.Models.Team", "AwayTeam")
                        .WithMany("MatchAwayTeams")
                        .HasForeignKey("AwayTeamId")
                        .HasConstraintName("FK_Matches_Teams1");

                    b.HasOne("SocialballWebAPI.Models.Team", "HomeTeam")
                        .WithMany("MatchHomeTeams")
                        .HasForeignKey("HomeTeamId")
                        .HasConstraintName("FK_Matches_Teams");

                    b.Navigation("AwayTeam");

                    b.Navigation("HomeTeam");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.MatchEvent", b =>
                {
                    b.HasOne("SocialballWebAPI.Models.Match", "Match")
                        .WithMany("MatchEvents")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialballWebAPI.Models.Player", "Player")
                        .WithMany("MatchEvents")
                        .HasForeignKey("PlayerId")
                        .HasConstraintName("FK_MatchGoals_Players1")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Message", b =>
                {
                    b.HasOne("SocialballWebAPI.Models.User", "FromUser")
                        .WithMany("SentMessages")
                        .HasForeignKey("FromUserId")
                        .HasConstraintName("FK_Messages_Users");

                    b.Navigation("FromUser");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Player", b =>
                {
                    b.HasOne("SocialballWebAPI.Models.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .HasConstraintName("FK_Players_Teams");

                    b.HasOne("SocialballWebAPI.Models.User", "User")
                        .WithOne("Player")
                        .HasForeignKey("SocialballWebAPI.Models.Player", "UserId")
                        .HasConstraintName("FK_Users_Players");

                    b.Navigation("Team");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Team", b =>
                {
                    b.HasOne("SocialballWebAPI.Models.League", "League")
                        .WithMany("Teams")
                        .HasForeignKey("LeagueId")
                        .HasConstraintName("FK_Teams_Leagues");

                    b.Navigation("League");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.UserMessage", b =>
                {
                    b.HasOne("SocialballWebAPI.Models.Message", "Message")
                        .WithMany("MessageRecipients")
                        .HasForeignKey("MessageId")
                        .HasConstraintName("FK_UserMessages_Messages")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocialballWebAPI.Models.User", "ToUser")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("ToUserId")
                        .HasConstraintName("FK_UserMessages_Users")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.MatchEventGoal", b =>
                {
                    b.HasOne("SocialballWebAPI.Models.Player", "AssistPlayer")
                        .WithMany("MatchGoalsAssisted")
                        .HasForeignKey("AssistPlayerId")
                        .HasConstraintName("FK_MatchGoals_Players2")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("AssistPlayer");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.League", b =>
                {
                    b.Navigation("Teams");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Match", b =>
                {
                    b.Navigation("MatchEvents");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Message", b =>
                {
                    b.Navigation("MessageRecipients");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Player", b =>
                {
                    b.Navigation("MatchEvents");

                    b.Navigation("MatchGoalsAssisted");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.Team", b =>
                {
                    b.Navigation("MatchAwayTeams");

                    b.Navigation("MatchHomeTeams");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("SocialballWebAPI.Models.User", b =>
                {
                    b.Navigation("Player");

                    b.Navigation("ReceivedMessages");

                    b.Navigation("SentMessages");
                });
#pragma warning restore 612, 618
        }
    }
}