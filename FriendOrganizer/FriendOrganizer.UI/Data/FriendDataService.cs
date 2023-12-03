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
    public class FriendDataService : IFriendDataService
    {
        private  Func<FriendOrganizerDbContext> context;

        public FriendDataService(Func<FriendOrganizerDbContext> context)
        {
            this.context = context;
        }
        public async Task<Friend> GetByIdAsync(int friendId)
        {
           using(var ctx= context())
            {
                return await ctx.Friends.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==friendId);
            }

        }

        

    }
}
