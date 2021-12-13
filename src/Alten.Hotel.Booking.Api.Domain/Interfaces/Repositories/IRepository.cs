using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface that defines the repository.
    /// </summary>
    /// <typeparam name="T">Type of entity.</typeparam>
    public interface IRepository<T> : IDisposable
        where T : class
    {
        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns true if successfully inserted.</returns>
        Task<bool> InsertAsync(T entity);

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Returns the entity</returns>
        Task<T> GetAsync(Expression<Func<T, bool>> filter);

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns true if successfully deleted.</returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns true if successfully updated.</returns>
        Task<bool> UpdateAsync(T entity);
    }
}
