using Autofac.Features.Indexed;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Events;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel:ViewModelBase
    {
        public MainViewModel(IMessageDialogService messageBoxService,INavigationViewModel navigationViewModel, IIndex<string,IDetailViewModel> detailViewModelCreator, IEventAggregator eventAggregator)
        {
            MessageBoxService = messageBoxService;
            NavigationViewModel = navigationViewModel;
            this.detailViewModelCreator = detailViewModelCreator;
           
            EventAggregator = eventAggregator;
            eventAggregator.GetEvent<OpenDetailViewEvent>().Subscribe(OnOpenDetailView);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            eventAggregator.GetEvent<AfterDetailClosedEvent>().Subscribe(AfterDetailClosed);
            OpenSingleDetailViewModel = new DelegateCommand<Type>(OnOpenSingleDetailExecute);
            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            DetailViewModels = new ObservableCollection<IDetailViewModel>();
        }

        

        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id,args.ViewModelName);

        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id,args.ViewModelName);
        }

        private void RemoveDetailViewModel(int Id,string viewModelName)
        {
            var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == Id && vm.GetType().Name == viewModelName);
            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        private int nextNewItemId = 0;
        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView( new OpenDetailViewEventArgs() {Id=nextNewItemId--,ViewModelName=viewModelType.Name });
        }

        public ObservableCollection<IDetailViewModel> DetailViewModels { get;  }
        public ICommand CreateNewDetailCommand { get; }
        public ICommand OpenSingleDetailViewModel { get;  }
        public IMessageDialogService MessageBoxService { get; set; }
        public INavigationViewModel NavigationViewModel { get; set; }

        private IDetailViewModel _selecteddetailViewModel;
        private readonly IIndex<string, IDetailViewModel> detailViewModelCreator;

        public IDetailViewModel SelectedDetailViewModel
        {
            get { return _selecteddetailViewModel; }
            set { _selecteddetailViewModel = value;OnPropertyChanged(); }
        }

               
       
        public IEventAggregator EventAggregator { get; set; }

        public async Task Load()
        {
            await NavigationViewModel.LoadAsync();
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            //if(SelectedDetailViewModel!=null&&SelectedDetailViewModel.HasChanges)
            //{
            //  var result=  MessageBoxService.ShowOkCancelDialog("You have made changes.Nabigate away?", "Question");
            //    if(result==MessageDialogResult.Cancel) { return; }                
            //}

           var detailViewModel= DetailViewModels.SingleOrDefault(vm=>vm.Id==args.Id&&vm.GetType().Name==args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = detailViewModelCreator[args.ViewModelName];
                try
                {
await detailViewModel.LoadAsync(args.Id);
                }
                catch (Exception)
                {

                    MessageBoxService.ShowInfoDialog("Entity was deleted navigation has refreshed");
                    await NavigationViewModel.LoadAsync();
                }
                
                DetailViewModels.Add(detailViewModel);
            }

            SelectedDetailViewModel = detailViewModel;           
            
        }

        private void OnOpenSingleDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs() {Id=-1,  ViewModelName = viewModelType.Name });
        }

    }


}
