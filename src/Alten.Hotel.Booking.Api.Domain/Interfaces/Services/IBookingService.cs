using Alten.Hotel.Booking.Api.Domain.Interfaces.Aggregates;
using System;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Domain.Interfaces.Services
{
    /// <summary>
    /// BookingService's contract
    /// </summary>
    public interface IBookingService : IDisposable
    {
        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns booking's confirmation id.</returns>
        Task<Guid> CreateReservationAsync(IBooking booking);

        /// <summary>
        /// Updates a new booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true if it's was successfully updated.</returns>
        Task UpdateReservationAsync(IBooking booking);
    }
}
