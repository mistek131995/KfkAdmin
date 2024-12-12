using KfkAdmin.Infrastructure.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace KfkAdmin.Infrastructure.Database;

public class SqLiteContext(DbContextOptions<SqLiteContext> options) : DbContext(options)
{
    public DbSet<Log> Logs { get; set; }
    public DbSet<Topic> Topics { get; set; }
}