using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Applications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Interface.Controllers
{
    /// <summary>
    /// /Booking's controller
    /// </summary>
    /// <seealso cref="ControllerBase" />
    [ApiController]
    [Route("bookings")]
    public class BookingController : BaseController
    {
        /// <summary>
        /// The booking application
        /// </summary>
        private readonly IBookingApplication _bookingApplication;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="bookingApplication">The booking application.</param>
        /// <param name="notification">Teh notification.</param>
        public BookingController(IBookingApplication bookingApplication,
            INotification notification) : base(notification)
        {
            _bookingApplication = bookingApplication;
        }

        /// <summary>
        /// Get the Booking.
        /// </summary>
        /// <param name="bookingId">The booking identifier.</param>
        /// <returns>A booking</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /0000000-0000-0000-0000-000000000000
        ///
        /// </remarks>
        /// <response code="200">If the booking is found.</response>
        /// <response code="204">If the booking isn't found.</response>
        /// <response code="500">When something goes wrong.</response>
        [HttpGet("{bookingId}")]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(Guid bookingId)
        {
            var booking = await _bookingApplication.GetAsync(bookingId);

            return CreateResponse(booking);
        }

        /// <summary>
        /// Creates the booking.
        /// </summary>
        /// <param name="bookingDto">The booking.</param>
        /// <returns>Return booking's confirmation code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Booking
        ///     {
        ///        "startDate": "2021-12-09",
        ///        "endDate": "2021-12-09"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">If the Booking is created.</response>
        /// <response code="400">If the booking is invalid.</response>
        /// <response code="500">When something goes wrong.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync(BookingDto bookingDto)
        {
            var bookingId = await _bookingApplication.CreateAsync(bookingDto);

            return CreateResponse(new { BookingId = bookingId });
        }

        /// <summary>
        /// Update the booking.
        /// </summary>
        /// <param name="bookingId">The booking identifier.</param>
        /// <param name="bookingDto">The booking.</param>
        /// <returns>Return booking's confirmation code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Booking/00000000-0000-0000-0000-000000000000
        ///     {
        ///        "startDate": "2021-12-09",
        ///        "endDate": "2021-12-09"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the booking is successfully updated.</response>
        /// <response code="400">If the booking is invalid.</response>
        /// <response code="500">When something goes wrong.</response>
        [HttpPut("{bookingId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid bookingId, BookingDto bookingDto)
        {
            await _bookingApplication.UpdateAsync(bookingId, bookingDto);

            return CreateResponse();
        }

        /// <summary>
        /// Detele the Booking.
        /// </summary>
        /// <param name="bookingId">The booking identifier.</param>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /0000000-0000-0000-0000-000000000000
        ///
        /// </remarks>
        /// <response code="204">If the booking is found.</response>
        /// <response code="400">If the booking isn't found.</response>
        /// <response code="500">When something goes wrong.</response>
        [HttpDelete("{bookingId}")]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAsync(Guid bookingId)
        {
            await _bookingApplication.DeleteAsync(bookingId);

            return CreateResponse();
        }

        /// <summary>
        /// Creates the response.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns IActionResult.</returns>
        protected override IActionResult CreateResponse(object value)
        {
            if (value is BookingDto dto && BookingDto.Empty.Equals(dto))
            {
                return CreateResponse();
            }

            return base.CreateResponse(value);
        }
    }
}
