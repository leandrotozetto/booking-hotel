using Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
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
    /// Calendar service
    /// </summary>
    /// <seealso cref="ICalendarService" />
    public class CalendarService : ICalendarService
    {
        /// <summary>
        /// Calendar repository.
        /// </summary>
        private readonly ICalendarRepository _calendarRepository;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CalendarService> _logger;

        /// <summary>
        /// The notification
        /// </summary>
        private readonly INotification _notification;

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Creates a new BookingService.
        /// </summary>
        /// <param name="calendarRepository">The calendar repository.</param>
        /// <param name="notification">The notification.</param>
        /// <param name="logger">The log.</param>
        public CalendarService(ICalendarRepository calendarRepository, INotification notification, 
            ILogger<CalendarService> logger)
        {
            _calendarRepository = calendarRepository;
            _notification = notification;
            _logger = logger;
        }

        /// <summary>
        /// Get available dates of period.
        /// </summary>
        /// <returns>Returns list of dates.</returns>
        public async Task<IEnumerable<AvailableDate>> GetAvailableDatesAsync(Guid? excludeBookingId = null)
        {
            try
            {
                var startDate = DateTime.Now.AddDays(Aggregates.BookingAggregate.Booking.MIN_DAYS_BEFORE_BOOKING);
                var endDate = DateTime.Now.AddDays(Aggregates.BookingAggregate.Booking.MAX_DAYS_IN_ADVANCE_TO_BOOK);

                return await _calendarRepository.GetAvailableDatesAsync(startDate, endDate, excludeBookingId);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, "[GetAvailableDatesAsync] - Operation Aborted");

                _notification.AddCritical("The operation was aborted.");

                return Enumerable.Empty<AvailableDate>();
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
                    _notification.Dispose();
                    _calendarRepository.Dispose();
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
