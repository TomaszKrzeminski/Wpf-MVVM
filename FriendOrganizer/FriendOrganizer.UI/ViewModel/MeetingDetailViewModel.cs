using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MeetingDetailViewModel :DetailViewModelBase, IMeetingDetailViewModel
    {

        public MeetingDetailViewModel(IEventAggregator eventAggregator,IMeetingRepository meetingRepository,IMessageDialogService messageDialogService):base(eventAggregator, messageDialogService)
        {            
            this.meetingRepository = meetingRepository;

            eventAggregator.GetEvent<AfterDetailSaveEvent>().Subscribe(AfterDetailSaved);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            AddedFriends = new ObservableCollection<Friend>();          
            AvailableFriends = new ObservableCollection<Friend>();

            AddFriendCommand = new DelegateCommand(OnAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);

        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(FriendDetailViewModel))
            {
                
                _allFriends = await meetingRepository.GetAllFriendsAsync();
                SetupPicklist();
            }
        }

        private async void AfterDetailSaved(AfterSaveDetailEventArgs args)
        {
            if(args.ViewModelName==nameof(FriendDetailViewModel))
            {
                await meetingRepository.ReloadFriendAsync(args.Id);
                _allFriends = await meetingRepository.GetAllFriendsAsync();
                SetupPicklist();
            }
        }

        private bool OnRemoveFriendCanExecute()
        {
            return SelectedAddedFriend != null;
        }

        private void OnRemoveFriendExecute()
        {
            var friendToRemove = SelectedAddedFriend;

            Meeting.Model.Friends.Remove(friendToRemove);
            AddedFriends.Remove(friendToRemove);
            AvailableFriends.Add(friendToRemove);
            HasChanges = meetingRepository.HasCHanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddFriendCanExecute()
        {
            return SelectedAvailableFriend != null;
        }

        private void OnAddFriendExecute()
        {
            var friendToAdd = SelectedAvailableFriend;

            Meeting.Model.Friends.Add(friendToAdd);
            AddedFriends.Add(friendToAdd);
            AvailableFriends.Remove(friendToAdd);
            HasChanges = meetingRepository.HasCHanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public ICommand AddFriendCommand { get; set; }
        public ICommand RemoveFriendCommand { get; set; }

        private Friend _selectedAvailableFriend;

        public Friend SelectedAvailableFriend
        {
            get { return _selectedAvailableFriend; }
            set 
            {   _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)AddFriendCommand).RaiseCanExecuteChanged();
            
            }
        }


        private Friend _selectedAddedFriend;

        public Friend SelectedAddedFriend
        {
            get { return _selectedAddedFriend; }
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged();

            }
        }





        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }

       

       
        private readonly IMeetingRepository meetingRepository;
        private MeetingWrapper MeetingWrapper;
       

        public ObservableCollection<Friend> AddedFriends { get; private set; }
        public ObservableCollection<Friend> AvailableFriends { get; private set; }

        private MeetingWrapper _meeting;
        private List<Friend> _allFriends;

        public override async Task LoadAsync(int meetingId)
        {
            // var meeting=meetingId.HasValue ?await meetingRepository.GetByIdAsync(meetingId.Value) : CreateNewMeeting();
            // Id = meeting.Id;
            // InitializeMeeting(meeting);
            //_allFriends=await meetingRepository.GetAllFriendsAsync();
            // SetupPicklist();


            var meeting = meetingId>0 ? await meetingRepository.GetByIdAsync(meetingId) : CreateNewMeeting();
            Id = meetingId;
            InitializeMeeting(meeting);
            _allFriends = await meetingRepository.GetAllFriendsAsync();
            SetupPicklist();

        }

        private void SetupPicklist()
        {
            var meetingFriendIds=Meeting.Model.Friends.Select(f=>f.Id).ToList();
            var addedFriends = _allFriends.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availableFriends = _allFriends.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear(); 
            AvailableFriends.Clear();

            foreach (var added in addedFriends)
            {
                AddedFriends.Add(added);
            }

            foreach (var aval in availableFriends)
            {
                AvailableFriends.Add(aval);
            }

        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = meetingRepository.HasCHanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if(e.PropertyName==nameof(Meeting.Title))
                {
                    SetTitle();
                }

            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if(Meeting.Id==0)
            {
                Meeting.Title = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = Meeting.Title;
        }


        protected override void OnDeleteExecute()
        {
            var result = MessageDialogService.ShowOkCancelDialog($"Do you really want to delete meeting","Question");
            if(result==MessageDialogResult.OK)
            {
                meetingRepository.Remove(Meeting.Model);
                meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);

            }

        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await meetingRepository.SaveAsync();
            HasChanges = meetingRepository.HasCHanges();
            Id=Meeting.Id;  
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            meetingRepository.Add(meeting);
            return meeting;
        }
       
    }
}

//17/10