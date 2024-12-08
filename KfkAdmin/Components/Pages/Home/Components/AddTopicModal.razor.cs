using System.ComponentModel.DataAnnotations;
using KfkAdmin.Components.Utils.Modal;
using KfkAdmin.Interfaces.Providers;
using KfkAdmin.Models.Entities;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.Home.Components;

public partial class AddTopicModal(IKafkaRepositoryProvider repositoryProvider) : ComponentBase
{
    [Parameter] public EventCallback OnCreateComplete { get; set; }
    
    private Modal modal;
    private AddTopicViewModal formModal = new ();
    
    private async Task CreateTopicHandlerAsync()
    {
        await repositoryProvider.TopicRepository.CreateAsync(new Topic()
        {
            Name = formModal.Name,
            PartitionCount = formModal.PartitionCount,
            ReplicationFactor = formModal.ReplicationFactor,
        });

        await OnCreateComplete.InvokeAsync();
        modal.Hide();
        formModal = new();
    }

    private class AddTopicViewModal()
    {
        [Required(ErrorMessage = "Имя топика обязательно к заполнению")]
        public string Name { get; set; } = string.Empty;
    
        [Required(ErrorMessage = "Кол-во партиций обязательно к заполнению")]
        [Range(1, int.MaxValue, ErrorMessage = $"Минимальное значение партиций - 1")]
        public int PartitionCount { get; set; } = 1;

        [Required(ErrorMessage = "Фактор репликации обязателен к заполнению")]
        [Range(1, short.MaxValue, ErrorMessage = $"Минимальное значение фактора репликации - 1")]
        public short ReplicationFactor { get; set; } = 1;
    }
}