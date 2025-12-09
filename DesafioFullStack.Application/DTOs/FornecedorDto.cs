using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFullStack.Application.DTOs
{
    public class FornecedorDto
    {
        public Guid Id { get; set; }
        public string CpfCnpj { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool EhPessoaFisica { get; set; }
        public DateTime DataCadastro { get; set; }
    }

    public class CreateFornecedorDto 
    {
        public string CpfCnpj { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
    }

    public class UpdateFornecedorDto 
    {
        public string CpfCnpj { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string? Rg { get; set; }
        public DateTime? DataNascimento { get; set; }
    }
}
