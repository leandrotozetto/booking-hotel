using Alten.Hotel.Booking.Api.Domain.Interfaces.Aggregates;
using Alten.Hotel.Booking.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Alten.Hotel.Booking.Api.Infrastructure.Test
{
    public class RepositoryTest
    {
        private static readonly DbContextOptions<BookingDbContext> _dbContextOptions = new DbContextOptionsBuilder<BookingDbContext>()
                .UseInMemoryDatabase(databaseName: "BookingDb")
                .Options;

        private readonly BookingDbContext _bookingContext = new(_dbContextOptions);

        private readonly Repository<Domain.Aggregates.BookingAggregate.Booking> _repository;

        private readonly DbSet<Domain.Aggregates.BookingAggregate.Booking> _dbSet;

        public RepositoryTest()
        {
            _dbSet = _bookingContext.Set<Domain.Aggregates.BookingAggregate.Booking>();
            _bookingContext.Database.EnsureCreated();
            _repository = new Repository<Domain.Aggregates.BookingAggregate.Booking>(_bookingContext);
        }

        [Fact]
        public async Task Should_Return_Entity_When_Entity_Exists()
        {
            var booking = Domain.Aggregates.BookingAggregate.Booking.New(DateTime.Now, DateTime.Now);

            AddEntity(booking);

            var entity = await _repository.GetAsync(x => x.Id.Equals(booking.Id));

            Assert.NotNull(entity);
            Assert.Equal(booking.Id, entity.Id);
        }

        [Fact]
        public async Task Should_Not_Return_Entity_When_Entity_Not_Exists()
        {
            var booking = Domain.Aggregates.BookingAggregate.Booking.New(DateTime.Now, DateTime.Now);

            AddEntity(booking);

            var entity = await _repository.GetAsync(x => x.Id.Equals(Guid.Empty));

            Assert.Null(entity);
        }

        [Fact]
        public async Task Should_Insert_Entity()
        {
            var booking = Domain.Aggregates.BookingAggregate.Booking.New(DateTime.Now, DateTime.Now);
            var inserted = await _repository.InsertAsync(booking as Domain.Aggregates.BookingAggregate.Booking);

            Assert.True(inserted);
        }

        [Fact]
        public async Task Should_Not_Insert_When_Entity_Is_Null()
        {
            var deleted = await _repository.InsertAsync(null);

            Assert.False(deleted);
        }

        [Fact]
        public async Task Should_Update_Entity()
        {
            var booking = Domain.Aggregates.BookingAggregate.Booking.New(DateTime.Now, DateTime.Now);

            AddEntity(booking);

            booking.CheckOut.AddDays(2);

            var inserted = await _repository.UpdateAsync(booking as Domain.Aggregates.BookingAggregate.Booking);
            var insertedEntity = GetEntity(booking.Id);

            Assert.True(inserted);
            Assert.Equal(booking.CheckOut, insertedEntity.CheckOut);
        }

        [Fact]
        public async Task Should_Not_Update_When_Entity_Is_Null()
        {
            var inserted = await _repository.UpdateAsync(null);

            Assert.False(inserted);
        }

        [Fact]
        public async Task Should_Delete_Entity()
        {
            var booking = Domain.Aggregates.BookingAggregate.Booking.New(DateTime.Now, DateTime.Now);

            AddEntity(booking);

            var deleted = await _repository.DeleteAsync(booking as Domain.Aggregates.BookingAggregate.Booking);
            var deletedEntity = GetEntity(booking.Id);

            Assert.True(deleted);
            Assert.Null(deletedEntity);
        }

        [Fact]
        public async Task Should_Not_Delete_When_Entity_Is_Null()
        {
            var deleted = await _repository.DeleteAsync(null);

            Assert.False(deleted);
        }

        private void AddEntity(IBooking booking)
        {
            _dbSet.Add(booking as Domain.Aggregates.BookingAggregate.Booking);

            _bookingContext.SaveChanges();
        }

        private Domain.Aggregates.BookingAggregate.Booking GetEntity(Guid bookingId)
        {
           return _dbSet.Find(bookingId);
        }
    }
}