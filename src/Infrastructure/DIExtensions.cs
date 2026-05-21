using Application.Abstractions;
using Application.Interfaces.Repositories.User;
using Core.Application.Interface;
using Core.Application.Interface.Repository;
using Core.Application.Interface.Repository.Sales;
using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Repository.SEIH.Hospital;
using Core.Application.Interface.Security;
using Core.Application.Interface.Services.Emails;
using Core.Application.Interface.Services.Sales;
using Core.Application.Interface.Services.SEIH;
using Core.Application.Interface.Services.SEIH.Hospital;
using Core.Application.Interface.Services.SEIH.Transfer;
using Core.Application.Interface.Services.SEIH.User;
using Core.Application.Interface.Token;
using Infrastructure.Repositories.User;
using Infrastructure.Repository;
using Infrastructure.Repository.Product;
using Infrastructure.Repository.Sales;
using Infrastructure.Repository.SEIH.Hospital;
using Infrastructure.Repository.SEIH.User;
using Infrastructure.Security;
using Infrastructure.Security.Permission;
using Infrastructure.Services;
using Infrastructure.Services.Emails;
using Infrastructure.Services.Products;
using Infrastructure.Services.Sales;
using Infrastructure.Services.SEIH;
using Infrastructure.Services.SEIH.Hospital;
using Infrastructure.Services.SEIH.Transfer;
using Infrastructure.Services.SEIH.User;
using Infrastructure.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Infrastructure
{
    public static class DIExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            MainServicesRegistrations.RegisterMainServices(services, config);

            //Repositories
            services.AddScoped<IUserManager, UserManagerWrapper>();
            services.AddScoped<ISignInManager, SignInManagerWrapper>();
            services.AddScoped<IRoleManager, RoleManagerWrapper>();

            //DbContext



            //Token
            services.AddScoped<ITokenServices, TokenServices>();

            //Email
            //services.AddScoped<IEmailSender, EmailSender>();


            // Register Repository and Service using interfaces
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductUpdateService, ProductUpdateService>();
            services.AddScoped<ICreateSalesService, CreateSalesService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<IGetSalesService, GetSalesServices>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IGetSellerDailySalesResumeService, GetSellerDailySalesResumeService>();




            //Security
            services.AddScoped<IHashingServices, HashingServices>();


            //NEW
            services.AddScoped<IUserPermissionService, UserPermissionService>();
            services.AddScoped<IPermissionExclusionService, PermissionExclusionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersSignIn, SignInUsers>();
            services.AddScoped<IRolesRepository, RoleRepository>();
            services.AddScoped<IHospitalRepository, HospitalRepository>();
            services.AddScoped<IHospitalUserService, HospitalUserService>();
            services.AddScoped<IHospitalRoleService, HospitalRoleService>();
            services.AddScoped<IHospitalRoleRepository, HospitalRoleRepository>();
            services.AddScoped<IHospitalPermissionService, HospitalPermissionService>();
            services.AddScoped<IHospitalPermissionRepository, HospitalPermissionRepository>();

            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<ICryptoService, CryptoService>();
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IHospitalRepository, HospitalRepository>();
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<ITransferValidationService, TransferValidationService>();
            services.AddScoped<ITransferInstitutionKeyRepository, TransferInstitutionKeyRepository>();
            services.AddScoped<ITransferDeliveryService, TransferDeliveryService>();

            return services;
        }
    }
}
