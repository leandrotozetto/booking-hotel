using Alten.Hotel.Booking.Api.Application;
using Alten.Hotel.Booking.Api.Application.Mappers;
using Alten.Hotel.Booking.Api.Domain;
using Alten.Hotel.Booking.Api.Domain.Interfaces;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Applications;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Repositories;
using Alten.Hotel.Booking.Api.Domain.Interfaces.Services;
using Alten.Hotel.Booking.Api.Domain.Services;
using Alten.Hotel.Booking.Api.Infrastructure;
using Alten.Hotel.Booking.Api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Alten.Hotel.Booking.Api.Interface
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BookingDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Alten.Hotel.Booking.Api.Infrastructure")));

            services.AddTransient<IDbConnection>(x => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new Converters.DateTimeConverter());
            });

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel Booking Api", Version = "v1" });
                c.IncludeXmlComments(xmlPath);
                c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
            });

            services.AddSwaggerGen(x =>
                {
                    // Set the comments path for the Swagger JSON and UI.
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    x.IncludeXmlComments(xmlPath);
                });

            services.AddTransient<IRepository<Domain.Aggregates.BookingAggregate.Booking>, Repository<Domain.Aggregates.BookingAggregate.Booking>>();
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<ICalendarRepository, CalendarRepository>();
            services.AddTransient<IBookingApplication, BookingApplication>();
            services.AddTransient<ICalendarApplication, CalendarApplication>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddScoped<INotification, Notification>();

            services.AddAutoMapper(x =>
            {
                x.AddProfile(new BookingProfile());
                x.AddProfile(new AvailableDateProfile());
            });
        }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="env">The env.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alten.Hotel.Booking.Api.Interface v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
}
