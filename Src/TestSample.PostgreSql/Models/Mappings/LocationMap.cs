using Microsoft.EntityFrameworkCore;

namespace TestSample.PostgreSql.Models.Mappings;

public class LocationMap
{
    public void ConfigureEntity(ModelBuilder modelBuilder) =>
        modelBuilder.Entity<UserDataModel>(
            builder =>
            {
                builder.ToTable("Location");
                builder.HasKey(x => x.Id);
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