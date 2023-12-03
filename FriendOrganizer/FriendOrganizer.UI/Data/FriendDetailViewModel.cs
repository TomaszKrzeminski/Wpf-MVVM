using FriendOrganizer.Model;
using FriendOrganizer.UI.ViewModel;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        public IFriendDataService friendDataService { get; set; }
        public FriendDetailViewModel(IFriendDataService _friendDataService)
        {
            friendDataService = _friendDataService;
        }

        public async Task LoadAsync(int friendId)
        {
            Friend = await friendDataService.GetByIdAsync(friendId);
        }

        private Friend friend;

        public Friend Friend
        {
            get { return friend; }
            set { friend = value; OnPropertyChanged(); }
        }


    }
}
