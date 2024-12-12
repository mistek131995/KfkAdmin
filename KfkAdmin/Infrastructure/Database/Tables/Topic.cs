using System.ComponentModel.DataAnnotations;

namespace KfkAdmin.Infrastructure.Database.Tables;

public class TopicScheme
{
    [Key]
    public string Name { get; set; }
    public string Scheme { get; set; }
}