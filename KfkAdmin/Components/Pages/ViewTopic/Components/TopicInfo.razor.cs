using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.ViewTopic.Components;

public partial class TopicInfo : ComponentBase
{
    [Inject] private IKafkaRepositoryProvider _repositoryProvider { get; set; }
    
    [Parameter] public Topic Topic { get; set; }
    
    private List<Broker> _brokers = new();
    private string _brokerHosts = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _brokers = await _repositoryProvider.BrokerRepository.GetByIdsAsync(Topic.BrokerIds);
        _brokerHosts = string.Join(',', _brokers.Select(x => $"{x.Host}:{x.Port}").ToList());
    }
}