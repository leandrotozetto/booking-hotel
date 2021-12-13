using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository.
    /// </summary>
    /// <typeparam name="T">Entity's type.</typeparam>
    /// <seealso cref="IRepository{T}" />
    public class Repository<T> : IRepository<T>
         where T : class
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly DbContext _context;

        /// <summary>
        /// Can be used to linq query.
        /// </summary>
        private readonly DbSet<T> _dbSet;

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(BookingDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns true if successfully inserted.</returns>
        public async Task<bool> InsertAsync(T entity)
        {
            if (entity != null)
            {
                await _context.AddAsync(entity);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Returns the entity</returns>
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AsNoTracking()
                .AsQueryable()
                .FirstOrDefaultAsync(filter);
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns true if successfully deleted.</returns>
        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity != null)
            {
                _context.Remove(entity);

                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns true if successfully updated.</returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity != null)
            {
                _context.Update(entity);

                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
