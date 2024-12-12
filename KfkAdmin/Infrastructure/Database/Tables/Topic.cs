using System.ComponentModel.DataAnnotations;

namespace KfkAdmin.Infrastructure.Database.Tables;

public class Topic
{
    [Key]
    public string Name { get; set; }
    public string Scheme { get; set; }
}