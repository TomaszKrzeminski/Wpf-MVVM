﻿using Autofac;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.ViewModel;

namespace FriendOrganizer.UI.Statrup
{
    public class Bootstraper
    {
        public IContainer Bootstrap()
        {
        
            var builder = new ContainerBuilder();

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().As<IFriendDetailViewModel>();
            
            builder.RegisterType<LookUpDataService>().AsImplementedInterfaces();       
                 builder.RegisterType<FriendDataService>().As<IFriendDataService>();
                     
     
            
            return builder.Build();
        }
    }
}
