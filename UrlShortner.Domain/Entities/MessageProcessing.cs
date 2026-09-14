using UrlShortner.Domain.Enums;

namespace UrlShortner.Domain.Entities;

public class MessageProcessing : Entity
{

    public MessageProcessing() { }

    public string MessageId { get; private set; } = string.Empty;

    public string MessageType { get; private set; } = string.Empty;

    public string QueueName { get; private set; } = string.Empty;

    public string? RoutingKey { get; private set; }

    public string Payload { get; private set; } = string.Empty;

    public MessageProcessingStatus Status { get; private set; }

    public int AttemptCount { get; private set; }

    public string? ErrorMessage { get; private set; }
    
    public DateTime? ProcessedAtUtc { get; private set; }
}