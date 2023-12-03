﻿using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel:ViewModelBase
    {
        public MainViewModel(INavigationViewModel navigationViewModel,IFriendDetailViewModel friendDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            FriendDetailViewModel = friendDetailViewModel;
        }

        public INavigationViewModel NavigationViewModel { get;  }
        public IFriendDetailViewModel FriendDetailViewModel { get;  }

        public async Task Load()
        {
            await NavigationViewModel.LoadAsync();
        }     
                  

    }


}