using Alten.Hotel.Booking.Api.Domain.Interfaces.Aggregates;
using System;
using System.Collections.ObjectModel;

namespace Alten.Hotel.Booking.Api.Domain.Aggregates.BookingAggregate
{
    /// <summary>
    /// Booking Aggregate
    /// </summary>
    public class Booking : IBooking
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the check-in date.
        /// </summary>
        /// <value>
        /// The check-in date.
        /// </value>
        public DateTime CheckIn { get; private set; }

        /// <summary>
        /// Gets the check-out date.
        /// </summary>
        /// <value>
        /// The check-out date.
        /// </value>
        public DateTime CheckOut { get; private set; }

        /// <summary>
        /// Error list
        /// </summary>
        public Collection<string> Errors { get; private set; }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static IBooking Empty { get; } = new Booking();

        /// <summary>
        /// Max days in advance to book room.
        /// </summary>
        public static readonly int MAX_DAYS_IN_ADVANCE_TO_BOOK = 30;

        /// <summary>
        /// Min days to book room.
        /// </summary>
        public static readonly int MIN_DAYS_BEFORE_BOOKING = 1;

        /// <summary>
        /// Max days to stay.
        /// </summary>
        public static readonly int MAX_DAYS_TO_STAY = 3;

        /// <summary>
        /// Gets the maximum checkout days.
        /// </summary>
        /// <value>
        /// The maximum checkout days.
        /// </value>
        public static readonly int MAX_CHECK_OUT_DAYS = MAX_DAYS_IN_ADVANCE_TO_BOOK + MAX_DAYS_TO_STAY - 1;

        /// <summary>
        /// Gets the maximum checkout days.
        /// </summary>
        /// <value>
        /// The maximum checkout days.
        /// </value>
        public static readonly int MAX_CHECK_IN_DAYS = MAX_DAYS_IN_ADVANCE_TO_BOOK + MAX_DAYS_TO_STAY - 2;

        /// <summary>
        /// The next available check in date
        /// </summary>
        public static DateTime NextAvailableCheckin
        {
            get { return DateTime.Now.AddDays(MIN_DAYS_BEFORE_BOOKING).Date; }
        }

        /// <summary>
        /// Gets the last avalible check in date.
        /// </summary>
        /// <value>
        /// The last avalible check in date.
        /// </value>
        public static DateTime LastAvailableCheckIn
        {
            get { return DateTime.Now.AddDays(MAX_CHECK_IN_DAYS).Date; }
        }

        /// <summary>
        /// Gets the next avalible check out.
        /// </summary>
        /// <value>
        /// The next avalible check out.
        /// </value>
        public static DateTime NextAvailableCheckOut
        {
            get { return DateTime.Now.AddDays(MIN_DAYS_BEFORE_BOOKING).Date; }
        }

        /// <summary>
        /// Gets the last avalible check out.
        /// </summary>
        /// <value>
        /// The last avalible check out.
        /// </value>
        public static DateTime LastAvailableCheckOut
        {
            get { return DateTime.Now.AddDays(MAX_DAYS_IN_ADVANCE_TO_BOOK).Date; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Booking"/> class.
        /// </summary>
        private Booking()
        {
            Errors = new Collection<string>();
        }

        /// <summary>
        /// Creates a new Booking
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>IBooking</returns>
        public static IBooking New(DateTime startDate, DateTime endDate)
        {
            var booking = new Booking
            {
                CheckOut = endDate,
                CheckIn = startDate,
                Id = Guid.NewGuid()
            };

            return booking;
        }

        /// <summary>
        /// Changes the date.
        /// </summary>
        /// <param name="checkIn">The start date.</param>
        /// <param name="checkOut">The end date.</param>
        public IBooking ChangeDate(DateTime checkIn, DateTime checkOut)
        {
            CheckIn = checkIn.Date;
            CheckOut = checkOut.Date;

            return this;
        }

        /// <summary>
        /// Validates 
        /// </summary>
        /// <returns>Returns true if it's valid</returns>
        public bool IsValid()
        {
            ValidateDate(nameof(CheckIn), CheckIn);
            ValidateDate(nameof(CheckOut), CheckOut);

            CheckIfDateIsGreaterThan(nameof(CheckIn), CheckIn, CheckOut);

            CheckIfDateIsLessThan(nameof(CheckIn), CheckIn.Date, NextAvailableCheckin);
            CheckIfDateIsGreaterThan(nameof(CheckIn), CheckIn.Date, LastAvailableCheckIn);

            CheckIfDateIsLessThan(nameof(CheckOut), CheckOut.Date, NextAvailableCheckOut);
            CheckIfDateIsGreaterThan(nameof(CheckOut), CheckOut.Date, LastAvailableCheckOut);

            if (CalculateDaysBetweenDates() > MAX_DAYS_TO_STAY)
            {
                Errors.Add($"The room can’t be booked for more than 3 days.");
            }

            if (IsValidSartDate())
            {
                Errors.Add($"The room can’t be booked for more than 30 days in advance.");
            }

            return Errors.Count == 0;
        }

        private void CheckIfDateIsGreaterThan(string fieldName, DateTime startDate, DateTime endDate)
        {
            if (DateTime.Compare(startDate, endDate) > 0)
            {
                Errors.Add($"The {fieldName} can't be later than {endDate:yyyy-MM-dd}.");
            }
        }

        private void CheckIfDateIsLessThan(string fieldName, DateTime startDate, DateTime endDate)
        {
            if (DateTime.Compare(startDate, endDate) < 0)
            {
                Errors.Add($"The {fieldName} can't be earlier than {endDate:yyyy-MM-dd}.");
            }
        }

        /// <summary>
        /// Calculates the days between dates.
        /// </summary>
        /// <returns>Days between dates</returns>
        private int CalculateDaysBetweenDates()
        {
            if (CheckOut < CheckIn)
            {
                return 0;
            }

            return (CheckOut - CheckIn).Days;
        }

        /// <summary>
        /// Checks if start date is valid
        /// </summary>
        /// <returns></returns>
        private bool IsValidSartDate()
        {
            var limitDate = DateTime.Now.AddDays(MAX_DAYS_IN_ADVANCE_TO_BOOK);

            return CheckIn > limitDate;
        }

        /// <summary>
        /// Validates the date.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <param name="date">The date.</param>
        private void ValidateDate(string field, DateTime date)
        {
            if (date.Date == DateTime.MinValue.Date)
            {
                Errors.Add($"The {field} is invalid.");
            }
        }
    }
}
