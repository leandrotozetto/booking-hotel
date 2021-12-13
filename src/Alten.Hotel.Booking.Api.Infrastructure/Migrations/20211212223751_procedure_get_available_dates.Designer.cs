﻿// <auto-generated />
using System;
using Alten.Hotel.Booking.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Alten.Hotel.Booking.Api.Infrastructure.Migrations
{
    [DbContext(typeof(BookingDbContext))]
    [Migration("20211212223751_procedure_get_available_dates")]
    partial class procedure_get_available_dates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Alten.Hotel.Booking.Api.Domain.Aggregates.BookingAggregate.Booking", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CheckIn")
                        .HasColumnType("date");

                    b.Property<DateTime>("CheckOut")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("CheckIn", "CheckOut")
                        .IsUnique();

                    b.ToTable("Booking");
                });
#pragma warning restore 612, 618
        }
    }
}
