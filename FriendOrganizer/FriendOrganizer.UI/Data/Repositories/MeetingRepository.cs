
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDbContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context) : base(context)
        {
        }

        public override async Task<Meeting> GetByIdAsync(int Id)
        {
            return await Context.Meetings.Include(c => c.Friends).FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<List<Friend>> GetAllFriendsAsync()
        {
            return await Context.Set<Friend>().ToListAsync();
        }

        public async Task ReloadFriendAsync(int id)
        {
            var dbEntityEntry = Context.ChangeTracker.Entries<Friend>()
                .SingleOrDefault(d=>d.Entity.Id==id);

            if(dbEntityEntry!=null)
            {
              await  dbEntityEntry.ReloadAsync();
            }

        }
    }
}
