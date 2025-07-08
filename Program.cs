using Messanger.Data;
using Messanger.Helpers;
using Messanger.Interfaces;
using Messanger.Models;
using Messanger.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Messanger
{
    public class Program
    {
        public static async Task Main(string[] args)
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
                builder.Services.AddSwaggerGen(option =>
                {
                    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    option.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type=ReferenceType.SecurityScheme,
                                    Id="Bearer"
                                }
                            },
                            new string[]{}
                        }
                    });
                });
                //builder.Services.AddSignalR();

                builder.Services.AddScoped<IMessageService, MessageService>();
                builder.Services.AddScoped<IUserService, UserService>();
                builder.Services.AddScoped<ITokenService, TokenService>();
                builder.Services.AddScoped<IMessageHelper, MessageHelper>();
                //builder.Services.AddScoped<MessageDeliveryService>();

                builder.Services.AddDbContext<MessengerDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
                {
                    options.User.RequireUniqueEmail = true; // Ensure unique email addresses
                    options.Password.RequireDigit = false; // Disable digit requirement for simplicity
                    options.Password.RequireLowercase = false; // Disable lowercase requirement for simplicity
                    options.Password.RequireUppercase = false; // Disable uppercase requirement for simplicity
                    options.Password.RequireNonAlphanumeric = true; // Require at least one non-alphanumeric character
                    options.Password.RequiredLength = 6; // Minimum password length
                })
                    .AddEntityFrameworkStores<MessengerDbContext>()
                    .AddDefaultTokenProviders(); // Add token providers for password reset, etc.)

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme =
                    options.DefaultChallengeScheme =
                    options.DefaultScheme =
                    options.DefaultForbidScheme =
                    options.DefaultSignInScheme =
                    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
                        )
                    };
                });

                builder.Services.AddAuthorizationBuilder()
                    .AddPolicy("AdminPolicy", policy =>
                    {
                        policy.RequireAuthenticatedUser()
                        .RequireRole("Admin");
                    })
                    .AddPolicy("UserPolicy", policy =>
                    {
                        policy.RequireAuthenticatedUser()
                        .RequireRole("Admin", "User");
                    });

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
                app.UseAuthentication();
                app.UseAuthorization();

                //app.MapHub<ChatHub>("/chatHub");  
                app.MapControllers();
                // Program.cs  (add just *before* app.Run();)

                await app.Services.SeedRolesAsync();
                await app.Services.SeedAdminAsync(builder.Configuration);

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
