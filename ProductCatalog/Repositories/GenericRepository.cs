using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.DTOs;
using ProductCatalog.Models;
using ProductCatalog.Repositories.Interfaces;

namespace ProductCatalog.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly CatalogDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(CatalogDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<ApiResponse<IEnumerable<T>>> GetAllAsync()
        {
            var entities = await _dbSet
                .Where(e => !e.IsDeleted)
                .AsNoTracking()
                .ToListAsync();

            return entities.Any()
                ? ApiResponse<IEnumerable<T>>.Ok(entities)
                : ApiResponse<IEnumerable<T>>.Fail("No records found");
        }

        public async Task<ApiResponse<T>> GetByIdAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null || entity.IsDeleted)
                return ApiResponse<T>.Fail("Entity not found or deleted");

            return ApiResponse<T>.Ok(entity);
        }

        public async Task<ApiResponse<bool>> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Entity added successfully");
        }

        public async Task<ApiResponse<bool>> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Entity updated successfully");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return ApiResponse<bool>.Fail("Entity not found");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Ok(true, "Entity deleted successfully");
        }

        public async Task<ApiResponse<bool>> SoftDeleteAsync(Guid id, string deletedBy)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                return ApiResponse<bool>.Fail("Entity not found");

            entity.IsDeleted = true;
            entity.DeletedUtc = DateTime.UtcNow;
            entity.DeletedBy = deletedBy;

            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "Entity soft deleted successfully");
        }
    }
}
