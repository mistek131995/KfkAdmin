using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.ViewTopic.Components;

public partial class PartitionTable : ComponentBase
{
    [Inject] private IKafkaRepositoryProvider _repositoryProvider { get; set; }
    
    [Parameter] public string TopicName { get; set; }
    
    private List<Partition>? _partitions;
    private PartitionFilterModel filterModel = new PartitionFilterModel() { OnlyPartitionsWithMessage = true, Count = 10 };

    protected override async Task OnInitializedAsync() => await LoadPartitionsAsync();

    private async Task LoadPartitionsAsync()
    {
        _partitions = null;
        
        var partitions = await _repositoryProvider.PartitionRepository.GetByTopicNameAsync(TopicName);

        if (filterModel.OnlyPartitionsWithMessage)
        {
            partitions = partitions.Where(x => x.MaxOffset - x.MinOffset > 0).ToList();
        }

        //Если -1, отображаем все партиции и условие не должно выполняться
        if (filterModel.Count < 0)
        {
            partitions = partitions.Take(filterModel.Count).ToList();
        }
        
        _partitions = partitions;
    }

    private class PartitionFilterModel
    {
        public bool OnlyPartitionsWithMessage { get; set; }
        public int Count { get; set; }
    }
}