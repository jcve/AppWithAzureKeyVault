namespace AppWithAzureKeyVault.Domain.Models
{
    public class KeyVaultConfig
    {
        public string Url { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecretId { get; set; }
    }
}
