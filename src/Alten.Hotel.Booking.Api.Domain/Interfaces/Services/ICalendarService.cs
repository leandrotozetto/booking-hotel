using Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Domain.Interfaces.Services
{
    /// <summary>
    /// alendar service's contract
    /// </summary>
    public interface ICalendarService : IDisposable
    {
        /// <summary>
        /// Get available dates of period.
        /// </summary>
        /// <returns>Returns list of dates.</returns>
        Task<IEnumerable<AvailableDate>> GetAvailableDatesAsync(Guid? currentBookingId = null);
    }
}
