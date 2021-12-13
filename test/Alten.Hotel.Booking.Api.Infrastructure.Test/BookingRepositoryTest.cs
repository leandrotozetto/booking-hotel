using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using Alten.Hotel.Booking.Api.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Alten.Hotel.Booking.Api.Infrastructure.Test
{
    public class BookingRepositoryTest
    {
        private readonly BookingRepository _bookingRepository;

        private readonly IList<Domain.Aggregates.BookingAggregate.Booking> _entities;

        public BookingRepositoryTest()
        {
            _bookingRepository = new BookingRepository(GetRepository());

            _entities = CreateEntities();
        }

        [Fact]
        public async Task Should_Get_Booking()
        {
            var booking = await _bookingRepository.GetAsync(_entities[0].Id);

            Assert.NotNull(booking);
        }

        [Fact]
        public async Task Should_Not_Get_Booking_When_Booking_Not_Exists()
        {
            var booking = await _bookingRepository.GetAsync(Guid.Empty);

            Assert.Equal(Domain.Aggregates.BookingAggregate.Booking.Empty, booking);
        }

        [Fact]
        public async Task Should_Insert_Booking()
        {
            var booking = CreateBooking(new DateTime(2021, 12, 15), new DateTime(2021, 12, 16));
            var inserted = await _bookingRepository.InsertAsync(booking);

            Assert.True(inserted);
        }

        [Fact]
        public async Task Should_Not_Insert_Booking_When_Booking_Is_Invalid()
        {
            var booking = Domain.Aggregates.BookingAggregate.Booking.Empty as Domain.Aggregates.BookingAggregate.Booking;
            var inserted = await _bookingRepository.InsertAsync(booking);

            Assert.False(inserted);
        }

        [Fact]
        public async Task Should_Update_Booking()
        {
            var booking = CreateBooking(new DateTime(2021, 12, 15), new DateTime(2021, 12, 16));
            var inserted = await _bookingRepository.UpdateAsync(booking);

            Assert.True(inserted);
        }

        [Fact]
        public async Task Should_Not_Update_Booking_When_Booking_Not_Exist()
        {
            var booking = Domain.Aggregates.BookingAggregate.Booking.Empty as Domain.Aggregates.BookingAggregate.Booking;
            var inserted = await _bookingRepository.UpdateAsync(booking);

            Assert.False(inserted);
        }

        [Fact]
        public async Task Should_Delete_Booking()
        {
            var booking = CreateBooking(new DateTime(2021, 12, 15), new DateTime(2021, 12, 16));
            var inserted = await _bookingRepository.DeleteAsync(booking);

            Assert.True(inserted);
        }

        [Fact]
        public async Task Should_Not_Delete_Booking_When_Booking_Not_Exist()
        {
            var booking = Domain.Aggregates.BookingAggregate.Booking.Empty as Domain.Aggregates.BookingAggregate.Booking;
            var inserted = await _bookingRepository.DeleteAsync(booking);

            Assert.False(inserted);
        }

        private IRepository<Domain.Aggregates.BookingAggregate.Booking> GetRepository()
        {
            var repositoryMock = new Mock<IRepository<Domain.Aggregates.BookingAggregate.Booking>>();

            repositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Domain.Aggregates.BookingAggregate.Booking, bool>>>()))
                .ReturnsAsync((Expression<Func<Domain.Aggregates.BookingAggregate.Booking, bool>> filter) =>
                {
                    return _entities.FirstOrDefault(filter.Compile());
                });

            repositoryMock.Setup(x => x.InsertAsync(It.IsAny<Domain.Aggregates.BookingAggregate.Booking>()))
                .ReturnsAsync((Domain.Aggregates.BookingAggregate.Booking booking) =>
                {
                    if (booking is null || Domain.Aggregates.BookingAggregate.Booking.Empty.Equals(booking))
                    {
                        return false;
                    }

                    return true;
                });

            repositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Domain.Aggregates.BookingAggregate.Booking>()))
                .ReturnsAsync((Domain.Aggregates.BookingAggregate.Booking booking) =>
                {
                    if (booking is null || Domain.Aggregates.BookingAggregate.Booking.Empty.Equals(booking))
                    {
                        return false;
                    }

                    return true;
                });

            repositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Domain.Aggregates.BookingAggregate.Booking>()))
                .ReturnsAsync((Domain.Aggregates.BookingAggregate.Booking booking) =>
                {
                    if (booking is null || Domain.Aggregates.BookingAggregate.Booking.Empty.Equals(booking))
                    {
                        return false;
                    }

                    return true;
                });

            return repositoryMock.Object;
        }

        private static IList<Domain.Aggregates.BookingAggregate.Booking> CreateEntities()
        {
            return new Collection<Domain.Aggregates.BookingAggregate.Booking>()
            {
                CreateBooking(new DateTime(2021, 12, 01), new DateTime(2021, 12, 01)),
                CreateBooking(new DateTime(2021, 12, 02), new DateTime(2021, 12, 03)),
                CreateBooking(new DateTime(2021, 12, 04), new DateTime(2021, 12, 06)),
                CreateBooking(new DateTime(2021, 12, 08), new DateTime(2021, 12, 09)),
                CreateBooking(new DateTime(2021, 12, 10), new DateTime(2021, 12, 11))
            };
        }

        private static Domain.Aggregates.BookingAggregate.Booking CreateBooking(DateTime checkIn, DateTime checkout)
        {
            return Domain.Aggregates.BookingAggregate.Booking.New(checkIn, checkout) as Domain.Aggregates.BookingAggregate.Booking;
        }
    }
}
