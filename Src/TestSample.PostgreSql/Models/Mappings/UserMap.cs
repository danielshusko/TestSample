using Microsoft.EntityFrameworkCore;

namespace TestSample.PostgreSql.Models.Mappings;

public static class UserMap
{
    public static void ConfigureEntity(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserDataModel>(
            builder =>
            {
                builder.ToTable("User");
                builder.HasKey(x => x.Id);
                builder.HasIndex(x => new { x.FirstName, x.LastName })
                       .IsUnique();
                builder.Property(x => x.Id)
                       .UseIdentityAlwaysColumn();
                builder.Property(x => x.FirstName)
                       .IsRequired()
                       .HasMaxLength(25);
                builder.Property(x => x.LastName)
                       .IsRequired()
                       .HasMaxLength(25);
            });
}