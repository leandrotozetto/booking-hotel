using System;
using System.Diagnostics.CodeAnalysis;

namespace Alten.Hotel.Booking.Api.Domain.Dto
{
    /// <summary>
    /// Date Available dto
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AvailableDateDto
    {
        /// <summary>
        /// Gets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance is available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// News the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="isAvailable">if set to <c>true</c> [is available].</param>
        /// <returns></returns>
        public static AvailableDateDto New(DateTime date, bool isAvailable)
        {
            return new AvailableDateDto
            {
                Date = date,
                IsAvailable = isAvailable
            };
        }
    }
}
