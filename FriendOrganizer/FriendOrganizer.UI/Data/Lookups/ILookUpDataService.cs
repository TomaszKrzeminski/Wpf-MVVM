using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.LookUp
{
    public interface ILookUpDataService
    {
        Func<FriendOrganizerDbContext> DbContext { get; set; }

        Task<IEnumerable<LookupItem>> GetFriendLookUpAsync();
        Task<IEnumerable<LookupItem>> GetProgrammingLanguagesLookkUpAsync();
    }
}