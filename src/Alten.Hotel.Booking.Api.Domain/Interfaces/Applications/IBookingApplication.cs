using Alten.Hotel.Booking.Api.Domain.Dto;
using System;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Domain.Interfaces.Applications
{
    /// <summary>
    /// Booking's orchestrator.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IBookingApplication : IDisposable
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="bookingId">The booking identifier.</param>
        /// <returns>Returns the booking</returns>
        Task<BookingResponseDto> GetAsync(Guid bookingId);

        /// <summary>
        /// Creates the booking.
        /// </summary>
        /// <param name="bookingDto">The booking insert dto.</param>
        /// <returns>Returns true if successfully created.</returns>
        /// <returns></returns>
        Task<Guid> CreateAsync(BookingDto bookingDto);

        /// <summary>
        /// Updates the booking.
        /// </summary>
        /// <param name="bookingId">The booking identifier.</param>
        /// <param name="bookingDto">The booking.</param>
        Task UpdateAsync(Guid bookingId, BookingDto bookingDto);

        /// <summary>
        /// Updates the booking.
        /// </summary>
        /// <param name="bookingId">The booking identifier.</param>
        Task DeleteAsync(Guid bookingId);
    }
}
