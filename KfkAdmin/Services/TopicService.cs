using Confluent.Kafka;
using KfkAdmin.Interfaces.Services;

namespace KfkAdmin.Services;

public class TopicService(IAdminClient adminClient, IConsumer<Ignore, string> consumer) : ITopicService
{
    public Task TransferDataAsync(string fromName, string toName)
    {
        throw new NotImplementedException();
    }

    public async Task RenameAsync(string oldName, string newName)
    {
        Console.WriteLine($"Rename to {newName}");
        await Task.CompletedTask;
    }
}