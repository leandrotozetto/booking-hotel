using System;
using System.Collections.ObjectModel;

namespace Alten.Hotel.Booking.Api.Domain.Interfaces.Aggregates
{
    /// <summary>
    /// Booking's contract
    /// </summary>
    public interface IBooking
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; }

        ///// <summary>
        ///// Gets the room number.
        ///// </summary>
        ///// <value>
        ///// The room number.
        ///// </value>
        //byte RoomNumber { get; }

        /// <summary>
        /// Gets the check-in date.
        /// </summary>
        /// <value>
        /// The check-in date.
        /// </value>
        public DateTime CheckIn { get; }

        /// <summary>
        /// Gets the check-out date.
        /// </summary>
        /// <value>
        /// The check-out date.
        /// </value>
        public DateTime CheckOut { get; }

        ///// <summary>
        ///// Gets the guest.
        ///// </summary>
        ///// <value>
        ///// The guest.
        ///// </value>
        //IGuest Guest { get; }

        ///// <summary>
        ///// Gets the payment.
        ///// </summary>
        ///// <value>
        ///// The payment.
        ///// </value>
        //IPayment Payment { get; }

        /// <summary>
        /// Error list
        /// </summary>
        Collection<string> Errors { get; }

        /// <summary>
        /// Changes the date.
        /// </summary>
        /// <param name="checkIn">The start date.</param>
        /// <param name="checkOut">The end date.</param>
        IBooking ChangeDate(DateTime checkIn, DateTime checkOut);

        /// <summary>
        /// Validates 
        /// </summary>
        /// <returns>Returns true if it's invalid</returns>
        bool IsValid();

        ///// <summary>
        ///// Changes the room.
        ///// </summary>
        ///// <param name="roomNumber">The room number.</param>
        ///// <returns></returns>
        //IBooking ChangeRoom(int roomNumber);
    }
}
