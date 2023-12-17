using FriendOrganizer.UI.Events;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationItemViewModel:ViewModelBase
    {
        private string _displayMember;
        public NavigationItemViewModel(int Id,string displayMember,IEventAggregator eventAggregator,string detailViewModelName) 
        {
            this.Id = Id;
            DisplayMember = displayMember;
            EventAggregator = eventAggregator;
            DetailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        private void OnOpenDetailViewExecute()
        {
            EventAggregator.GetEvent<OpenDetailViewEvent>().Publish(new OpenDetailViewEventArgs() { Id=Id,ViewModelName=DetailViewModelName});
        }

        public ICommand OpenDetailViewCommand { get; }
        

        public int Id { get; set; }
        

        public string DisplayMember
        {
            get { return _displayMember; }
            set { _displayMember = value;OnPropertyChanged(); }
        }

        public IEventAggregator EventAggregator { get; set; }
        public string DetailViewModelName { get; }
    }
}
