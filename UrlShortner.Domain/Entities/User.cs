namespace UrlShortner.Domain.Entities;

public class User : Entity
{
    public User() { }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Role Role { get; set; }
    public string? PhoneNumber { get; set; }
    public string? NationalCode { get; init; }
    public string? Username { get; set; }
    public Password Password { get; private set; } = null!;
}
