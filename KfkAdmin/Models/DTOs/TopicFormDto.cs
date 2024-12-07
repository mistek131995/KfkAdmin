using System.ComponentModel.DataAnnotations;

namespace KfkAdmin.Models.DTOs;

public class TopicFormDto
{
    [Required(ErrorMessage = "Имя топика обязательно к заполнению")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Кол-во партиций обязательно к заполнению")]
    [Range(1, int.MaxValue, ErrorMessage = $"Минимальное значение партиций - 1")]
    public int PartitionCount { get; set; } = 1;

    [Required(ErrorMessage = "Фактор репликации обязателен к заполнению")]
    [Range(1, short.MaxValue, ErrorMessage = $"Минимальное значение фактора репликации - 1")]
    public short ReplicationFactor { get; set; } = 1;
}