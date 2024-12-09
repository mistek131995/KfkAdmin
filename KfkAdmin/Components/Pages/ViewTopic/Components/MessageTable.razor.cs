using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.ViewTopic.Components;

public partial class MessageTable(IKafkaRepositoryProvider repositoryProvider) : ComponentBase
{
    [Parameter] public string TopicName { get; set; }
    private List<Message> messages = new();

    private LoadingMessageState showMessageState = LoadingMessageState.Hide;

    protected override async Task OnInitializedAsync()
    {
        await Task.Yield();
    }
    
    

    private async Task ShowMessageAsync()
    {
        showMessageState = LoadingMessageState.Loading;
        messages = await repositoryProvider.MessageRepository.GetByTopicNameAsync(TopicName);
        showMessageState = LoadingMessageState.Ready;
    }
    
    private enum LoadingMessageState
    {
        Hide,
        Loading,
        Ready
    }
}