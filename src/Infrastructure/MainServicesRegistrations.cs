using Application.Abstractions;
using Core.Application.Interfaces.Services.User;
using Infrastructure.Services.SEIH;
using Infrastructure.Services.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class MainServicesRegistrations
    {
        public static IServiceCollection RegisterMainServices(this IServiceCollection services, IConfiguration config)
        {

            //User
            services.AddScoped<ICreateUserService, CreateUserService>();
            services.AddScoped<ISignInUserService, SignInUserService>();
            services.AddScoped<IAdminAssignRoles, AdminAssignRoles>();    
            services.AddScoped<IAdminRemoveRoles, AdminRemoveRoles>();
            services.AddScoped<IDoesUserBelongToRole, DoesUserBelongToRole>();  
            services.AddScoped<ISignInUser, SignInUsers>();
            services.AddScoped<IUpdateUserPassword, UpdateUserPassword>();
            services.AddScoped<IGetUsersProfileData, GetUserProfileData>();

            services.Configure<SeihApiOptions>(config.GetSection("SeihApi"));

            services.AddHttpClient<ISeihTransferClient, HttpSeihTransferClient>((sp, http) =>
            {
                var opts = sp.GetRequiredService<IOptions<SeihApiOptions>>().Value;
                http.BaseAddress = new Uri(opts.BaseUrl);
                if (!string.IsNullOrWhiteSpace(opts.BearerToken))
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", opts.BearerToken);
            });
            // Consumables


            //AWS/S3


            //Issue Items


            //EndPoints



            return services;
        }
    }
}
