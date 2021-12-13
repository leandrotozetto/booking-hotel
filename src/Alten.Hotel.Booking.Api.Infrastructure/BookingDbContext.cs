using Alten.Hotel.Booking.Api.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System;

namespace Alten.Hotel.Booking.Api.Infrastructure
{
    public class BookingDbContext: DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.LogTo(Console.WriteLine);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        //    modelBuilder.ApplyConfiguration(GuestConfiguration.New());
        //    modelBuilder.ApplyConfiguration(PaymentConfiguration.New());
            modelBuilder.ApplyConfiguration(BookingConfiguration.New());
        }
    }
}
