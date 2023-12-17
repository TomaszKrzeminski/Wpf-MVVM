using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.LookUp
{
    public interface IFriendLookUpDataService
    {
        Task<IEnumerable<LookupItem>> GetFriendLookUpAsync();
    }
}