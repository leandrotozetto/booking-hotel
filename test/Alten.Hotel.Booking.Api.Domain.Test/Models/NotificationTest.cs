using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Xunit;

namespace Alten.Hotel.Booking.Api.Domain.Test.Models
{
    public class NotificationTest
    {
        [Fact]
        public void Should_Create_New_Notification()
        {
            var notification = new Notification();

            Assert.NotNull(notification);
            Assert.True(notification is INotification);
        }

        [Fact]
        public void Should_Add_One_Notification()
        {
            var notification = new Notification();

            Assert.Empty(notification.Errors);

            notification.AddNotification("Notification 1");

            Assert.NotEmpty(notification.Errors);
            Assert.True(notification.HasErrors);
            Assert.False(notification.HasCriticalError);
        }

        [Fact]
        public void Should_Not_Add_Duplicated_Notification()
        {
            var notification = new Notification();

            Assert.Empty(notification.Errors);

            notification.AddNotification("Notification 1");
            notification.AddNotification("Notification 1");

            Assert.Single(notification.Errors);
            Assert.NotEmpty(notification.Errors);
            Assert.True(notification.HasErrors);
            Assert.False(notification.HasCriticalError);
        }

        [Fact]
        public void Should_Add_List_Of_Notification()
        {
            var notification = new Notification();

            Assert.Empty(notification.Errors);

            notification.AddNotification(new string[] { "Notification 1", "Notification 2" });

            Assert.NotEmpty(notification.Errors);
            Assert.True(notification.HasErrors);
            Assert.False(notification.HasCriticalError);
        }

        [Fact]
        public void Should_Add_Critical_Notification()
        {
            var notification = new Notification();

            Assert.Empty(notification.Errors);

            notification.AddCritical("Critical 1");

            Assert.NotEmpty(notification.Errors);
            Assert.True(notification.HasErrors);
            Assert.True(notification.HasCriticalError);
        }

        [Fact]
        public void Should_Not_Add__Duplicated_Critical_Notification()
        {
            var notification = new Notification();

            Assert.Empty(notification.Errors);

            notification.AddCritical("Critical 1");
            notification.AddCritical("Critical 1");

            Assert.Single(notification.Errors);
            Assert.NotEmpty(notification.Errors);
            Assert.True(notification.HasErrors);
            Assert.True(notification.HasCriticalError);
        }
    }
}
