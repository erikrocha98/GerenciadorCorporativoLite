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
    public class EmpresaFornecedorConfiguration : IEntityTypeConfiguration<EmpresaFornecedor>
    {
        public void Configure(EntityTypeBuilder<EmpresaFornecedor> builder)
        {
            builder.ToTable("EmpresaFornecedores");

            // Chave composta
            builder.HasKey(ef => new { ef.EmpresaId, ef.FornecedorId });

            // Relacionamento com Empresa
            builder.HasOne(ef => ef.Empresa)
                .WithMany(e => e.EmpresaFornecedores)
                .HasForeignKey(ef => ef.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Fornecedor
            builder.HasOne(ef => ef.Fornecedor)
                .WithMany(f => f.EmpresaFornecedores)
                .HasForeignKey(ef => ef.FornecedorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(ef => ef.DataVinculo)
                .IsRequired();
        }
    }
}
