namespace KfkAdmin.Models;

public class Topic
{
    public string Name { get; set; }
    public int PartitionCount { get; set; }
    public short ReplicationFactor { get; set; }
    public bool IsInternal  { get; set; }
    
    public List<Partition> Partitions { get; set; }
}