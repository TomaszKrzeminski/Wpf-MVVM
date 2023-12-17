using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.LookUp;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.ViewModel;
using Prism.Events;

namespace FriendOrganizer.UI.Statrup
{
    public class Bootstraper
    {
        public IContainer Bootstrap()
        {
        
            var builder = new ContainerBuilder();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().Keyed<IDetailViewModel>(nameof(FriendDetailViewModel));
            builder.RegisterType<MeetingDetailViewModel>().Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));
            builder.RegisterType<ProgrammingLanguageDetailViewModel>().Keyed<IDetailViewModel>(nameof(ProgrammingLanguageDetailViewModel));
            
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();
            builder.RegisterType<LookUpDataService>().AsImplementedInterfaces();       
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<ProgrammingLanguageRepository>().As<IProgrammingLanguageRepository>();

            return builder.Build();
        }
    }
}
