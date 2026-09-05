namespace UrlShortner.Domain.Entities;

public class Password : Entity
{
    public string PasswordHash { get; private set; } = string.Empty;

    public User User { get; private set; } = null!;

    public Guid UserId { get; private set; }
}
