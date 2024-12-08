namespace KfkAdmin.Models.Entities;

public class Topic
{
    public string Name { get; set; }
    public List<int> BrokerIds { get; set; }
    public int PartitionCount { get; set; }
    public short ReplicationFactor { get; set; }
}