using Confluent.Kafka;

namespace KfkAdmin.Models.Entities;

public class ConsumerGroup
{
    public string GroupId { get; set; }
    public string State { get; set; }
    public string Type { get; set; }
    public bool IsSimpleConsumerGroup  { get; set; }
    
}