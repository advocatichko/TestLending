using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

namespace TestLending.Data
{
    public class ConnectionStringProvider
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IConfiguration _configuration;
        private readonly string _purpose = "SecureConnectionString";

        public ConnectionStringProvider(IDataProtectionProvider dataProtectionProvider, IConfiguration configuration)
        {
            _dataProtectionProvider = dataProtectionProvider;
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            var encryptedConnectionString = _configuration["ConnectionStrings:EncryptedSqlConnection"];

            if (string.IsNullOrEmpty(encryptedConnectionString))
            {
                throw new InvalidOperationException("Encrypted connection string not found in configuration");
            }

            var protector = _dataProtectionProvider.CreateProtector(_purpose);
            return protector.Unprotect(encryptedConnectionString);
        }

        public string EncryptConnectionString(string connectionString)
        {
            var protector = _dataProtectionProvider.CreateProtector(_purpose);
            return protector.Protect(connectionString);
        }
    }
}
