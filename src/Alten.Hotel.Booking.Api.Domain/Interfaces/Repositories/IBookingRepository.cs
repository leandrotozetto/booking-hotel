using System;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Booking's repository contract.
    /// </summary>
    public interface IBookingRepository : IDisposable
    {
        /// <summary>
        /// Inserts the specified booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true if successfully inserted.</returns>
        Task<bool> InsertAsync(Domain.Aggregates.BookingAggregate.Booking booking);

        /// <summary>
        /// Gets the specified booking.
        /// </summary>
        /// <param name="bookingId">The identifier.</param>
        /// <returns></returns>
        Task<Domain.Aggregates.BookingAggregate.Booking> GetAsync(Guid bookingId);

        /// <summary>
        /// Deletes the booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true if successfully deleted.</returns>
        Task<bool> DeleteAsync(Domain.Aggregates.BookingAggregate.Booking booking);

        /// <summary>
        /// Updates the booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true if successfully updated.</returns>
        Task<bool> UpdateAsync(Domain.Aggregates.BookingAggregate.Booking booking);
    }
}
