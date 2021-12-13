using System;
using System.Collections.Generic;

namespace Alten.Hotel.Booking.Api.Domain.Interfaces
{
    /// <summary>
    /// Notification's contract
    /// </summary>
    public interface INotification : IDisposable
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        IEnumerable<string> Errors { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        bool HasErrors { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has critical error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has critical error; otherwise, <c>false</c>.
        /// </value>
        bool HasCriticalError { get; }

        /// <summary>
        /// Adds the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        void AddNotification(string notification);

        /// <summary>
        /// Adds the specified notifications.
        /// </summary>
        /// <param name="notifications">The notifications.</param>
        void AddNotification(IEnumerable<string> notifications);

        /// <summary>
        /// Adds the critical.
        /// </summary>
        /// <param name="notification">The notification.</param>
        void AddCritical(string notification);
    }
}
