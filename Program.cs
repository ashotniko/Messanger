using Messanger.Data;
using Messanger.Interfaces;
using Messanger.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Messanger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

                builder.Host.UseSerilog();

                // Add services to the container.
                builder.Services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    });
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
                //builder.Services.AddSignalR();

                builder.Services.AddScoped<IMessageService, MessageService>();
                builder.Services.AddScoped<IUserService, UserService>();
                //builder.Services.AddScoped<MessageDeliveryService>();

                builder.Services.AddDbContext<MessengerDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseSerilogRequestLogging(); //This logs HTTP method, path, response time, and status code. 

                //app.UseAuthorization();

                //app.MapHub<ChatHub>("/chatHub");  
                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
