namespace UrlShortner.Domain.Entities;

public class User : Entity
{
    public User() { }

    public required string KeycloakUserId { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? NationalCode { get; set; }

    public Role? Role { get; set; }
}