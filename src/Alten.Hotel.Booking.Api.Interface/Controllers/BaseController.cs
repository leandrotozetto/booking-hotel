using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alten.Hotel.Booking.Api.Interface.Controllers
{
    /// <summary>
    /// Base controller
    /// </summary>
    /// <seealso cref="ControllerBase" />
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// The notification
        /// </summary>
        protected readonly INotification _notification;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public BaseController(INotification notification)
        {
            _notification = notification;
        }

        /// <summary>
        /// Creates the response.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Returns IActionResult.</returns>
        protected virtual IActionResult CreateResponse(object value)
        {
            if (_notification.HasErrors)
            {
                return CreateErrorResponse();
            }

            return Ok(value);
        }

        /// <summary>
        /// Creates the response.
        /// </summary>
        /// <returns>Returns IActionResult.</returns>
        protected IActionResult CreateResponse()
        {
            if (_notification.HasErrors)
            {
                return CreateErrorResponse();
            }

            return NoContent();
        }

        /// <summary>
        /// Creates the error response.
        /// </summary>
        /// <returns></returns>
        protected IActionResult CreateErrorResponse()
        {
            if (_notification.HasCriticalError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDto.New(_notification.Errors));
            }

            return BadRequest(ErrorDto.New(_notification.Errors));
        }
    }
}
