using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Auth.Domain.Users;

namespace Auth.Infrastructure.Users
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(x => x.Id);

            builder.OwnsOne(x => x.username, username =>
           {
               username.Property(y => y.username).IsRequired();
           });

            // builder.OwnsOne(x => x.nomeCompleto, nomeCompleto =>
            // {
            //     nomeCompleto.Property(y => y.nomeCompleto).IsRequired();
            // });
            // 
            // builder.OwnsOne(x => x.email, email =>
            // {
            //     email.Property(y => y.email).IsRequired();
            // });
            // 
            // builder.OwnsOne(x => x.telemovel, telemovel =>
            // {
            //     telemovel.Property(y => y.telemovel).IsRequired();
            // });
            // builder.Property(x => x.permissoes);
            builder.ToTable("User");
        }
    }
}
