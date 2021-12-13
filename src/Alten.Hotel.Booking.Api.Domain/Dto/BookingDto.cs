using System;
using System.Diagnostics.CodeAnalysis;

namespace Alten.Hotel.Booking.Api.Domain.Dto
{
    /// <summary>
    /// Booking Dto
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BookingDto
    {
        /// <summary>
        /// Gets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime CheckIn { get; set; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime CheckOut { get; set; }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static BookingDto Empty { get; } = new BookingDto();

        /// <summary>
        /// News the specified check in.
        /// </summary>
        /// <param name="checkIn">The check in.</param>
        /// <param name="checkOut">The check out.</param>
        /// <returns></returns>
        public static BookingDto New(DateTime checkIn, DateTime checkOut)
        {
            return new BookingDto
            {
                CheckIn = checkIn,
                CheckOut = checkOut
            };
        }
    }
}
