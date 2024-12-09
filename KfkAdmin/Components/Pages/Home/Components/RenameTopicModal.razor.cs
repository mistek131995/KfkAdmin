using System.ComponentModel.DataAnnotations;
using KfkAdmin.Components.Utils.Modal;
using KfkAdmin.Interfaces.Services;
using Microsoft.AspNetCore.Components;

namespace KfkAdmin.Components.Pages.Home.Components;

public partial class RenameTopicModal : ComponentBase
{
    [Inject] private ITopicService topicService { get; set; }
    [Parameter] public string OldName { get; set; }
    [Parameter] public EventCallback OnRenameSuccess { get; set; }
    
    private Modal modal;
    private ViewModalForm modalForm = new();
    
    private class ViewModalForm
    {
        [Required(ErrorMessage = "Новое имя обязательно")]
        public string NewName { get; set; }
    }

    private async Task HandleValidSubmit()
    {
        await topicService.RenameAsync(OldName, modalForm.NewName);
        await OnRenameSuccess.InvokeAsync();
        modal.Hide();
        modalForm = new ViewModalForm();
    }
}