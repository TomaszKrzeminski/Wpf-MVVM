using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.LookUp
{
    public class LookUpDataService : IFriendLookUpDataService, ILookUpDataService, IProgrammingLanguageLookUpDataService,IMeetingLookUpDataService
    {
        public LookUpDataService(Func<FriendOrganizerDbContext> dbContext)
        {
            DbContext = dbContext;
        }

        public Func<FriendOrganizerDbContext> DbContext { get; set; }

        public async Task<IEnumerable<LookupItem>> GetFriendLookUpAsync()
        {
            using (var ctx = DbContext())
            {
                return await ctx.Friends.AsNoTracking().Select(x => new LookupItem() { Id = x.Id, DisplayMember = x.FirstName + " " + x.LastName }).ToListAsync();
            }
        }

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguagesLookkUpAsync()
        {
            using (var ctx = DbContext())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking().Select(x => new LookupItem() { Id = x.Id, DisplayMember = x.Name }).ToListAsync();
            }
        }

        public async Task<List<LookupItem>> GetMeetingLookUpAsync()
        {
            using (var ctx = DbContext())
            {
                return await ctx.Meetings.AsNoTracking().Select(x => 
                new LookupItem() { Id = x.Id, DisplayMember = x.Title}).ToListAsync();
            }
        }

    }
}
