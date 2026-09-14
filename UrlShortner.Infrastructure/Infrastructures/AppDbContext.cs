using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortner.Domain.Entities;

namespace UrlShortner.Infrastructure.Infrastructures;

public class AppDbContext : DbContext
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        IncludeFields = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<Password> Passwords { get; set; }
    
    public DbSet<ShortUrl> ShortUrls { get; set; }
    
    public DbSet<UrlClick> UrlClicks { get; set; }
    
    public DbSet<MessageProcessing> MessageProcessings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new PasswordEntityTypeConfiguration());
        builder.ApplyConfiguration(new ShortUrlEntityTypeConfiguration());
        builder.ApplyConfiguration(new UrlClickEntityTypeConfiguration());
        builder.ApplyConfiguration(new MessageProcessingEntityTypeConfiguration());
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
    
    private class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            
            builder.HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");
            
            builder.Property(model => model.FirstName)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("first_name");
            
            builder.Property(model => model.LastName)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("last_name");
            
            builder.Property(model => model.Role)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasConversion<string>()
                .HasColumnName("role");
            
            builder.Property(model => model.NationalCode)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("national_code")
                .IsRequired(false);
            
            builder.Property(model => model.PhoneNumber)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("phone_number")
                .IsRequired(false);
            
            builder.Property(model => model.Username)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("user_name")
                .IsRequired(false);
        
            
            builder.HasOne(user => user.Password)
                .WithOne(password => password.User)
                .HasForeignKey<Password>(password => password.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("created_date_utc");
        }
    }
    
    private class PasswordEntityTypeConfiguration : IEntityTypeConfiguration<Password>
    {
        public void Configure(EntityTypeBuilder<Password> builder)
        {
            builder.ToTable("passwords");

            builder.HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");
                
            builder.Property(model => model.PasswordHash)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("password_hash");
            
            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("created_date_utc");
        }
    }    
    
    private class ShortUrlEntityTypeConfiguration : IEntityTypeConfiguration<ShortUrl>
    {
        public void Configure(EntityTypeBuilder<ShortUrl> builder)
        {
            builder.ToTable("short_urls");

            builder.HasKey(model => model.Id);
            builder.HasIndex(x => x.ShortCode).IsUnique();
            
            builder.Property(model => model.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("id");
                
            builder.Property(model => model.ShortCode)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("short_code");
            
            builder.Property(model => model.OriginalUrl)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("original_url");
                
            builder.Property(model => model.CreatedDateUtc)
                .UsePropertyAccessMode(PropertyAccessMode.Property)
                .HasColumnName("created_date_utc");
        }
    }
    
    private class UrlClickEntityTypeConfiguration : IEntityTypeConfiguration<UrlClick>
    {
        public void Configure(EntityTypeBuilder<UrlClick> builder)
        {
            builder.ToTable("url_clicks");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.ShortUrlId)
                .HasColumnName("short_url_id")
                .IsRequired();

            builder.Property(x => x.UserAgent)
                .HasColumnName("user_agent");

            builder.Property(x => x.Referrer)
                .HasColumnName("referrer");

            builder.Property(x => x.CreatedDateUtc)
                .HasColumnName("created_date_utc");
        }
    }
    private class MessageProcessingEntityTypeConfiguration : IEntityTypeConfiguration<MessageProcessing>
    {
        public void Configure(EntityTypeBuilder<MessageProcessing> builder)
        {
            builder.ToTable("message_processings");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.MessageId)
                .HasColumnName("message_id")
                .IsRequired();

            builder.Property(x => x.MessageType)
                .HasColumnName("message_type")
                .IsRequired();

            builder.Property(x => x.QueueName)
                .HasColumnName("queue_name")
                .IsRequired();

            builder.Property(x => x.RoutingKey)
                .HasColumnName("routing_key");

            builder.Property(x => x.Payload)
                .HasColumnName("payload")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.AttemptCount)
                .HasColumnName("attempt_count")
                .IsRequired();

            builder.Property(x => x.ErrorMessage)
                .HasColumnName("error_message");

            builder.Property(x => x.ProcessedAtUtc)
                .HasColumnName("processed_at_utc");

            builder.Property(x => x.CreatedDateUtc)
                .HasColumnName("created_date_utc");
        }
    }
}
