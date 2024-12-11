using KfkAdmin.Database.Tables;
using Microsoft.EntityFrameworkCore;

namespace KfkAdmin.Database;

public class SqLiteContext(DbContextOptions<SqLiteContext> options) : DbContext(options)
{
    public DbSet<Log> Logs { get; set; }
}