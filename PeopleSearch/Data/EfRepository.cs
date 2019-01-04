using Microsoft.EntityFrameworkCore;
using PeopleSearch.Interfaces;
using PeopleSearch.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeopleSearch.Data
{
    public class EfRepository : IRepository
    {
        private readonly AppDbContext _context;

        public EfRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : BaseEntity
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAsync<T>() where T : BaseEntity
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync<T>(T entity) where T : BaseEntity
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync<T>(T entity) where T : BaseEntity
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EntityExistsAsync<T>(int id) where T : BaseEntity
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }
    }
}
