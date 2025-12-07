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
    public class FornecedorConfiguration : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.ToTable("Fornecedores");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.CpfCnpj)
                .IsRequired()
                .HasMaxLength(14);

            builder.HasIndex(f => f.CpfCnpj)
                .IsUnique();

            builder.Property(f => f.Nome)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(f => f.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(f => f.Cep)
                .IsRequired()
                .HasMaxLength(8);

            builder.Property(f => f.Rg)
                .HasMaxLength(20);

            builder.Property(f => f.DataNascimento)
                .IsRequired(false);

            builder.Property(f => f.DataCadastro)
                .IsRequired();

            builder.Property(f => f.DataAtualizacao)
                .IsRequired(false);

            // Ignorar propriedade computed
            builder.Ignore(f => f.EhPessoaFisica);
        }
    }
}
