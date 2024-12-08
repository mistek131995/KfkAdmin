namespace KfkAdmin.Interfaces.Services;

public interface ITopicService
{
    Task TransferDataAsync(string fromName, string toName);
    Task RenameAsync(string oldName, string newName);
}