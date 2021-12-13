using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using Alten.Hotel.Booking.Api.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xunit;

namespace Alten.Hotel.Booking.Api.Domain.Test.Services
{
    public class CalendarServiceTest
    {
        private readonly CalendarService _calendarService;

        private static Collection<string> _errors;

        private static bool _IsCriticalError;

        private static bool _throwExceptionOnGetAvailableDates;

        public CalendarServiceTest()
        {
            _errors = new Collection<string>();
            _IsCriticalError = false;
            _throwExceptionOnGetAvailableDates = false;
            _calendarService = new CalendarService(GetCalendarRepository(),
                GetNotification(), GetLogger());
        }

        [Fact]
        public async Task Should_Get_Available_Dates()
        {
            var entities = await _calendarService.GetAvailableDatesAsync(Guid.NewGuid());

            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task Should_Get_Available_Dates_When_ExcludeBookingId_Was_Not_Informed()
        {
            var entities = await _calendarService.GetAvailableDatesAsync();

            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task Should_Get_Available_Dates_When_ExcludeBookingId_Is_Null()
        {
            var entities = await _calendarService.GetAvailableDatesAsync(null);

            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task Should_Not_Get_Available_Dates_When_Exception_Is_Threw()
        {
            _throwExceptionOnGetAvailableDates = true;

            var entities = await _calendarService.GetAvailableDatesAsync(null);

            Assert.Empty(entities);
            Assert.True(_IsCriticalError);
            Assert.Contains(_errors, x => x.Equals("The operation was aborted."));
        }

        private static ICalendarRepository GetCalendarRepository()
        {
            var serviceMock = new Mock<ICalendarRepository>();

            serviceMock.Setup(x => x.GetAvailableDatesAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid?>()))
                .ReturnsAsync((DateTime StartDate, DateTime EndDate, Guid? currentBookingId) =>
                {
                    if (_throwExceptionOnGetAvailableDates)
                    {
                        throw new Exception();
                    }

                    return new Collection<Aggregates.HotelAggregate.AvailableDate>
                    {
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(1).Date, null),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(2).Date, null),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(3).Date, Guid.NewGuid()),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(4).Date, Guid.NewGuid()),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(5).Date, Guid.NewGuid()),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(6).Date, Guid.NewGuid()),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(7).Date, null),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(8).Date, null),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(9).Date, Guid.NewGuid()),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(10).Date, null),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(11).Date, null),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(12).Date, null),
                        Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now.AddDays(13).Date, null)
                    };
                });

            return serviceMock.Object;
        }

        private static INotification GetNotification()
        {
            var notificationMock = new Mock<INotification>();

            notificationMock.Setup(x => x.AddCritical(It.IsAny<string>()))
                .Callback((string notification) =>
                {
                    _errors.Add(notification);
                    _IsCriticalError = true;
                });

            return notificationMock.Object;
        }

        private static ILogger<CalendarService> GetLogger()
        {
            var loggerMock = new Mock<ILogger<CalendarService>>();

            return loggerMock.Object;
        }
    }
}
