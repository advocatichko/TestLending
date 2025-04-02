using Microsoft.AspNetCore.DataProtection;
using System;
using System.IO;

namespace TestLending.Utils
{
    public class ConnectionStringEncryptor
    {
        public static string EncryptConnectionString(string connectionString, string? keyDirectory = null) // Make parameter nullable with ?
        {
            keyDirectory ??= Path.Combine(AppContext.BaseDirectory, "keys");
            
            var dataProtectionProvider = DataProtectionProvider
                .Create(new DirectoryInfo(keyDirectory));
                
            var protector = dataProtectionProvider.CreateProtector("SecureConnectionString");
            return protector.Protect(connectionString);
        }
    }
}
