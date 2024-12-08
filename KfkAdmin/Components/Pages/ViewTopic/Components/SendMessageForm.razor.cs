using System.ComponentModel.DataAnnotations;
using KfkAdmin.Components.Utils.Modal;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.ViewTopic.Components;

public partial class SendMessageForm : ComponentBase
{
    [Parameter] public string TopicName { get; set; }
    
    private Modal? ModalInstance;
    
    private SendMessageFormViewModel model = new ();

    private async Task SendMessageFormHandler()
    {
        await Task.CompletedTask;
    }
    
    
    private void OpenModal()
    {
        ModalInstance?.Show();
    }

    private Task SaveChanges()
    {
        Console.WriteLine("Изменения сохранены.");
        return Task.CompletedTask;
    }

    private Task CancelChanges()
    {
        Console.WriteLine("Изменения отменены.");
        return Task.CompletedTask;
    }

    private class SendMessageFormViewModel
    {
        public string Key { get; set; }
        [Required(ErrorMessage = "Сообщение обязательно к заполнению")]
        public string Value { get; set; }
        public string Headers { get; set; }
    }
}