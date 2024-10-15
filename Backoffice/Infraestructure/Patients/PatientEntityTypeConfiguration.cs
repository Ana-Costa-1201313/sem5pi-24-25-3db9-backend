using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Backoffice.Domain.Patients;

namespace Backoffice.Infraestructure.Patients
{
    internal class PatientEntityTypeConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patient", SchemaNames.Backoffice);
            builder.HasKey(b => b.Id);
        }
    }
}