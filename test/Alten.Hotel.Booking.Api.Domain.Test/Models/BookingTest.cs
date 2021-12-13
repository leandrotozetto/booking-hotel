using System;
using Xunit;

namespace Alten.Hotel.Booking.Api.Domain.Test.Models
{
    public class BookingTest
    {
        //private Domain.Aggregates.BookingAggregate.Booking _booking;

        //public BookingTest()
        //{
        //    booking = Domain.Aggregates.BookingAggregate.Booking.New()
        //}

        private readonly string _checkOutField = nameof(Aggregates.BookingAggregate.Booking.CheckOut);

        private readonly string _checkInField = nameof(Aggregates.BookingAggregate.Booking.CheckIn);

        private readonly DateTime _nextAvailableCheckin = Aggregates.BookingAggregate.Booking.NextAvailableCheckin;

        private readonly DateTime _lastAvailableCheckIn = Aggregates.BookingAggregate.Booking.LastAvailableCheckIn;

        private readonly DateTime _nextAvailableCheckOut = Aggregates.BookingAggregate.Booking.NextAvailableCheckOut;

        private readonly DateTime _lastAvailableCheckOut = Aggregates.BookingAggregate.Booking.LastAvailableCheckOut;

        [Fact]
        public void Should_Create_New_Booking()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now, DateTime.Now);

            Assert.NotNull(booking);
            Assert.NotEqual(Guid.Empty, booking.Id);
        }

        [Fact]
        public void Should_Create_Empty_Booking()
        {
            var booking = Aggregates.BookingAggregate.Booking.Empty;

            Assert.NotNull(booking);
            Assert.Equal(Guid.Empty, booking.Id);
            Assert.Equal(DateTime.MinValue, booking.CheckIn);
            Assert.Equal(DateTime.MinValue, booking.CheckOut);
            Assert.Empty(booking.Errors);
        }

        [Fact]
        public void Should_Change_Dates()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now, DateTime.Now);

            booking.ChangeDate(DateTime.Now.AddDays(1), DateTime.Now.AddDays(2));

            Assert.Equal(DateTime.Now.AddDays(1).Date, booking.CheckIn);
            Assert.Equal(DateTime.Now.AddDays(2).Date, booking.CheckOut);
        }

        [Fact]
        public void Should_Be_Valid()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.Now.AddDays(1), DateTime.Now.AddDays(1));

            Assert.True(booking.IsValid());
        }

        [Fact]
        public void Should_Be_Invalid_When_CheckIn_Is_Invalid()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(DateTime.MinValue, DateTime.Now.AddDays(1));

            Assert.False(booking.IsValid());
            Assert.Contains(booking.Errors, x => x.Equals($"The {_checkInField} is invalid."));
        }

        [Fact]
        public void Should_Be_Invalid_When_CheckOut_Is_Invalid()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(_nextAvailableCheckin, DateTime.MinValue);

            Assert.False(booking.IsValid());
            Assert.Contains(booking.Errors, x => x.Equals($"The {_checkOutField} is invalid."));
        }

        [Fact]
        public void Should_Be_Invalid_When_CheckIn_Is_Later_Than_CheckOut()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(_nextAvailableCheckin.AddDays(1), _nextAvailableCheckin);

            Assert.False(booking.IsValid());
            Assert.Contains(booking.Errors, x => x.Equals($"The {_checkInField} can't be later than {_nextAvailableCheckin:yyyy-MM-dd}."));
        }

        [Fact]
        public void Should_Be_Invalid_When_CheckIn_Is_Earlier_Than_Next_Available_CheckIn()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(_nextAvailableCheckin.AddDays(-2), _nextAvailableCheckin.AddDays(-2));

            Assert.False(booking.IsValid());
            Assert.Contains(booking.Errors, x => x.Equals($"The {_checkInField} can't be earlier than {_nextAvailableCheckin:yyyy-MM-dd}."));
        }

        [Fact]
        public void Should_Be_Invalid_When_CheckIn_Is_Later_Than_Last_Available_CheckIn()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(_lastAvailableCheckIn.AddDays(1), _lastAvailableCheckIn.AddDays(1));

            Assert.False(booking.IsValid());
            Assert.Contains(booking.Errors, x => x.Equals($"The {_checkInField} can't be later than {_lastAvailableCheckIn:yyyy-MM-dd}."));
        }

        [Fact]
        public void Should_Be_Invalid_When_CheckOut_Is_Earlier_Than_Next_Available_CheckOut()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(_nextAvailableCheckOut.AddDays(-2), _nextAvailableCheckOut.AddDays(-2));

            Assert.False(booking.IsValid());
            Assert.Contains(booking.Errors, x => x.Equals($"The {_checkInField} can't be earlier than {_nextAvailableCheckOut:yyyy-MM-dd}."));
        }

        [Fact]
        public void Should_Be_Invalid_When_CheckOut_Is_Later_Than_Last_Available_CheckOut()
        {
            var booking = Aggregates.BookingAggregate.Booking.New(_nextAvailableCheckOut.AddDays(1), _lastAvailableCheckOut.AddDays(3));

            Assert.False(booking.IsValid());
            Assert.Contains(booking.Errors, x => x.Equals($"The {_checkOutField} can't be later than {_lastAvailableCheckOut:yyyy-MM-dd}."));
        }
    }
}
