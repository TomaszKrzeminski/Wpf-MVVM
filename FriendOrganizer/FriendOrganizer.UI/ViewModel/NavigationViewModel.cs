using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.LookUp;
using FriendOrganizer.UI.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel :ViewModelBase, INavigationViewModel
    {
        private IFriendLookUpDataService _friendLookupService { get; set; }
        public IEventAggregator EventAggregator { get; set; }
        public IMeetingLookUpDataService MeetingLookUpDataService { get; set; }
        public ObservableCollection<NavigationItemViewModel> Friends { get; set; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; set; }

        public NavigationViewModel(IFriendLookUpDataService friendLookupService,IEventAggregator eventAggregator,IMeetingLookUpDataService meetingLookUpDataService)
        {
            _friendLookupService = friendLookupService;
            EventAggregator = eventAggregator;
            MeetingLookUpDataService = meetingLookUpDataService;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            eventAggregator.GetEvent<AfterDetailSaveEvent>().Subscribe(AfterDetailSave);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);

        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {

            switch(args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailDeleted(Friends, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;
            }
                       
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterDetailSave(AfterSaveDetailEventArgs args)
        {
            switch(args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailSaved(Friends, args);
                    break;

               case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, args);
                    break;

            }
           
            
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterSaveDetailEventArgs args)
        {
            var lookupFriend = items.SingleOrDefault(x => x.Id == args.Id);
            if (lookupFriend == null)
            {
                Friends.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, EventAggregator, args.ViewModelName));
            }
            else
            {
                lookupFriend.DisplayMember = args.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _friendLookupService.GetFriendLookUpAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id,item.DisplayMember,EventAggregator, nameof(FriendDetailViewModel)));
            }

            var meetings = await MeetingLookUpDataService.GetMeetingLookUpAsync();
            Meetings.Clear();
            foreach (var item in meetings)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, EventAggregator, nameof(MeetingDetailViewModel)));
            }

        }

    }
}
