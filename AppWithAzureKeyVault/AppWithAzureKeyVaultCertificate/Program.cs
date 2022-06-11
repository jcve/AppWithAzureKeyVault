using Azure.Identity;
using AppWithAzureKeyVault.Domain.Models;
using System.Security.Cryptography.X509Certificates;
using Azure.Extensions.AspNetCore.Configuration.Secrets;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Get appsettings.json

var KeyVaultConfig = builder.Configuration.GetSection("KeyVaultConfig").Get<KeyVaultConfigCertificate>();

if (KeyVaultConfig == null)
    throw new Exception("KeyVaultConfig not found");

//if (builder.Environment.IsProduction())
//{
//}

// Setup certificate

using var x509Store = new X509Store(StoreLocation.CurrentUser);

x509Store.Open(OpenFlags.ReadOnly);

var x509Certificate = x509Store.Certificates
    .Find(
        X509FindType.FindByThumbprint,
        KeyVaultConfig.CertificateThumbprint,
        validOnly: false)
    .OfType<X509Certificate2>()
    .Single();

// End Setup certificate

// Setup with Azure KeyVault

builder.Configuration.AddAzureKeyVault(
    new Uri(KeyVaultConfig.Url),
    new ClientCertificateCredential(
        KeyVaultConfig.TenantId,
        KeyVaultConfig.ClientId,
        x509Certificate)
    );

// End Setup with Azure KeyVault 

// Test GetSecret()

string ConnectionStringDb = builder.Configuration["Database"];

var EncryptKey = builder.Configuration["EncryptKey"];

// End Test GetSecret()

builder.Services.AddSingleton(new AppConfig
{
    EncryptKey = EncryptKey
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
