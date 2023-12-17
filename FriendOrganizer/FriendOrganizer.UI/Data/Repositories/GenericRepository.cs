using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class GenericRepository<TEntity,TContext> : IGenericRepository<TEntity> where TContext:DbContext where TEntity : class
    {
        protected GenericRepository(TContext context) 
        {
            Context = context;
        }

        protected readonly TContext Context;

        public void Add(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public virtual async Task<TEntity> GetByIdAsync(int Id)
        {
            return await Context.Set<TEntity>().FindAsync(Id); 
        }

        public bool HasCHanges()
        {            
            return Context.ChangeTracker.HasChanges();            
        }

        public void Remove(TEntity model)
        {
            Context.Set<TEntity>().Remove(model);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }
    }
}
