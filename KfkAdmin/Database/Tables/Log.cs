using System.ComponentModel.DataAnnotations;

namespace KfkAdmin.Database.Tables;

public class Log
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public string Source { get; set; }
}