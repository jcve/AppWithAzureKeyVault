namespace AppWithAzureKeyVault.Domain.Models
{
    public class KeyVaultConfigCertificate
    {
        public string Url { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string CertificateThumbprint { get; set; }
    }
}
