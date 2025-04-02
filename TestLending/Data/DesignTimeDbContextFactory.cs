using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace TestLending.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Устанавливаем переменную среды для индикации, что код выполняется в контексте миграции
            Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_MIGRATIONS", "true");
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Используем прямую строку подключения для EF миграций
            var connectionString = "SERVER=176.37.171.239,1434;DATABASE=TestDB;Persist Security Info=True;User ID=sqlserver;Password=Recon12qw!;TrustServerCertificate=True;Connect Timeout=30;";

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
