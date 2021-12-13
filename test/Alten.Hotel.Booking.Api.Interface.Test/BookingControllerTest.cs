using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Applications;
using Alten.Hotel.Booking.Api.Interface.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xunit;

namespace Alten.Hotel.Booking.Api.Interface.Test
{
    public class BookingControllerTest
    {
        private readonly BookingController _bookingController;

        private static Collection<string> _errors;

        private static bool _IsCriticalError;

        private static bool _throwException;

        public BookingControllerTest()
        {
            _errors = new Collection<string>();
            _IsCriticalError = false;
            _throwException = false;
            _bookingController = new BookingController(GetBookingApplication(), GetNotification());
        }

        [Fact]
        public async Task Should_Create_Reservation()
        {
            var bookingDto = BookingDto.New(DateTime.Now, DateTime.Now);
            var actionResult = await _bookingController.CreateAsync(bookingDto);

            Assert.NotNull(actionResult);
            Assert.IsType<OkObjectResult>(actionResult);

            var okObjectResult = actionResult as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            var bookingId = okObjectResult.Value.GetType().GetProperty("BookingId").GetValue(okObjectResult.Value, null);
            Assert.NotEqual(Guid.Empty, bookingId);
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Booking_Is_Invalid()
        {
            var bookingDto = BookingDto.Empty;
            var actionResult = await _bookingController.CreateAsync(bookingDto);

            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);

            var badRequestObjectResult = actionResult as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

            var errorDto = badRequestObjectResult.Value as ErrorDto;
            Assert.NotEmpty(errorDto.Errors);
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Exception_Is_Threw()
        {
            _throwException = true;
            var bookingDto = BookingDto.Empty;
            var actionResult = await _bookingController.CreateAsync(bookingDto);

            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = actionResult as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var errorDto = objectResult.Value as ErrorDto;
            Assert.NotEmpty(errorDto.Errors);
        }

        [Fact]
        public async Task Should_Delete_Reservation()
        {
            var actionResult = await _bookingController.DeleteAsync(Guid.NewGuid());

            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);

            var okObjectResult = actionResult as NoContentResult;
            
            Assert.NotNull(okObjectResult);
            Assert.Equal(204, okObjectResult.StatusCode);
        }

        [Fact]
        public async Task Should_Not_Delete_Reservation_When_Booking_Is_Invalid()
        {
            var actionResult = await _bookingController.DeleteAsync(Guid.Empty);

            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);

            var badRequestObjectResult = actionResult as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

            var errorDto = badRequestObjectResult.Value as ErrorDto;
            Assert.NotEmpty(errorDto.Errors);
        }

        [Fact]
        public async Task Should_Not_Delete_Reservation_When_Exception_Is_Threw()
        {
            _throwException = true;
            var actionResult = await _bookingController.DeleteAsync(Guid.Empty);

            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = actionResult as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var errorDto = objectResult.Value as ErrorDto;
            Assert.NotEmpty(errorDto.Errors);
        }

        [Fact]
        public async Task Should_Get_Reservation()
        {
            var actionResult = await _bookingController.GetAsync(Guid.NewGuid());

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
            var actionResult = await _bookingController.GetAsync(Guid.Empty);

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
            var actionResult = await _bookingController.GetAsync(Guid.Empty);

            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = actionResult as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var errorDto = objectResult.Value as ErrorDto;
            Assert.NotEmpty(errorDto.Errors);
        }

        [Fact]
        public async Task Should_Update_Reservation()
        {
            var bookingDto = BookingDto.New(DateTime.Now, DateTime.Now);
            var actionResult = await _bookingController.UpdateAsync(Guid.NewGuid(), bookingDto);

            Assert.NotNull(actionResult);
            Assert.IsType<NoContentResult>(actionResult);

            var noContentResult = actionResult as NoContentResult;

            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Booking_Is_Invalid()
        {
            var bookingDto = BookingDto.Empty;
            var actionResult = await _bookingController.UpdateAsync(Guid.Empty, bookingDto);

            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestObjectResult>(actionResult);

            var badRequestObjectResult = actionResult as BadRequestObjectResult;

            Assert.NotNull(badRequestObjectResult);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

            var errorDto = badRequestObjectResult.Value as ErrorDto;
            Assert.NotEmpty(errorDto.Errors);
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Exception_Is_Threw()
        {
            _throwException = true;
            var bookingDto = BookingDto.Empty;
            var actionResult = await _bookingController.UpdateAsync(Guid.Empty, bookingDto);

            Assert.NotNull(actionResult);
            Assert.IsType<ObjectResult>(actionResult);

            var objectResult = actionResult as ObjectResult;

            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var errorDto = objectResult.Value as ErrorDto;
            Assert.NotEmpty(errorDto.Errors);
        }

        private static IBookingApplication GetBookingApplication()
        {
            var repositoryMock = new Mock<IBookingApplication>();

            repositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid bookingId) =>
                {
                    HasThrowException();

                    if (Guid.Empty.Equals(bookingId))
                    {
                        return BookingResponseDto.Empty;
                    }

                    return BookingResponseDto.New(Guid.NewGuid(), DateTime.Now, DateTime.Now);
                });

            repositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Callback((Guid bookingId) =>
                {
                    HasThrowException();

                    if (Guid.Empty.Equals(bookingId))
                    {
                        _errors.Add("The operation was aborted.");
                    }
                });

            repositoryMock.Setup(x => x.CreateAsync(It.IsAny<BookingDto>()))
                .ReturnsAsync((BookingDto bookingDto) =>
                {
                    HasThrowException();

                    if (BookingDto.Empty.Equals(bookingDto))
                    {
                        _errors.Add("The operation was aborted.");

                        return Guid.Empty;
                    }

                    return Guid.NewGuid();
                });

            repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<BookingDto>()))
                .Callback((Guid bookingId, BookingDto bookingDto) =>
                {
                    HasThrowException();

                    if (BookingDto.Empty.Equals(bookingDto))
                    {
                        _errors.Add("The operation was aborted.");
                    }
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

        private static void HasThrowException()
        {
            if (_throwException)
            {
                _errors.Add("The operation was aborted.");
                _IsCriticalError = true;
            }
        }
    }
}
