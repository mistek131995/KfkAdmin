using KfkAdmin.Components.Utils.Modal;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Interfaces.Services;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace KfkAdmin.Components.Pages.Home.Components;

public partial class ChangePartitionModal : ComponentBase
{
    [Parameter] public string TopicName { get; set; }
    [Parameter] public EventCallback OnChangeSuccess { get; set; }
    
    [Inject] private IKafkaRepositoryProvider repositoryProvider { get; set; }
    [Inject] private ITopicService _topicService { get; set; }
    
    private Modal modal;
    private EditContext editContext;
    private ChangePartitionViewModel modalForm = new();
    private int partitionCount = 0;

    protected override void OnInitialized()
    {
        partitionCount = repositoryProvider.TopicRepository.GetByName(TopicName)?.PartitionCount ?? 0;
    }

    public async Task HandleValidSubmit()
    {
        await _topicService.ChangePartitionCountAsync(TopicName, modalForm.NewCount);
        modal.Hide();
        await OnChangeSuccess.InvokeAsync();
    }

    private class ChangePartitionViewModel
    {
        public int NewCount { get; set; }
    }
}