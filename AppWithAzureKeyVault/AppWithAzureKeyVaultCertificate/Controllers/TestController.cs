using AppWithAzureKeyVault.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppWithAzureKeyVaultCertificate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly AppConfig _appconfig;

        public TestController(ILogger<TestController> logger, AppConfig apiConfig)
        {
            _logger = logger;
            _appconfig = apiConfig;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_appconfig.EncryptKey);
        }
    }
}