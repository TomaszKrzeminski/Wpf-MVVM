using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.LookUp;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.ViewModel;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : DetailViewModelBase, IFriendDetailViewModel
    {
        //public IMessageDialogService DialogService { get; }
        public IFriendRepository friendDataService { get; set; }
       
        public IProgrammingLanguageLookUpDataService ProgrammingLanguageLookupDataService { get; set; }
        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; set; }
        public FriendPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }
        public ICommand AddPhoneNumberCommand { get; }
        public ICommand RemovePhoneNumberCommand { get; }
        public FriendDetailViewModel(IMessageDialogService dialogService,IFriendRepository _friendDataService,IEventAggregator eventAggregator,IProgrammingLanguageLookUpDataService programmingLanguageLookupDataService):base(eventAggregator, dialogService)
        {
            //DialogService = dialogService;
            friendDataService = _friendDataService;           
            ProgrammingLanguageLookupDataService = programmingLanguageLookupDataService;
            
            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            eventAggregator.GetEvent<AfterCollectionSavedEvent>().Subscribe(AfterCollectionSaved);
            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
        }

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs obj)
        {
            if(obj.ViewModelName==nameof(ProgrammingLanguageDetailViewModel))
            {
                await LoadProgrammingLookUpLanguagesAsync();
            }
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;

        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            friendDataService.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = friendDataService.HasCHanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

        }

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }

        protected override async void OnDeleteExecute()
        {
            if(await friendDataService.HasMeetingsAsync(Friend.Id))
            {
                MessageDialogService.ShowInfoDialog($"{Friend.FirstName}  {Friend.LastName} cant be delected vecose he is part of the meeting");
                return;
            }



            var result = MessageDialogService.ShowOkCancelDialog($"Do you really want to delete friend {Friend.LastName} {Friend.FirstName} ", "Question");
            if (result == MessageDialogResult.OK)
            {
                friendDataService.Remove(Friend.Model);
                await friendDataService.SaveAsync();
                RaiseDetailDeletedEvent(Friend.Id);

            }
        }

    
        protected override async void OnSaveExecute()
        {

           await SaveWithOptimisticConcurrentcyAsync(friendDataService.SaveAsync, () =>
           {

               HasChanges = friendDataService.HasCHanges();
               Id = Friend.Id;
               RaiseDetailSavedEvent(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");


           });




           
           
            
        }

        protected override bool OnSaveCanExecute()
        {
            return Friend!=null&&!Friend.HasErrors&&HasChanges&&PhoneNumbers.All(x=>!x.HasErrors);
        }

        //private bool _hasChanges;

      


        public override async Task LoadAsync(int friendId)
        {
            var friend = friendId>0? await friendDataService.GetByIdAsync(friendId) : CreateNewFriend();
            Id = friendId;
            InitializeFriend(friend);
            InitializeFriendPhoneNumbers(friend.PhoneNumbers);
            await LoadProgrammingLookUpLanguagesAsync();

        }

        private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();

            foreach (var friendPhoneNumber in phoneNumbers) 
            {
                var wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(!HasChanges)
            {
                HasChanges = friendDataService.HasCHanges();
            }
            if(e.PropertyName==nameof(FriendPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = friendDataService.HasCHanges();
                }

                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if(e.PropertyName==nameof(Friend.FirstName)||e.PropertyName==nameof(Friend.LastName))
                {
                    SetTitle();
                }


            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                Friend.FirstName = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title=$"{Friend.FirstName} {Friend.LastName}";
        }

        private async Task LoadProgrammingLookUpLanguagesAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem() { DisplayMember="-"});
            var lookup = await ProgrammingLanguageLookupDataService.GetProgrammingLanguagesLookkUpAsync();
            foreach (var l in lookup)
            {
                ProgrammingLanguages.Add(l);
            }
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            friendDataService.Add(friend);
            return friend;
        }

       

        private FriendWrapper friend;
        private FriendPhoneNumberWrapper _selectedPhoneNumber;

        public FriendWrapper Friend
        {
            get { return friend; }
            set { friend = value; OnPropertyChanged(); }
        }

       

    }
}
