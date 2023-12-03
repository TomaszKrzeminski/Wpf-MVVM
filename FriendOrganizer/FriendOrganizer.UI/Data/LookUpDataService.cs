using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class LookUpDataService : IFriendLookUpDataService
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

    }
}
