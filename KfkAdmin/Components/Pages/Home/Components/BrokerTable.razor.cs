using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.Home.Components;

public partial class BrokerTable(IKafkaRepositoryProvider repositoryProvider) : ComponentBase
{
    private List<BrokerTableViewModel> viewModel;

    protected override async Task OnInitializedAsync()
    {
        var brokers = await repositoryProvider.BrokerRepository.GetAllAsync();
        var partitions = await repositoryProvider.PartitionRepository.GetAllAsync();

        viewModel = brokers.Select(x => new BrokerTableViewModel()
        {
            BrokerId = x.BrokerId,
            Host = x.Host,
            Port = x.Port,
            PartitionCount = partitions.Count(p => p.BrokerId == x.BrokerId),
            PartitionPercent = (double)partitions.Count(p => p.BrokerId == x.BrokerId) / partitions.Count * 100
        }).ToList();
    }
    
    private class BrokerTableViewModel : Broker
    {
        public int PartitionCount { get; set; }
        public double PartitionPercent { get; set; }
    }
}