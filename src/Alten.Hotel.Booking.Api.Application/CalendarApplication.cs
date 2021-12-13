using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Applications;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Application
{
    public class CalendarApplication : ICalendarApplication
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The calendar repository
        /// </summary>
        private readonly ICalendarService _calendarService;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<CalendarApplication> _logger;

        /// <summary>
        /// The notification
        /// </summary>
        private readonly INotification _notification;

        public CalendarApplication(ICalendarService calendarService, IMapper mapper,
            INotification notification, ILogger<CalendarApplication> logger)
        {
            _calendarService = calendarService;
            _mapper = mapper;
            _notification = notification;
            _logger = logger;
        }

        /// <summary>
        /// Get available dates of period.
        /// </summary>
        /// <returns>Returns list of dates.</returns>
        public async Task<IEnumerable<AvailableDateDto>> GetAvailableDatesAsync()
        {
            try
            {
                var entities = await _calendarService.GetAvailableDatesAsync();
                var dtos = _mapper.Map<IEnumerable<AvailableDateDto>>(entities);

                return dtos;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, "[GetAvailableDatesAsync] - Operation Aborted");

                _notification.AddCritical("The operation was aborted.");

                return Enumerable.Empty<AvailableDateDto>();
            }
        }
    }
}
