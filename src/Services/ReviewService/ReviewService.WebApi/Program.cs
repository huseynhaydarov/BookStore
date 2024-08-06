using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReviewService.Application.Services;
using ReviewService.Domain.Repositories;
using ReviewService.Infrastructure.Consumers;
using ReviewService.Infrastructure.Persistence.Contexts;
using ReviewService.Infrastructure.Repositories;
using ReviewService.Infrastructure.Services;
using ReviewService.WebApi.Middlewares;
using Serilog;
using System.Text.Json.Serialization;

namespace ReviewService.WebApi;

public class Program
{
    public static string AppKey => "Test";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // ��������� Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog(); // ��������� Serilog � ����

        // ���������� �������� � ���������
        builder.Services.AddDbContext<ReviewDbContext>(con => con.UseSqlServer(builder.Configuration["ConnectionString"])
            .LogTo(Console.Write, LogLevel.Error)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        #region AddMassTransit
        builder.Services.AddMassTransit(x =>
        {
            x.AddConsumer<BookCreatedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://localhost", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint("book-created-event-queue", e =>
                {
                    e.ConfigureConsumer<BookCreatedConsumer>(context);
                });
            });
        });
        #endregion

        builder.Services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.AddScoped<IReviewService, ReviewServices>();
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });

        try
        {
            Log.Information("Starting web host");
            var app = builder.Build();

            #region DataConfigurations
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ReviewDbContext>();
#if DEBUG
                if (builder.Environment.IsEnvironment("Test"))
                {
                    context.Database.EnsureCreated();
                }
                else
                {
#endif
                    context.Database.Migrate();
#if DEBUG
                }
#endif
            }
            #endregion

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseMiddleware<RateLimitingMiddleware>();
            app.UseMiddleware<ApplicationKeyMiddleware>(AppKey);
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
