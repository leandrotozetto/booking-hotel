using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Applications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Interface.Controllers
{
    /// <summary>
    /// /Booking's controller
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("calendars")]
    public class CalendarController : BaseController
    {

        /// <summary>
        /// The booking application
        /// </summary>
        private readonly ICalendarApplication _calendarApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="calendarApplication">The booking application.</param>
        /// <param name="notification">Teh notification.</param>
        public CalendarController(ICalendarApplication calendarApplication,
            INotification notification) : base(notification)
        {
            _calendarApplication = calendarApplication;
        }

        /// <summary>
        /// Get available dates.
        /// </summary>
        /// <returns>Available dates</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /calendar/available
        ///
        /// </remarks>
        /// <response code="200">If the dates are found</response>
        /// <response code="204">If the dates aren't found.</response>
        /// <response code="500">When something goes wrong</response>
        [HttpGet("dates/available-dates")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAvailableDatesAsync()
        {
            var booking = await _calendarApplication.GetAvailableDatesAsync();

            return CreateResponse(booking);
        }

        /// <summary>
        /// Creates the response.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns IActionResult.</returns>
        protected override IActionResult CreateResponse(object value)
        {
            if (value is IEnumerable<AvailableDateDto> enumerable && enumerable.Any() == false)
            {
                return CreateResponse();
            }

            return base.CreateResponse(value);
        }
    }
}
