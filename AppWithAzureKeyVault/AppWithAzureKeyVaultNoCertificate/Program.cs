using AppWithAzureKeyVault.Domain.Models;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Get appsettings.json

var KeyVaultConfig = builder.Configuration.GetSection("KeyVaultConfig").Get<KeyVaultConfig>();

if (KeyVaultConfig == null)
    throw new Exception("KeyVaultConfig not found");

// Setup with Azure KeyVault

var credential = new ClientSecretCredential(KeyVaultConfig.TenantId, KeyVaultConfig.ClientId, KeyVaultConfig.ClientSecretId);

var client = new SecretClient(new Uri(KeyVaultConfig.Url), credential);
builder.Configuration.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions());

// End Setup with Azure KeyVault 

// Test Get

string VaultConnectionStringDbAlternative = builder.Configuration["Database"];

var ValueA = builder.Configuration.GetSection("Multiple:a");
var ValueB = builder.Configuration.GetSection("Multiple:b");


var VaultConnectionStringDb = client.GetSecret("Database", null);
string ConnectionStringDb = VaultConnectionStringDb.Value.Value;

var VaultEncryptKey = client.GetSecret("EncryptKey", null);
var EncryptKey = VaultEncryptKey.Value.Value;

// End Test Get

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
