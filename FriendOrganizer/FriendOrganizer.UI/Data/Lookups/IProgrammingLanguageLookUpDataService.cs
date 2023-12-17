using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.LookUp
{
    public interface IProgrammingLanguageLookUpDataService
    {        
        Task<IEnumerable<LookupItem>> GetProgrammingLanguagesLookkUpAsync();
    }
}