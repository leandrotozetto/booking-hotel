using Alten.Hotel.Booking.Api.Domain.Dto;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Aggregates;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Applications;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Alten.Hotel.Booking.Api.Application
{
    public class BookingApplication : IBookingApplication
    {
        /// <summary>
        /// The repository
        /// </summary>
        private readonly IBookingRepository _bookingRepository;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// The booking service
        /// </summary>
        private readonly IBookingService _bookingService;

        /// <summary>
        /// The notification
        /// </summary>
        private readonly INotification _notification;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<BookingApplication> _logger;

        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue = false;

        public BookingApplication(IBookingRepository repository, IMapper mapper,
            IBookingService bookingService, INotification notification, ILogger<BookingApplication> logger)
        {
            _bookingRepository = repository;
            _mapper = mapper;
            _bookingService = bookingService;
            _notification = notification;
            _logger = logger;
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="bookingId">The booking identifier.</param>
        /// <returns>Returns the booking</returns>
        public async Task<BookingResponseDto> GetAsync(Guid bookingId)
        {
            try
            {
                var booking = await _bookingRepository.GetAsync(bookingId);

                if (Domain.Aggregates.BookingAggregate.Booking.Empty.Equals(booking))
                {
                    return BookingResponseDto.Empty;
                }

                var bookindDto = _mapper.Map<BookingResponseDto>(booking);

                return bookindDto;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, "[GetAsync] - Operation Aborted");

                _notification.AddCritical("The operation was aborted.");

                return BookingResponseDto.Empty;
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <returns>Returns true if successfully created.</returns>
        public async Task<Guid> CreateAsync(BookingDto bookingDto)
        {
            var booking = _mapper.Map<IBooking>(bookingDto);

            return await _bookingService.CreateReservationAsync(booking);
        }

        /// <summary>
        /// Updates the booking.
        /// </summary>
        /// <param name="bookingId">The booking identifier.</param>
        /// <param name="booking">The booking.</param>
        public async Task UpdateAsync(Guid bookingId, BookingDto bookingDto)
        {
            try
            {
                var booking = await GetById(bookingId);

                if (!Domain.Aggregates.BookingAggregate.Booking.Empty.Equals(booking))
                {
                    booking.ChangeDate(bookingDto.CheckIn, bookingDto.CheckOut);
                }

                await _bookingService.UpdateReservationAsync(booking);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, "[UpdateAsync] - Operation Aborted");

                _notification.AddCritical("The operation was aborted.");
            }
        }

        /// <summary>
        /// Updates the booking.
        /// </summary>
        /// <param name="bookingId">The booking identifier.</param>
        /// <param name="booking">The booking.</param>
        public async Task DeleteAsync(Guid bookingId)
        {
            try
            {
                var deleted = false;
                var booking = await GetById(bookingId);

                if (!Domain.Aggregates.BookingAggregate.Booking.Empty.Equals(booking))
                {
                    deleted = await _bookingRepository.DeleteAsync(booking);
                }

                if (!deleted)
                {
                    _notification.AddNotification($"The resevation couldn't be deleted.");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, "[DeleteAsync] - Operation Aborted");

                _notification.AddCritical("The operation was aborted.");
            }
        }

        private async Task<Domain.Aggregates.BookingAggregate.Booking> GetById(Guid bookingId)
        {
            var booking = await _bookingRepository.GetAsync(bookingId);

            if (Domain.Aggregates.BookingAggregate.Booking.Empty.Equals(booking))
            {
                _notification.AddNotification($"The resevation {bookingId} doesn't exist.");
            }

            return booking;
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
                    _bookingRepository.Dispose();
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
