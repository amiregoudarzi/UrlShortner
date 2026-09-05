namespace UrlShortner.Domain.Entities;

public abstract class Entity
{
    protected Entity()
    {
    }

    protected Entity(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public DateTime CreatedDateUtc { get; private set; } = DateTime.UtcNow;
}
