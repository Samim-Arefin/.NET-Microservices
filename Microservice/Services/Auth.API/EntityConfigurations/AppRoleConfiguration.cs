using Auth.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.API.EntityConfigurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable(nameof(AppRole));
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasColumnType("uniqueidentifier");
        }
    }
}
