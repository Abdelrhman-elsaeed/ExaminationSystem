using ExaminationSystem.DataBase;
using ExaminationSystem.Models;
using ExaminationSystem.ModelVm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace ExaminationSystem.Repo
{
    public class GenericRepository<T> where T : BaseModel
    {

        protected readonly Context _Context;
        protected DbSet<T> _dbSet;

        public GenericRepository(Context Context)
        {
            _Context = Context;
            _dbSet = _Context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {

            return  _dbSet.Where(x =>!x.Deleted);
        }
        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return GetAll().Where(expression);

        }
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.ID == id && x.Deleted == false);
        }
        public async Task<T?> GetByIdWithTrackingAsync(int id)
        {
            return await _dbSet.AsTracking()
                .FirstOrDefaultAsync(x => x.ID == id && x.Deleted == false);
        }
        public async Task<bool> AddAsync(T model)
        {
            _dbSet.Add(model);
            var savedRows = await _Context.SaveChangesAsync();

            return savedRows > 0;
        }
        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        //----------------------------------------------
        public async Task<bool> UpdateInclude(T model, params string[] modifiedProperties)
        {
            if (!await _dbSet.AnyAsync(x => x.ID == model.ID && !x.Deleted))
                return false ;

            var local = _dbSet.Local.FirstOrDefault(x => x.ID == model.ID);
            EntityEntry entityEntry;

            if (local is null)
                entityEntry = _Context.Entry(model);
            else 
                entityEntry = _Context.ChangeTracker.Entries<T>().FirstOrDefault(x => x.Entity.ID == model.ID);

            foreach (var prop in entityEntry.Properties)
            {
                if (modifiedProperties.Contains(prop.Metadata.Name))
                {
                    prop.CurrentValue = model.GetType().GetProperty(prop.Metadata.Name).GetValue(model);
                    prop.IsModified = true;
                }
            }

            var savedRows = await _Context.SaveChangesAsync();
            return savedRows > 0;

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var updatedRows = await _dbSet
                .Where(x => x.ID == id && x.Deleted == false)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.Deleted, true));

            return updatedRows > 0;
        }
        public async Task<int> DeleteRangeAsync(Expression<Func<T,bool>> predicate)
        {
            var updatedRows = await _dbSet.Where(predicate).Where(x => x.Deleted == false)
                .ExecuteUpdateAsync(x => x.SetProperty(prop => prop.Deleted, true));

            return updatedRows;
        }

    }
}
