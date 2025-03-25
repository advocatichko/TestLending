using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Добавь здесь таблицы, с которыми работаешь
    public DbSet<Personnel> Personnels { get; set; }
}

// Пример таблицы Personnel
public class Personnel
{
    public int Id { get; set; }
    public string Name { get; set; }
}
