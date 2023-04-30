using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSisMed.Models.Entities;

namespace WebSisMed.Models.EntityConfigurations
{
    public class PacienteConfiguration : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> builder)
        {
            builder.ToTable("Pacientes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(x => x.CPF)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.HasIndex(x => x.CPF)
                   .IsUnique();

            builder.Property(x => x.DataNascimento)
                   .IsRequired();

            builder.HasOne(p => p.InformacoesComplementares)
                   .WithOne(i => i.Paciente)
                   .HasForeignKey<InformacoesComplementaresPaciente>(i => i.IdPaciente);

            builder.HasMany(p => p.Monitoramentos)
                   .WithOne(m => m.Paciente)
                   .HasForeignKey(m => m.IdPaciente);
        }
    }
}
