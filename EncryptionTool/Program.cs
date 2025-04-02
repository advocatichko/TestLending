using Microsoft.AspNetCore.DataProtection;
using System;
using System.IO;

namespace EncryptionTool
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "SERVER=176.37.171.239,1434;DATABASE=TestDB;Persist Security Info=True;User ID=testJun;Password=testJun#1;TrustServerCertificate=True;Connect Timeout=30;";
            
            var keyDirectory = Path.Combine(AppContext.BaseDirectory, "keys");
            Directory.CreateDirectory(keyDirectory);
            
            var dataProtectionProvider = DataProtectionProvider
                .Create(new DirectoryInfo(keyDirectory));
                
            var protector = dataProtectionProvider.CreateProtector("SecureConnectionString");
            string encrypted = protector.Protect(connectionString);
            
            Console.WriteLine("Зашифрованная строка подключения:");
            Console.WriteLine(encrypted);
            Console.WriteLine("\nСкопируйте эту строку в appsettings.json в параметр ConnectionStrings:EncryptedSqlConnection");
        }
    }
}
