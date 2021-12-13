using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Applications;
using Alten.Hotel.Booking.Api.Interface.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Alten.Hotel.Booking.Api.Interface.Test
{
    public class CalendarControllerTest
    {
        private readonly CalendarController _calendarController;

        private static Collection<string> _errors;

        private static bool _IsCriticalError;

        private static bool _throwException;

        private static bool isEmpty;

        public CalendarControllerTest()
        {
            _errors = new Collection<string>();
            _IsCriticalError = false;
            _throwException = false;
            _calendarController = new CalendarController(GetCalendarApplication(), GetNotification());
        }

        [Fact]
        public async Task Should_Get_Reservation()
        {
            var actionResult = await _calendarController.GetAvailableDatesAsync();

            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            var bookingDto = okObjectResult.Value as BookingDto;
            Assert.NotEqual(BookingDto.Empty, bookingDto);
        }

        [Fact]
        public async Task Should_Not_Get_Reservation_When_Booking_Is_Invalid()
        {
            isEmpty = true;
            var actionResult = await _calendarController.GetAvailableDatesAsync();

            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);

            var noContentResult = actionResult as NoContentResult;

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task Should_Not_Get_Reservation_When_Exception_Is_Threw()
        {
            _throwException = true;
            var actionResult = await _calendarController.GetAvailableDatesAsync();

            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = actionResult as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var errorDto = objectResult.Value as ErrorDto;
            Assert.NotEmpty(errorDto.Errors);
        }

        private static ICalendarApplication GetCalendarApplication()
        {
            var repositoryMock = new Mock<ICalendarApplication>();

            repositoryMock.Setup(x => x.GetAvailableDatesAsync())
                .ReturnsAsync(() =>
                {
                    if (_throwException)
                    {
                        _errors.Add("The operation was aborted.");
                        _IsCriticalError = true;
                    }

                    if (isEmpty)
                    {
                        return Enumerable.Empty<AvailableDateDto>();
                    }

                    return new Collection<AvailableDateDto>
                    {
                        AvailableDateDto.New(DateTime.Now, false),
                        AvailableDateDto.New(DateTime.Now, true)
                    };
                });            

            return repositoryMock.Object;
        }

        private static INotification GetNotification()
        {
            var notificationMock = new Mock<INotification>();

            notificationMock.SetupGet(x => x.HasCriticalError).Returns(() => { return _IsCriticalError; });

            notificationMock.SetupGet(x => x.HasErrors).Returns(() => { return _errors.Count > 0; });

            notificationMock.SetupGet(x => x.HasCriticalError).Returns(() => { return _IsCriticalError; });

            notificationMock.SetupGet(x => x.Errors).Returns(() => { return _errors; });

            return notificationMock.Object;
        }
    }
}
