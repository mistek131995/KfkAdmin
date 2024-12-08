using KfkAdmin.Components.Utils.Modal;
using KfkAdmin.Models.DTOs;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.ViewTopic.Components;

public partial class SendMessageForm : ComponentBase
{
    [Parameter] public string TopicName { get; set; }
    
    private Modal? ModalInstance;
    
    private SendMessageFormDto model = new ();

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
}