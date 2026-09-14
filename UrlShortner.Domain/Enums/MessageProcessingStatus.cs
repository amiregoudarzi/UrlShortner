namespace UrlShortner.Domain.Enums;

public enum MessageProcessingStatus
{
    Pending = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4,
    DeadLettered = 5
}