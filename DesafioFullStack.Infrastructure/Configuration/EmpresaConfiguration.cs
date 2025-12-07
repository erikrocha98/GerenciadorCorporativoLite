using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesafioFullStack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DesafioFullStack.Infrastructure.Configuration
{
    public class EmpresaConfiguration : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(EntityTypeBuilder<Empresa> builder)
        {
            builder.ToTable("Empresas");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Cnpj)
                .IsRequired()
                .HasMaxLength(14);

            builder.HasIndex(e => e.Cnpj)
                .IsUnique();

            builder.Property(e => e.NomeFantasia)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Cep)
                .IsRequired()
                .HasMaxLength(8);

            builder.Property(e => e.DataCadastro)
                .IsRequired();

            builder.Property(e => e.DataAtualizacao)
                .IsRequired(false);
        }
    }

}
