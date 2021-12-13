using Alten.Hotel.Booking.Api.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Domain.Interfaces.Applications
{
    /// <summary>
    /// Calendar application
    /// </summary>
    public interface ICalendarApplication
    {
        /// <summary>
        /// Get available dates of period.
        /// </summary>
        /// <returns>Returns list of dates.</returns>
        Task<IEnumerable<AvailableDateDto>> GetAvailableDatesAsync();
    }
}
