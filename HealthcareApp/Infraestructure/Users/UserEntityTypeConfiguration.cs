using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HealthcareApp.Domain.Users;

namespace HealthcareApp.Infraestructure.Users
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", SchemaNames.HealthcareApp);
            builder.HasKey(b => b.Id);
        }
    }
}