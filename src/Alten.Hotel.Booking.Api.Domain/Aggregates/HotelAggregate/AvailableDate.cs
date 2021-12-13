using System;

namespace Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate
{
    /// <summary>
    /// Date Available
    /// </summary>
    public class AvailableDate
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
        public bool IsAvailable
        {
            get
            {
                return BookingId is null || Guid.Empty.Equals(BookingId);
            }
        }

        /// <summary>
        /// Gets the booking identifier.
        /// </summary>
        /// <value>
        /// The booking identifier.
        /// </value>
        public Guid? BookingId { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="AvailableDate"/> class from being created.
        /// </summary>
        private AvailableDate() { }

        /// <summary>
        /// News the specified date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="bookingId">The booking identifier.</param>
        /// <returns></returns>
        public static AvailableDate New(DateTime date, Guid? bookingId = null)
        {
            return new AvailableDate
            {
                Date = date,
                BookingId = bookingId
            };
        }
    }
}
