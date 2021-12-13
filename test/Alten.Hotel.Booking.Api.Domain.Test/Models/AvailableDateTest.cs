using System;
using Xunit;

namespace Alten.Hotel.Booking.Api.Domain.Test.Models
{
    public class AvailableDateTest
    {
        [Fact]
        public void Should_Create_New_AvailableDate()
        {
            var id = Guid.NewGuid();
            var date = DateTime.Now;

            var availableDate = Aggregates.HotelAggregate.AvailableDate.New(date, id);

            Assert.NotNull(availableDate);
            Assert.Equal(date, availableDate.Date);
            Assert.Equal(id, availableDate.BookingId);
        }

        [Fact]
        public void Should_Be_Unavailable_When_BookingId_Is_Valid()
        {
            var availableDate = Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now, Guid.NewGuid());

            Assert.NotNull(availableDate);
            Assert.False(availableDate.IsAvailable);
        }

        [Fact]
        public void Should_Be_Available_When_BookingId_Is_Null()
        {
            var availableDate = Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now);

            Assert.NotNull(availableDate);
            Assert.True(availableDate.IsAvailable);
        }

        [Fact]
        public void Should_Be_Available_When_BookingId_Is_Empty()
        {
            var availableDate = Aggregates.HotelAggregate.AvailableDate.New(DateTime.Now, Guid.Empty);

            Assert.NotNull(availableDate);
            Assert.True(availableDate.IsAvailable);
        }
    }
}
