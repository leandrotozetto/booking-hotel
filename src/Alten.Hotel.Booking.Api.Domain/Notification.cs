using Alten.Hotel.Booking.Api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Alten.Hotel.Booking.Api.Domain
{
    /// <summary>
    /// Notification entity
    /// </summary>
    /// <seealso cref="INotification" />
    public class Notification : INotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        public Notification()
        {
            _errors = new Collection<string>();
        }

        /// <summary>
        /// The errors
        /// </summary>
        private readonly ICollection<string> _errors;

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public IEnumerable<string> Errors { get { return _errors; } }

        /// <summary>
        /// Gets a value indicating whether this instance has errors.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has errors; otherwise, <c>false</c>.
        /// </value>
        public bool HasErrors { get { return _errors.Count > 0; } }

        /// <summary>
        /// Gets a value indicating whether this instance has critical error.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has critical error; otherwise, <c>false</c>.
        /// </value>
        public bool HasCriticalError { get; private set; }

        /// <summary>
        /// Adds the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public void AddNotification(string notification)
        {
            if (!_errors.Contains(notification))
            {
                _errors.Add(notification);
            }
        }

        /// <summary>
        /// Adds the specified notifications.
        /// </summary>
        /// <param name="notifications">The notifications.</param>
        public void AddNotification(IEnumerable<string> notifications)
        {
            foreach (var item in notifications)
            {
                if (!_errors.Contains(item))
                {
                    _errors.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds the critical.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public void AddCritical(string notification)
        {
            HasCriticalError = true;

            if (!_errors.Contains(notification))
            {
                _errors.Add(notification);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
