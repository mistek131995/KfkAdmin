using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using KfkAdmin.Components.Utils.Modal;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.ViewTopic.Components;

public partial class SendMessageForm : ComponentBase
{
    [Parameter] public string TopicName { get; set; }

    [Inject] private IKafkaRepositoryProvider _repositoryProvider { get; set; }
    
    private Modal? modalInstance;
    
    private SendMessageFormViewModel model = new ();

    private async Task SendMessageFormHandler()
    {
        await Task.CompletedTask;
    }

    private async Task SendMessageHandlerAsync()
    {
        var headers = JsonSerializer.Deserialize<Dictionary<string, string>>(model.Headers)
            .ToDictionary(x => x.Key, x => Encoding.UTF8.GetBytes(x.Value));

        await _repositoryProvider.MessageRepository.SendMessagesAsync(new Message()
        {
            Topic = TopicName,
            Key = model.Key,
            Headers = headers,
            Payload = model.Value
        });
        
        modalInstance.Hide();
    }

    private void CancelChanges()
    {
        model = new SendMessageFormViewModel();
    }

    private class SendMessageFormViewModel
    {
        public string? Key { get; set; }
        [Required(ErrorMessage = "Сообщение обязательно к заполнению")]
        public string Value { get; set; } = string.Empty;
        public string SerializeFormat { get; set; }
        public string? Headers { get; set; }
    }
}