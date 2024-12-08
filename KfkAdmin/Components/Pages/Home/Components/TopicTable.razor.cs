using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.Home.Components;

public partial class TopicTable(IKafkaRepositoryProvider repositoryProvider) : ComponentBase
{
    private List<Topic> topics;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        topics = await repositoryProvider.TopicRepository.GetAllAsync();
    }
    
    private async Task DeleteTopicHandlerAsync(string name)
    {
        await repositoryProvider.TopicRepository.DeleteAsync(name);
        
        topics = await repositoryProvider.TopicRepository.GetAllAsync();
    }
}