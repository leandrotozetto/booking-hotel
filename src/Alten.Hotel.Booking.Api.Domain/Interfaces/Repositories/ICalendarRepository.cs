using Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Hotel repository
    /// </summary>
    public interface ICalendarRepository : IDisposable
    {
        /// <summary>
        /// Gets the available dates asynchronous.
        /// </summary>
        /// <param name="StartDate">The start date.</param>
        /// <param name="EndDate">The end date.</param>
        /// <param name="currentBookingId">Current BookingId.</param>
        /// <returns>Return list of available dates</returns>
        Task<IEnumerable<AvailableDate>> GetAvailableDatesAsync(DateTime StartDate, DateTime EndDate, Guid? currentBookingId);
    }
}
