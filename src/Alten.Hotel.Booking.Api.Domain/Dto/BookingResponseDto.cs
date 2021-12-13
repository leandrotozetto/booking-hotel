using System;

namespace Alten.Hotel.Booking.Api.Domain.Dto
{
    /// <summary>
    /// /
    /// </summary>
    /// <seealso cref="BookingDto" />
    public class BookingResponseDto : BookingDto
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static new BookingResponseDto Empty { get; } = new BookingResponseDto();

        /// <summary>
        /// News the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="checkIn">The check in.</param>
        /// <param name="checkOut">The check out.</param>
        /// <returns></returns>
        public static BookingResponseDto New(Guid id, DateTime checkIn, DateTime checkOut)
        {
            return new BookingResponseDto
            {
                Id = id,
                CheckIn = checkIn,
                CheckOut = checkOut
            };
        }
    }
}
