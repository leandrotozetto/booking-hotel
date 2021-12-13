using Alten.Hotel.Booking.Api.Domain.Aggregates.HotelAggregate;
using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Alten.Hotel.Booking.Api.Application.Test.Applications
{
    public class CalendarApplicationTest
    {
        private readonly CalendarApplication _calendarApplication;

        private static Collection<string> _errors;

        private static bool _IsCriticalError;

        private static bool _throwException;

        private static bool _isEmpty;

        public CalendarApplicationTest()
        {
            _errors = new Collection<string>();
            _isEmpty = false;
            _IsCriticalError = false;
            _throwException = false;

            _calendarApplication = new CalendarApplication(GetCalendarService(), GetMapper(), GetNotification(), GetLogger());
        }

        [Fact]
        public async Task Should_Get_Available_Dates()
        {
            var entities = await _calendarApplication.GetAvailableDatesAsync();

            Assert.NotEmpty(entities);
        }

        [Fact]
        public async Task Should_Get_Empty_List_When_There_Are_Not_Available_Dates()
        {
            _isEmpty = true;
            var entities = await _calendarApplication.GetAvailableDatesAsync();

            Assert.Empty(entities);
        }

        [Fact]
        public async Task Should_Get_Empty_List_When_Exception_Is_Threw()
        {
            _throwException = true;
            var entities = await _calendarApplication.GetAvailableDatesAsync();

            Assert.Empty(entities);
            Assert.True(_IsCriticalError);
            Assert.Contains(_errors, x => x.Equals("The operation was aborted."));
        }

        private static ILogger<CalendarApplication> GetLogger()
        {
            var loggerMock = new Mock<ILogger<CalendarApplication>>();

            return loggerMock.Object;
        }

        private static IMapper GetMapper()
        {
            var mapperMock = new Mock<IMapper>();

            mapperMock.Setup(x => x.Map<IEnumerable<AvailableDateDto>>(It.IsAny<IEnumerable<AvailableDate>>()))
                .Returns((IEnumerable<AvailableDate> availableDates) =>
                {
                    if (availableDates.Any())
                    {
                        var list = new Collection<AvailableDateDto>();

                        foreach (var item in availableDates)
                        {
                            list.Add(AvailableDateDto.New(item.Date, item.IsAvailable));
                        }

                        return list;
                    }

                    return Enumerable.Empty<AvailableDateDto>();
                });

            return mapperMock.Object;
        }

        private static ICalendarService GetCalendarService()
        {
            var service = new Mock<ICalendarService>();

            service.Setup(x => x.GetAvailableDatesAsync(It.IsAny<Guid?>())).
                ReturnsAsync((Guid? bookingId) =>
                {
                    if (_throwException)
                    {
                        throw new Exception();
                    }

                    if (_isEmpty)
                    {
                        return Enumerable.Empty<AvailableDate>();
                    }

                    return new Collection<AvailableDate>
                    {
                        AvailableDate.New(DateTime.Now, Guid.NewGuid()),
                        AvailableDate.New(DateTime.Now, Guid.Empty)
                    };
                });

            return service.Object;
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
    }
}
