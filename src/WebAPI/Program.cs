using Core.Application.Interface.Repository.SEIH;
using Core.Application.Interface.Security;
using Core.Application.Interface.Services.SEIH.Transfer;
using DotNetEnv;
using Infrastructure;
using Infrastructure.Repository.SEIH.Hospital;
using Infrastructure.Repository.SEIH.Transfer;
using Infrastructure.Security;
using Infrastructure.Services.Security;
using Infrastructure.Services.SEIH.Transfer;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration["Kestrel:Endpoints:Https:Certificate:Password"] =
    Environment.GetEnvironmentVariable("CERTIFICATE_PASSWORD");

builder.Services.AddControllers();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IInstitutionKeyService, InstitutionKeyService>();
builder.Services.AddScoped<IInstitutionKeyRepository, InstitutionKeyRepository>();
builder.Services.AddScoped<ITransferRequestNetworkService, TransferRequestNetworkService>();
builder.Services.AddScoped<ITransferRequestRepository, TransferRequestRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromHours(3));

builder.Logging.AddConsole();

var app = builder.Build();

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("GeneralPolicy");
app.UseCookiePolicy();
app.UseSession();

app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api/rest"),
    restApp =>
    {
        restApp.UseMiddleware<IpWhitelistMiddleware>();
        restApp.UseMiddleware<ApiKeyMiddleware>();

        restApp.UseWhen(
            context => !context.Request.Path.StartsWithSegments("/api/rest/seih/hospital/network"),
            inner =>
            {
                inner.UseMiddleware<SignatureMiddleware>();
            });
    }
);

app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/api/rest"),
    uiApp =>
    {
        uiApp.UseAuthentication();
        uiApp.UseAuthorization();
    }
);

app.MapControllers();

app.Run();