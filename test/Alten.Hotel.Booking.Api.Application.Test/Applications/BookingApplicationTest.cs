using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Aggregates;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xunit;

namespace Alten.Hotel.Booking.Api.Application.Test.Applications
{
    public class BookingApplicationTest
    {
        private readonly BookingApplication _bookingApplication;

        private static Collection<string> _errors;

        private static bool _IsCriticalError;

        private static bool _throwException;

        public BookingApplicationTest()
        {
            _throwException = false;
            _errors = new Collection<string>();
            _IsCriticalError = false;
            _bookingApplication = new BookingApplication(GetBookingRepository(), GetMapper(),
                GetBookingService(), GetNotification(), GetLogger());
        }

        [Fact]
        public async Task Should_Get_Booking_When_BookingId_Is_Valid()
        {
            var bookingDto = await _bookingApplication.GetAsync(Guid.NewGuid());

            Assert.NotNull(bookingDto);
            Assert.NotEqual(BookingDto.Empty, bookingDto);
        }

        [Fact]
        public async Task Should_Not_Get_Booking_When_BookingId_Is_Invalid()
        {
            var bookingDto = await _bookingApplication.GetAsync(Guid.Empty);

            Assert.NotNull(bookingDto);
            Assert.Equal(BookingDto.Empty, bookingDto);
        }

        [Fact]
        public async Task Should_Not_Get_Booking_When_Exception_Is_Threw()
        {
            _throwException = true;
            var bookingDto = await _bookingApplication.GetAsync(Guid.NewGuid());

            Assert.NotNull(bookingDto);
            Assert.Equal(BookingDto.Empty, bookingDto);
            Assert.Contains(_errors, x => x.Equals("The operation was aborted."));
            Assert.True(_IsCriticalError);
        }

        [Fact]
        public async Task Should_Create_Reservation()
        {
            var bookingDto = BookingDto.New(DateTime.Now, DateTime.Now);
            var bookingId = await _bookingApplication.CreateAsync(bookingDto);

            Assert.NotEqual(Guid.Empty, bookingId);
            Assert.Empty(_errors);
        }

        [Fact]
        public async Task Should_Not_Create_Reservation()
        {
            var bookingDto = BookingDto.New(DateTime.Now.AddDays(-1), DateTime.Now);
            var bookingId = await _bookingApplication.CreateAsync(bookingDto);

            Assert.Equal(Guid.Empty, bookingId);
            Assert.Empty(_errors);
        }

        [Fact]
        public async Task Should_Delete_Booking_When_BookingId_Is_Valid()
        {
            await _bookingApplication.DeleteAsync(Guid.NewGuid());

            Assert.Empty(_errors);
        }

        [Fact]
        public async Task Should_Not_Delete_Booking_When_BookingId_Is_Invalid()
        {
            await _bookingApplication.DeleteAsync(Guid.Empty);

            Assert.Contains(_errors, x => x.Equals("The resevation couldn't be deleted."));
        }

        [Fact]
        public async Task Should_Not_Delete_Booking_When_Exception_Is_Threw()
        {
            _throwException = true;

            await _bookingApplication.DeleteAsync(Guid.NewGuid());

            Assert.Contains(_errors, x => x.Equals("The operation was aborted."));
            Assert.True(_IsCriticalError);
        }

        [Fact]
        public async Task Should_Update_Reservation()
        {
            var bookingDto = BookingDto.New(DateTime.Now, DateTime.Now);
            await _bookingApplication.UpdateAsync(Guid.NewGuid(), bookingDto);

            Assert.Empty(_errors);
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Exception_Is_Threw()
        {
            _throwException = true;
            var bookingDto = BookingDto.New(DateTime.Now.AddDays(-1), DateTime.Now);
            await _bookingApplication.UpdateAsync(Guid.Empty, bookingDto);

            Assert.Contains(_errors, x => x.Equals("The operation was aborted."));
            Assert.True(_IsCriticalError);
        }

        private static IBookingRepository GetBookingRepository()
        {
            var repositoryMock = new Mock<IBookingRepository>();

            repositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Guid bookingId) =>
                {
                    if (_throwException)
                    {
                        throw new Exception();
                    }

                    if (Guid.Empty.Equals(bookingId))
                    {
                        return Domain.Aggregates.BookingAggregate.Booking.Empty as Domain.Aggregates.BookingAggregate.Booking;
                    }

                    return Domain.Aggregates.BookingAggregate.Booking.New(DateTime.Now, DateTime.Now) as Domain.Aggregates.BookingAggregate.Booking;
                });

            repositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Domain.Aggregates.BookingAggregate.Booking>()))
                .ReturnsAsync((Domain.Aggregates.BookingAggregate.Booking booking) =>
                {
                    return true;
                });

            return repositoryMock.Object;
        }

        private static INotification GetNotification()
        {
            var notificationMock = new Mock<INotification>();

            notificationMock.Setup(x => x.AddNotification(It.IsAny<string>()))
                .Callback((string notification) =>
                {
                    _errors.Add(notification);
                });

            notificationMock.Setup(x => x.AddCritical(It.IsAny<string>()))
                .Callback((string notification) =>
                {
                    _errors.Add(notification);
                    _IsCriticalError = true;
                });

            return notificationMock.Object;
        }

        private static ILogger<BookingApplication> GetLogger()
        {
            var loggerMock = new Mock<ILogger<BookingApplication>>();

            return loggerMock.Object;
        }

        private static IMapper GetMapper()
        {
            var mapperMock = new Mock<IMapper>();

            mapperMock.Setup(x => x.Map<IBooking>(It.IsAny<BookingDto>()))
                .Returns((BookingDto bookingDto) =>
                {
                    return Domain.Aggregates.BookingAggregate.Booking.New(bookingDto.CheckIn, bookingDto.CheckOut);
                });

            mapperMock.Setup(x => x.Map<BookingDto>(It.IsAny<Domain.Aggregates.BookingAggregate.Booking>()))
                .Returns((Domain.Aggregates.BookingAggregate.Booking booking) =>
                {
                    return BookingDto.New(booking.CheckIn, booking.CheckOut);
                });

            return mapperMock.Object;
        }

        private static IBookingService GetBookingService()
        {
            var service = new Mock<IBookingService>();

            service.Setup(x => x.CreateReservationAsync(It.IsAny<IBooking>())).
                ReturnsAsync((IBooking booking) =>
                {
                    if (DateTime.Compare(booking.CheckIn, DateTime.Now.Date) < 0)
                    {
                        return Guid.Empty;
                    }

                    return Guid.NewGuid();
                });

            return service.Object;
        }
    }
}
