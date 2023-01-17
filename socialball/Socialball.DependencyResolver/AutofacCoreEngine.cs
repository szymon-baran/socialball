using Autofac;
using SocialballWebAPI.Abstraction;
using SocialballWebAPI.Data.Repositories;
using SocialballWebAPI.Models;
using SocialballWebAPI.Services;

namespace SocialballWebAPI.Models
{
    public class AutofacCoreEngine : Module
    {
        public AutofacCoreEngine()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SocialballDBContext>().AsSelf().PropertiesAutowired().InstancePerLifetimeScope();

            builder.RegisterType<AdminService>().As<IAdminService>().InstancePerLifetimeScope();
            builder.RegisterType<AdminService>().As<AdminService>().InstancePerLifetimeScope();

            builder.RegisterType<JobAdvertisementService>().As<IJobAdvertisementService>().InstancePerLifetimeScope();
            builder.RegisterType<JobAdvertisementService>().As<JobAdvertisementService>().InstancePerLifetimeScope();

            builder.RegisterType<MatchService>().As<MatchService>().InstancePerLifetimeScope();
            builder.RegisterType<MatchService>().As<MatchService>().InstancePerLifetimeScope();

            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageService>().As<MessageService>().InstancePerLifetimeScope();

            builder.RegisterType<PlayerService>().As<IPlayerService>().InstancePerLifetimeScope();
            builder.RegisterType<PlayerService>().As<PlayerService>().InstancePerLifetimeScope();

            builder.RegisterType<PlayerTransferOfferService>().As<IPlayerTransferOfferService>().InstancePerLifetimeScope();
            builder.RegisterType<PlayerTransferOfferService>().As<PlayerTransferOfferService>().InstancePerLifetimeScope();

            builder.RegisterType<TeamService>().As<ITeamService>().InstancePerLifetimeScope();
            builder.RegisterType<TeamService>().As<TeamService>().InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<UserService>().InstancePerLifetimeScope();

            builder.RegisterType<JobAdvertisementAnswerRepository>().As<IJobAdvertisementAnswerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JobAdvertisementAnswerRepository>().As<JobAdvertisementAnswerRepository>().InstancePerLifetimeScope();

            builder.RegisterType<JobAdvertisementRepository>().As<IJobAdvertisementRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JobAdvertisementRepository>().As<JobAdvertisementRepository>().InstancePerLifetimeScope();

            builder.RegisterType<LeagueRepository>().As<ILeagueRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LeagueRepository>().As<LeagueRepository>().InstancePerLifetimeScope();

            builder.RegisterType<MatchEventRepository>().As<IMatchEventRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MatchEventRepository>().As<MatchEventRepository>().InstancePerLifetimeScope();

            builder.RegisterType<MatchPlayerRepository>().As<IMatchPlayerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MatchPlayerRepository>().As<MatchPlayerRepository>().InstancePerLifetimeScope();

            builder.RegisterType<MatchRepository>().As<IMatchRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MatchRepository>().As<MatchRepository>().InstancePerLifetimeScope();

            builder.RegisterType<MessageRepository>().As<IMessageRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MessageRepository>().As<MessageRepository>().InstancePerLifetimeScope();

            builder.RegisterType<PlayerRepository>().As<IPlayerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PlayerRepository>().As<PlayerRepository>().InstancePerLifetimeScope();

            builder.RegisterType<PlayerTransferOfferRepository>().As<IPlayerTransferOfferRepository>().InstancePerLifetimeScope();
            builder.RegisterType<PlayerTransferOfferRepository>().As<PlayerTransferOfferRepository>().InstancePerLifetimeScope();

            builder.RegisterType<TeamRepository>().As<ITeamRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TeamRepository>().As<TeamRepository>().InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<UserRepository>().InstancePerLifetimeScope();
        }
    }

}
