using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.ViewTopic.Components;

public partial class MessageTable(IKafkaRepositoryProvider repositoryProvider) : ComponentBase
{
    [Parameter] public string TopicName { get; set; }
    private List<Message> messages = new();


    protected override async Task OnInitializedAsync()
    {
        await GetMessagesAsync();
    }

    private async Task GetMessagesAsync()
    {
        messages = await repositoryProvider.MessageRepository.GetByTopicNameAsync(TopicName);
    }
}