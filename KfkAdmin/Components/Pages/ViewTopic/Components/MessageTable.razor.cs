using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.ViewTopic.Components;

public partial class MessageTable(IKafkaRepositoryProvider repositoryProvider) : ComponentBase
{
    [Parameter] public string TopicName { get; set; }
    [Parameter] public long MessageCount { get; set; }
    
    private List<Message> messages = new();

    private LoadingMessageState showMessageState = LoadingMessageState.Hide;

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