using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Infrastructure.Repositories
{
    /// <summary>
    /// Booking's repository.
    /// </summary>
    /// <seealso cref="IBookingRepository" />
    public class BookingRepository : IBookingRepository
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IRepository<Domain.Aggregates.BookingAggregate.Booking> _repository;

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingRepository"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public BookingRepository(IRepository<Domain.Aggregates.BookingAggregate.Booking> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Inserts the specified booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true if successfully inserted.</returns>
        public async Task<bool> InsertAsync(Domain.Aggregates.BookingAggregate.Booking booking)
        {
           return await _repository.InsertAsync(booking);
        }

        /// <summary>
        /// Gets the specified booking.
        /// </summary>
        /// <param name="bookingId">The identifier.</param>
        /// <returns>Retunr the booking</returns>
        public async Task<Domain.Aggregates.BookingAggregate.Booking> GetAsync(Guid bookingId)
        {
            var entity = await _repository.GetAsync(x => x.Id.Equals(bookingId));

            if (entity is null)
            {
                return Domain.Aggregates.BookingAggregate.Booking.Empty as Domain.Aggregates.BookingAggregate.Booking;
            }

            return entity;
        }

        /// <summary>
        /// Deletes the booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true if successfully deleted.</returns>
        public async Task<bool> DeleteAsync(Domain.Aggregates.BookingAggregate.Booking booking)
        {
            return await _repository.DeleteAsync(booking);
        }

        /// <summary>
        /// Updates the booking.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true if successfully updated.</returns>
        public async Task<bool> UpdateAsync(Domain.Aggregates.BookingAggregate.Booking booking)
        {
            return await _repository.UpdateAsync(booking);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        [ExcludeFromCodeCoverage]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
