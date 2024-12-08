using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.Home.Components;

public partial class ConsumerGroupTable(IKafkaRepositoryProvider repositoryProvider) : ComponentBase
{
    private List<ConsumerGroup> _consumerGroups { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _consumerGroups = await repositoryProvider.ConsumerGroupRepository.GetAllAsync();
    }
}