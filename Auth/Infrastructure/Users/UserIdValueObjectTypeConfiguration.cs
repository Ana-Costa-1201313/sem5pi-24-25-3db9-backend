using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Auth.Domain.Users;

namespace Auth.Infrastructure.Users
{
    internal class UserNameValueObjectTypeConfiguration : IEntityTypeConfiguration<UserDb>
    {
        public void Configure(EntityTypeBuilder<UserDb> builder)
        {
            //builder.ToTable("UserNames",SchemaNames.Auth);
            builder.Property<int>("Id").IsRequired();//Id is a shadow property
            builder.HasKey("Id"); //Id is a shadow property
        }
    }
}
