using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Auth.API.Entities;

namespace Auth.API.EntityConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable(nameof(AppUser));
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("uniqueidentifier");
            builder.Property(u => u.FirstName).IsRequired(false).HasColumnType("nvarchar(256)");
            builder.Property(u => u.LastName).IsRequired(false).HasColumnType("nvarchar(256)");
        }
    }
}
