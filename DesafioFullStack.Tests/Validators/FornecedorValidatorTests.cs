using DesafioFullStack.Domain.Entities;
using DesafioFullStack.Domain.Validators;
using FluentAssertions;

namespace DesafioFullStack.Tests.Validators;

public class FornecedorValidatorTests
{
    private readonly FornecedorValidator _validator = new();

    private static Fornecedor FornecedorPJValido() => new()
    {
        CpfCnpj = "11222333000181",
        Nome = "Empresa XYZ Ltda",
        Email = "contato@xyz.com",
        Cep = "12345678"
    };

    private static Fornecedor FornecedorPFValido() => new()
    {
        CpfCnpj = "52998224725",
        Nome = "João Silva",
        Email = "joao@teste.com",
        Cep = "12345678",
        Rg = "1234567",
        DataNascimento = DateTime.Today.AddYears(-25)
    };

    // ── Casos válidos ─────────────────────────────────────────────────────────

    [Fact]
    public void Validar_FornecedorPJValido_SemErros()
    {
        var resultado = _validator.Validate(FornecedorPJValido());

        resultado.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validar_FornecedorPFValido_SemErros()
    {
        var resultado = _validator.Validate(FornecedorPFValido());

        resultado.IsValid.Should().BeTrue();
    }

    // ── CPF / CNPJ ────────────────────────────────────────────────────────────

    [Fact]
    public void Validar_CpfCnpjVazio_RetornaErro()
    {
        var fornecedor = FornecedorPJValido();
        fornecedor.CpfCnpj = "";

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(Fornecedor.CpfCnpj));
    }

    [Fact]
    public void Validar_CpfInvalido_RetornaErro()
    {
        var fornecedor = FornecedorPFValido();
        fornecedor.CpfCnpj = "52998224726"; // dígito verificador errado

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.CpfCnpj) &&
            e.ErrorMessage == "CPF/CNPJ inválido");
    }

    [Fact]
    public void Validar_CnpjInvalido_RetornaErro()
    {
        var fornecedor = FornecedorPJValido();
        fornecedor.CpfCnpj = "11222333000182"; // dígito verificador errado

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.CpfCnpj) &&
            e.ErrorMessage == "CPF/CNPJ inválido");
    }

    // ── Nome ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Validar_NomeVazio_RetornaErro()
    {
        var fornecedor = FornecedorPJValido();
        fornecedor.Nome = "";

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.Nome) &&
            e.ErrorMessage == "Nome é obrigatório");
    }

    [Fact]
    public void Validar_NomeComMaisDe200Caracteres_RetornaErro()
    {
        var fornecedor = FornecedorPJValido();
        fornecedor.Nome = new string('A', 201);

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.Nome) &&
            e.ErrorMessage == "Nome deve ter no máximo 200 caracteres");
    }

    // ── E-mail ────────────────────────────────────────────────────────────────

    [Fact]
    public void Validar_EmailVazio_RetornaErro()
    {
        var fornecedor = FornecedorPJValido();
        fornecedor.Email = "";

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.Email) &&
            e.ErrorMessage == "E-mail é obrigatório");
    }

    [Fact]
    public void Validar_EmailInvalido_RetornaErro()
    {
        var fornecedor = FornecedorPJValido();
        fornecedor.Email = "email-sem-arroba";

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.Email) &&
            e.ErrorMessage == "E-mail inválido");
    }

    // ── CEP ───────────────────────────────────────────────────────────────────

    [Fact]
    public void Validar_CepVazio_RetornaErro()
    {
        var fornecedor = FornecedorPJValido();
        fornecedor.Cep = "";

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.Cep) &&
            e.ErrorMessage == "CEP é obrigatório");
    }

    [Fact]
    public void Validar_CepComMenosDeOitoDigitos_RetornaErro()
    {
        var fornecedor = FornecedorPJValido();
        fornecedor.Cep = "1234567"; // 7 dígitos

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(Fornecedor.Cep));
    }

    // ── Campos exclusivos de Pessoa Física ───────────────────────────────────

    [Fact]
    public void Validar_PFSemRg_RetornaErro()
    {
        var fornecedor = FornecedorPFValido();
        fornecedor.Rg = null;

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.Rg) &&
            e.ErrorMessage == "RG é obrigatório para pessoa física");
    }

    [Fact]
    public void Validar_PFSemDataNascimento_RetornaErro()
    {
        var fornecedor = FornecedorPFValido();
        fornecedor.DataNascimento = null;

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.DataNascimento) &&
            e.ErrorMessage == "Data de nascimento é obrigatória para pessoa física");
    }

    [Fact]
    public void Validar_PFComDataNascimentoFutura_RetornaErro()
    {
        var fornecedor = FornecedorPFValido();
        fornecedor.DataNascimento = DateTime.Now.AddDays(1);

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Fornecedor.DataNascimento) &&
            e.ErrorMessage == "Data de nascimento deve ser no passado");
    }

    [Fact]
    public void Validar_PJSemRgESemDataNascimento_SemErros()
    {
        // Campos de PF não devem ser exigidos para PJ
        var fornecedor = FornecedorPJValido();
        fornecedor.Rg = null;
        fornecedor.DataNascimento = null;

        var resultado = _validator.Validate(fornecedor);

        resultado.IsValid.Should().BeTrue();
    }
}
