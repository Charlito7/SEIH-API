using AutoMapper;
using Core.Domain.Entities;
using Infrastructure.Constants;
using Infrastructure.Mapper.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING")!;


        services.AddDbContext<AppDbContext>(options =>
             options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 28))));

        var serverVersion = ServerVersion.AutoDetect(connectionString);
        services.AddDbContext<AppDbContext>(options =>
        {
            options
                .UseMySql(connectionString, serverVersion, b =>
                    b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
        });
        /* services.AddDbContext<AppDbContext>(options =>
         {
             options.UseSqlServer(
                 connectionString,
                 b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
         });
         */

        services.AddCors(options =>
        {
            var originsRaw = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS") ?? "";
            var allowedOrigins = originsRaw
                .Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(o => o.Trim().TrimEnd('/'))
                .ToArray();
            options.AddPolicy("GeneralPolicy",
                policy =>
                {
                    policy.WithOrigins(allowedOrigins) // Specify the allowed origin
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials(); // Allow credentials
                });
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdminRole", policy =>
                policy.RequireClaim("Role", "User"));
        });

        /*services.AddCors(options =>
        {
            options.AddPolicy("SpecificOrigin",
              policy =>
              {
                  policy.WithOrigins("http://localhost:5254")
          .AllowAnyHeader()
          .AllowAnyMethod();
              });
        });*/
        services.AddMvc().AddSessionStateTempDataProvider();

        var mapperConfig = new MapperConfiguration(mc =>
        {
           mc.AddProfile(new MappingProfile());
        });

        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromSeconds(60);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddHttpClient();

        services.AddIdentity<UserEntity, UserRoleEntity>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 10;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequiredUniqueChars = 2;
        })
            .AddEntityFrameworkStores<AppDbContext>();


        // Replace this line:
        // DIExtensions.AddServices(services);

        // With this line:
        DIExtensions.AddServices(services, configuration);
       // DIExtensions.AddServices(services);

        services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddJwtBearer(

        options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false; //TODO: Change to true in Production
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Cherche le cookie "SessionId"
                    var token = context.Request.Cookies["SessionId"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                }
            };

            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                NameClaimType = "name",
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Environment.GetEnvironmentVariable(EnvFileConstants.ISSUER),
                ValidAudience = Environment.GetEnvironmentVariable(EnvFileConstants.AUDIENCE),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(EnvFileConstants.ACCESS_TOKEN_SECRET)!)),
                ClockSkew = TimeSpan.Zero

            };
        }
    );
        services.AddSwaggerGen(setup =>
        {
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {jwtSecurityScheme, Array.Empty<string>() }
                });
        });
        return services;
    }
}
