using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.ViewTopic.Components;

public partial class PartitionTable : ComponentBase
{
    [Inject] private IKafkaRepositoryProvider _repositoryProvider { get; set; }
    
    [Parameter] public string TopicName { get; set; }
    
    private List<Partition> _partitions;

    protected override async Task OnInitializedAsync()
    {
        _partitions = await _repositoryProvider.PartitionRepository.GetByTopicNameAsync(TopicName);
    }
}