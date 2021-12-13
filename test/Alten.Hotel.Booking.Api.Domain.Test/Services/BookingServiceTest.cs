using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Services;
using Alten.Hotel.Booking.Api.Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Alten.Hotel.Booking.Api.Domain.Test.Services
{
    public class BookingServiceTest
    {
        private readonly BookingService _bookingService;

        private static Collection<string> _errors;

        private static bool _IsCriticalError;

        private static bool _throwExceptionOnGetAvailableDates;

        private static bool _emptyListOnGetAvailableDates;

        public BookingServiceTest()
        {
            _errors = new Collection<string>();
            _IsCriticalError = false;
            _throwExceptionOnGetAvailableDates = false;
            _emptyListOnGetAvailableDates = false;
            _bookingService = new BookingService(GetCalendarService(), GetBookingRepository(),
                GetNotification(), GetLogger());
        }

        [Fact]
        public async Task Should_Create_Reservation()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(2).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.NotEqual(Guid.Empty, reservationId);
            Assert.Empty(_errors);
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Past_Dates()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(-3).Date, DateTime.Now.AddDays(-2).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(-3).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(-2).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Booking_Has_One_Date_And_It_Is_Unvailable()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(3).Date, DateTime.Now.AddDays(3).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(3).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Booking_Has_Three_Dates_And_One_Date_Is_Unvailable()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(3).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(3).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Equal(Guid.Empty, reservationId);
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Booking_Has_Two_Dates_And_Two_Dates_Are_Unvailable()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(3).Date, DateTime.Now.AddDays(4).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(3).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(4).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Booking_Has_Three_Dates_And_All_Dates_Are_Unvailable()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(3).Date, DateTime.Now.AddDays(5).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(3).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(4).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(5).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Booking_Is_Null()
        {
            var reservationId = await _bookingService.CreateReservationAsync(null);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals("The object booking is invalid"));
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_CheckOut_Is_Later_Than_Last_Available_Date()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(13).Date, DateTime.Now.AddDays(15).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);

            Assert.Contains(_errors, x => x.Equals($"The dates of stay need to be between {DateTime.Now.AddDays(1).Date:yyyy-MM-dd} and {DateTime.Now.AddDays(13).Date:yyyy-MM-dd}."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(14).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(15).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Booking_Was_Not_Inserted()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(11).Date, DateTime.Now.AddDays(11).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals("The resevation couldn't be inserted."));
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Booking_Is_Empty()
        {
            var booking = Aggregates.BookingAggregate.Booking.Empty;

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals("The object booking is invalid"));
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Exception_Is_Threw_On_Insert()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(10).Date, DateTime.Now.AddDays(10).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals("The operation was aborted."));
            Assert.Contains(_errors, x => x.Equals("The resevation couldn't be inserted."));
            Assert.True(_IsCriticalError);
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_Exception_Is_Threw_On_CreateReservation()
        {
            _throwExceptionOnGetAvailableDates = true;
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(10).Date, DateTime.Now.AddDays(10).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals("The operation was aborted."));
            Assert.True(_IsCriticalError);
        }

        [Fact]
        public async Task Should_Not_Create_Reservation_When_GetAvailableDates_Returns_Empty_List()
        {
            _emptyListOnGetAvailableDates = true;
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(10).Date, DateTime.Now.AddDays(10).Date);

            var reservationId = await _bookingService.CreateReservationAsync(booking);

            Assert.Equal(Guid.Empty, reservationId);
            Assert.Contains(_errors, x => x.Equals("It was not possible to get the dates."));
            Assert.False(_IsCriticalError);
        }

        [Fact]
        public async Task Should_Update_Reservation()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(2).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Empty(_errors);
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Past_Dates()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(-3).Date, DateTime.Now.AddDays(-2).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(-3).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(-2).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Booking_Has_One_Date_And_It_Is_Unvailable()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(3).Date, DateTime.Now.AddDays(3).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(3).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Booking_Has_Three_Dates_And_One_Date_Is_Unvailable()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(1).Date, DateTime.Now.AddDays(3).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(3).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Booking_Has_Two_Dates_And_Two_Dates_Are_Unvailable()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(3).Date, DateTime.Now.AddDays(4).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(3).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(4).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Booking_Has_Three_Dates_And_All_Dates_Are_Unvailable()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(3).Date, DateTime.Now.AddDays(5).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(3).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(4).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(5).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Booking_Is_Null()
        {
            await _bookingService.UpdateReservationAsync(null);

            Assert.Contains(_errors, x => x.Equals("The object booking is invalid"));
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_CheckOut_Is_Later_Than_Last_Available_Date()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(13).Date, DateTime.Now.AddDays(15).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals($"The dates of stay need to be between {DateTime.Now.AddDays(1).Date:yyyy-MM-dd} and {DateTime.Now.AddDays(13).Date:yyyy-MM-dd}."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(14).Date.Date:yyyy-MM-dd} is unavailable."));
            Assert.Contains(_errors, x => x.Equals($"The date {DateTime.Now.AddDays(15).Date.Date:yyyy-MM-dd} is unavailable."));
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Booking_Was_Not_Inserted()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(11).Date, DateTime.Now.AddDays(11).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals("The resevation couldn't be updated."));
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Booking_Is_Empty()
        {
            var booking = Aggregates.BookingAggregate.Booking.Empty;

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals("The object booking is invalid"));
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Exception_Is_Threw_On_Update()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(10).Date, DateTime.Now.AddDays(10).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals("The operation was aborted."));
            Assert.Contains(_errors, x => x.Equals("The resevation couldn't be updated."));
            Assert.True(_IsCriticalError);
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_Exception_Is_Threw_On_UpdateReservation()
        {
            _throwExceptionOnGetAvailableDates = true;
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(10).Date, DateTime.Now.AddDays(10).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals("The operation was aborted."));
            Assert.True(_IsCriticalError);
        }

        [Fact]
        public async Task Should_Not_Update_Reservation_When_GetAvailableDates_Returns_Empty_List()
        {
            _emptyListOnGetAvailableDates = true;
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(10).Date, DateTime.Now.AddDays(10).Date);

            await _bookingService.UpdateReservationAsync(booking);

            Assert.Contains(_errors, x => x.Equals("It was not possible to get the dates."));
            Assert.False(_IsCriticalError);
        }

        private static IBookingRepository GetBookingRepository()
        {
            var repositoryMock = new Mock<IBookingRepository>();

            repositoryMock.Setup(x => x.InsertAsync(It.IsAny<Aggregates.BookingAggregate.Booking>()))
                .ReturnsAsync((Aggregates.BookingAggregate.Booking booking) =>
                {
                    if (booking is null || Aggregates.BookingAggregate.Booking.Empty.Equals(booking)
                    || (booking.CheckIn == DateTime.Now.AddDays(11).Date
                            && booking.CheckOut == DateTime.Now.AddDays(11).Date))
                    {
                        return false;
                    }
                    if (booking.CheckIn == DateTime.Now.AddDays(10).Date
                            && booking.CheckOut == DateTime.Now.AddDays(10).Date)
                    {
                        throw new Exception();
                    }

                    return true;
                });

            repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Aggregates.BookingAggregate.Booking>()))
                .ReturnsAsync((Aggregates.BookingAggregate.Booking booking) =>
                {
                    if (booking is null || Aggregates.BookingAggregate.Booking.Empty.Equals(booking)
                    || (booking.CheckIn == DateTime.Now.AddDays(11).Date
                            && booking.CheckOut == DateTime.Now.AddDays(11).Date))
                    {
                        return false;
                    }
                    if (booking.CheckIn == DateTime.Now.AddDays(10).Date
                            && booking.CheckOut == DateTime.Now.AddDays(10).Date)
                    {
                        throw new Exception();
                    }

                    return true;
                });

            return repositoryMock.Object;
        }

        private static ICalendarService GetCalendarService()
        {
            var serviceMock = new Mock<ICalendarService>();

            serviceMock.Setup(x => x.GetAvailableDatesAsync(It.IsAny<Guid?>()))
                .ReturnsAsync((Guid? boolingId) =>
                {
                    if (_throwExceptionOnGetAvailableDates)
                    {
                        throw new Exception();
                    }

                    if (_emptyListOnGetAvailableDates)
                    {
                        return Enumerable.Empty<Aggregates.HotelAggregate.AvailableDate>();
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

        private static ILogger<BookingService> GetLogger()
        {
            var loggerMock = new Mock<ILogger<BookingService>>();

            return loggerMock.Object;
        }
    }
}
