﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebSisMed.Models.Entities;

namespace WebSisMed.Models.EntityConfigurations
{
    public class MedicoConfiguration : IEntityTypeConfiguration<Medico>
    {
        public void Configure(EntityTypeBuilder<Medico> builder)
        {
            builder.ToTable("Medicos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.CRM)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasIndex(x => x.CRM)
                .IsUnique();
        }
    }
}
