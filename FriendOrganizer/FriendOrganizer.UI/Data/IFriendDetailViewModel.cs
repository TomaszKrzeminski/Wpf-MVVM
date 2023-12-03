using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public interface IFriendDetailViewModel
    {
        Task LoadAsync(int friendId);
    }
}