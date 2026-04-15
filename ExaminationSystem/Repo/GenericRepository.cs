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

        public GenericRepository()
        {
            _Context = new Context();
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

        //----------------------------------------------
        public void UpdateInclude(T model, params string[] modifiedProperties)
        {
            if (!_dbSet.Any(x => x.ID == model.ID && !x.Deleted))
                return ;

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

            _Context.SaveChanges();

        }
        public async Task<bool> DeleteAsync(int id)
        {
            var updatedRows = await _dbSet
                .Where(x => x.ID == id && x.Deleted == false)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.Deleted, true));

            return updatedRows > 0;
        }

    }
}
