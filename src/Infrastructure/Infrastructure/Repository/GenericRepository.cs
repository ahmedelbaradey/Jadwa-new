using System.Linq.Expressions;
using Abstraction.Contract.Repository;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository;
using DocumentFormat.OpenXml.InkML;
using Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;


namespace Infrastructure.Repository
{

    public class GenericRepository : IGenericRepository
    {
        #region Fields  
        protected AppDbContext RepositoryContext;
        public readonly ICurrentUserService _currentUserService;

        #endregion

        #region Constructors  
        public GenericRepository(AppDbContext dbContext, ICurrentUserService currentUserService)
        { 
           RepositoryContext = dbContext;
           _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions  
        public IQueryable<T> GetAll<T>(bool trackChanges) where T : class
        {
            return !trackChanges ? RepositoryContext.Set<T>().AsNoTracking() : RepositoryContext.Set<T>();
        }
        public virtual IQueryable<T> GetByCondition<T>(Expression<Func<T, bool>> expression, bool trackChanges) where T : class
        {
            return !trackChanges ? RepositoryContext.Set<T>().Where(expression).AsNoTracking() : RepositoryContext.Set<T>().Where(expression);
        }
        public virtual async Task<T> GetByIdAsync<T>(int id, bool trackChanges) where T : class
        {
            try
            {
                var entity = await RepositoryContext.Set<T>().FindAsync(id).ConfigureAwait(false);
                if (entity == null)
                {
                    throw new InvalidOperationException($"Entity of type {typeof(T).Name} with ID {id} was not found.");
                }
                return entity;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public virtual async Task<T> GetByIdAsync<T>(int id,bool trackChanges,Func<IQueryable<T>, IQueryable<T>>? include = null) where T : class
        {
            try
            {
                IQueryable<T> query = RepositoryContext.Set<T>();

                if (!trackChanges)
                    query = query.AsNoTracking();

                if (include is not null)
                query = include(query);

                 return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
            
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return await RepositoryContext.Set<T>().AnyAsync(expression);

        }
        public virtual async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await RepositoryContext.Set<T>().AddAsync(entity, cancellationToken);
                int count = await RepositoryContext.SaveChangesAsync(_currentUserService.UserId.GetValueOrDefault());
                return entity;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public virtual async Task AddRangeAsync<T>(ICollection<T> entities, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                await RepositoryContext.Set<T>().AddRangeAsync(entities, cancellationToken);
                await RepositoryContext.SaveChangesAsync(_currentUserService.UserId.GetValueOrDefault());
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public virtual async Task<bool> DeleteAsync<T>(T entity) where T : class
        {
            try
            {
                RepositoryContext.Set<T>().Remove(entity);
                var rowEffectedCount = await RepositoryContext.SaveChangesAsync(_currentUserService.UserId.GetValueOrDefault());
                return rowEffectedCount > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public virtual async Task DeleteRangeAsync<T>(ICollection<T> entities) where T : class
        {
            try
            {
                RepositoryContext.Set<T>().RemoveRange(entities);
                await RepositoryContext.SaveChangesAsync(_currentUserService.UserId.GetValueOrDefault());
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public virtual async Task<bool> UpdateAsync<T>(T entity) where T : class
        {
            try
            {
                RepositoryContext.Set<T>().Update(entity);
                var rowUpdated = await RepositoryContext.SaveChangesAsync(_currentUserService.UserId.GetValueOrDefault());
                return rowUpdated > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public virtual async Task UpdateRangeAsync<T>(ICollection<T> entities) where T : class
        {
            try
            {
                RepositoryContext.Set<T>().UpdateRange(entities);
                await RepositoryContext.SaveChangesAsync(_currentUserService.UserId.GetValueOrDefault());
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            try
            {
                return await RepositoryContext.Database.BeginTransactionAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public async Task CommitAsync()
        {
            try
            {
                await RepositoryContext.Database.CommitTransactionAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        public async Task RolleBackAsync()
        {
            try
            {
                await RepositoryContext.Database.RollbackTransactionAsync();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error accessing the database.", ex);
            }
        }
        #endregion
    }
}
