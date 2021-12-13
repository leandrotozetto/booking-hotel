using Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Aggregates;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Domain.Services
{
    /// <summary>
    /// Booking service.
    /// </summary>
    public class BookingService : IBookingService
    {
        /// <summary>
        /// Booking repository.
        /// </summary>
        private readonly IBookingRepository _bookingRepository;

        /// <summary>
        /// The calendar service
        /// </summary>
        private readonly ICalendarService _calendarService;

        /// <summary>
        /// Notification
        /// </summary>
        private readonly INotification _notification;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<BookingService> _logger;

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Creates a new BookingService.
        /// </summary>
        /// <param name="calendarService">The calendar repository.</param>
        /// <param name="bookingRepository">The booking repository.</param>
        /// <param name="notification">the notification.</param>
        /// <param name="logger">The log.</param>
        public BookingService(ICalendarService calendarService, IBookingRepository bookingRepository,
            INotification notification, ILogger<BookingService> logger)
        {
            _calendarService = calendarService;
            _bookingRepository = bookingRepository;
            _notification = notification;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns booking's confirmation id.</returns>
        public async Task<Guid> CreateReservationAsync(IBooking booking)
        {
            try
            {
                if (booking is not null && !Aggregates.BookingAggregate.Booking.Empty.Equals(booking))
                {
                    var isValid = IsValid(booking);
                    var isAvailableDates = await CheckAvailabilityAsync(booking.CheckIn, booking.CheckOut);

                    if (isAvailableDates && isValid)
                    {
                        var inserted = await InsertAsync(booking);

                        if (inserted)
                        {
                            return booking.Id;
                        }
                        else
                        {
                            _notification.AddNotification($"The resevation couldn't be inserted.");
                        }
                    }
                }
                else
                {
                    _notification.AddNotification("The object booking is invalid");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, "[CreateReservationAsync] - Operation Aborted");

                _notification.AddCritical("The operation was aborted.");
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Updates a new booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true if it's was successfully updated.</returns>
        public async Task UpdateReservationAsync(IBooking booking)
        {
            try
            {
                var updated = false;

                if (booking is not null && !Aggregates.BookingAggregate.Booking.Empty.Equals(booking))
                {
                    var isValid = IsValid(booking);
                    var isAvailableDates = await CheckAvailabilityAsync(booking.CheckIn, booking.CheckOut, booking.Id);

                    if (isAvailableDates && isValid)
                    {
                        updated = await UpdateAsync(booking);
                    }

                    if (!updated)
                    {
                        _notification.AddNotification($"The resevation couldn't be updated.");
                    }
                }
                else
                {
                    _notification.AddNotification("The object booking is invalid");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, "[UpdateReservationAsync] - Operation Aborted");

                _notification.AddCritical("The operation was aborted.");
            }
        }

        /// <summary>
        /// Inserts new booking on database
        /// </summary>
        /// <param name="booking"></param>
        /// <returns>Returns true if it was successfully inserted</returns>
        private async Task<bool> InsertAsync(IBooking booking)
        {
            try
            {
                return await _bookingRepository.InsertAsync(booking as Aggregates.BookingAggregate.Booking);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, "[InsertAsync] - Operation Aborted");

                _notification.AddCritical("The operation was aborted.");

                return false;
            }
        }

        /// <summary>
        /// Updates new booking on database
        /// </summary>
        /// <param name="booking"></param>
        /// <returns>Returns true if it was successfully updated</returns>
        private async Task<bool> UpdateAsync(IBooking booking)
        {
            try
            {
                return await _bookingRepository.UpdateAsync(booking as Aggregates.BookingAggregate.Booking);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, "[UpdateAsync] - Operation Aborted");

                _notification.AddCritical("The operation was aborted.");

                return false;
            }
        }

        /// <summary>
        /// Checks is the booking is valid.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true is it's valid.</returns>
        private bool IsValid(IBooking booking)
        {
            var isValid = booking.IsValid();

            if (!isValid)
            {
                _notification.AddNotification(booking.Errors);
            }

            return isValid;
        }

        /// <summary>
        /// Checks if the dates are available.
        /// </summary>
        /// <param name="checkIn"></param>
        /// <param name="checkout"></param>
        /// <param name="excludeBookingId">Current BookingId.</param>
        /// <returns>Returns true is the date is available.</returns>
        private async Task<bool> CheckAvailabilityAsync(DateTime checkIn, DateTime checkout, Guid? excludeBookingId = null)
        {
            var availableDatesInPeriod = await _calendarService.GetAvailableDatesAsync(excludeBookingId);

            if (availableDatesInPeriod.Any())
            {
                var selectedDatesOfStay = GetDatesBetweenCheckInAndCheckOut(checkIn, checkout).ToList();
                var lastAvailableDate = availableDatesInPeriod.Last().Date;
                var firstAvailableDate = availableDatesInPeriod.First().Date;
                var isBookingDatesValid = IsBookingDatesValid(checkIn, checkout, firstAvailableDate, lastAvailableDate);
                var isSelectedDatesAvailable = IsSelectedDateAvailable(availableDatesInPeriod, selectedDatesOfStay);

                return isBookingDatesValid && isSelectedDatesAvailable;
            }
            else
            {
                _notification.AddNotification("It was not possible to get the dates.");

                return false;
            }
        }

        /// <summary>
        /// Check if the selected dates are availables.
        /// </summary>
        /// <param name="availableDatesInPeriod">Available dates</param>
        /// <param name="selectedDatesOfStay">Selected dates</param>
        /// <returns>Returns true if it's valid</returns>
        private bool IsSelectedDateAvailable(IEnumerable<AvailableDate> availableDatesInPeriod, IEnumerable<DateTime> selectedDatesOfStay)
        {
            var allowedDates = availableDatesInPeriod
                .Where(x => (selectedDatesOfStay.Contains(x.Date) && x.IsAvailable))
                .Select(x => x.Date);

            if (allowedDates.Count() == selectedDatesOfStay.Count())
            {
                return true;
            }
            else
            {
                var unavailableDates = selectedDatesOfStay.Where(x => !allowedDates.Contains(x.Date));

                foreach (var item in unavailableDates)
                {
                    _notification.AddNotification($"The date {item.Date:yyyy-MM-dd} is unavailable.");
                }

                return false;
            }
        }

        /// <summary>
        /// Checks if the booking's dates is invalid.
        /// </summary>
        /// <param name="checkIn">Check-in's date.</param>
        /// <param name="checkout">Check-out's date.</param>
        /// <param name="fisrtAvailableDate">First date available.</param>
        /// <param name="lastAvailableDate">Last date available.</param>
        /// <returns>Retuns true if booking's dates is valid</returns>
        private bool IsBookingDatesValid(DateTime checkIn, DateTime checkout, DateTime fisrtAvailableDate, DateTime lastAvailableDate)
        {
            if (checkout.Date > lastAvailableDate.Date.Date
                || checkIn.Date > lastAvailableDate.Date.AddDays((Aggregates.BookingAggregate.Booking.MAX_DAYS_TO_STAY - 1) * -1))
            {
                _notification.AddNotification($"The dates of stay need to be between {fisrtAvailableDate.Date:yyyy-MM-dd} and {lastAvailableDate.Date:yyyy-MM-dd}.");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Get dates between CheckIn and CheckOut.
        /// </summary>
        /// <param name="checkIn">CheckIn's date</param>
        /// <param name="checkout">CheckOut's date</param>
        /// <returns>Returns list of dates.</returns>
        private static IEnumerable<DateTime> GetDatesBetweenCheckInAndCheckOut(DateTime checkIn, DateTime checkout)
        {
            var currentDate = checkIn;

            while (checkout >= currentDate)
            {
                yield return currentDate;

                currentDate = currentDate.AddDays(1);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _bookingRepository.Dispose();
                    _calendarService.Dispose();
                    _notification.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
