using System.ComponentModel.DataAnnotations;

namespace KfkAdmin.Models.DTOs;

public class SendMessageFormDto
{
    public string Key { get; set; }
    [Required(ErrorMessage = "Сообщение обязательно к заполнению")]
    public string Value { get; set; }
    public string Headers { get; set; }
}