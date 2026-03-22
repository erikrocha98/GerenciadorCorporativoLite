using DesafioFullStack.Domain.Entities;
using DesafioFullStack.Domain.Validators;
using FluentAssertions;

namespace DesafioFullStack.Tests.Validators;

public class EmpresaValidatorTests
{
    private readonly EmpresaValidator _validator = new();

    private static Empresa EmpresaValida() => new()
    {
        Cnpj = "11222333000181",
        NomeFantasia = "Empresa Teste",
        Cep = "12345678"
    };

    [Fact]
    public void Validar_EmpresaValida_SemErros()
    {
        var resultado = _validator.Validate(EmpresaValida());

        resultado.IsValid.Should().BeTrue();
    }

    // ── CNPJ ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Validar_CnpjVazio_RetornaErro()
    {
        var empresa = EmpresaValida();
        empresa.Cnpj = "";

        var resultado = _validator.Validate(empresa);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(Empresa.Cnpj));
    }

    [Fact]
    public void Validar_CnpjInvalido_RetornaErro()
    {
        var empresa = EmpresaValida();
        empresa.Cnpj = "11222333000182"; // dígito verificador errado

        var resultado = _validator.Validate(empresa);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Empresa.Cnpj) &&
            e.ErrorMessage == "O CNPJ informado é inválido.");
    }

    [Fact]
    public void Validar_CnpjComMenosDe14Caracteres_RetornaErro()
    {
        var empresa = EmpresaValida();
        empresa.Cnpj = "1122233300018"; // 13 dígitos

        var resultado = _validator.Validate(empresa);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(Empresa.Cnpj));
    }

    // ── NomeFantasia ──────────────────────────────────────────────────────────

    [Fact]
    public void Validar_NomeFantasiaVazio_RetornaErro()
    {
        var empresa = EmpresaValida();
        empresa.NomeFantasia = "";

        var resultado = _validator.Validate(empresa);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Empresa.NomeFantasia) &&
            e.ErrorMessage == "O nome fantasia é obrigatório.");
    }

    [Fact]
    public void Validar_NomeFantasiaComMaisDe200Caracteres_RetornaErro()
    {
        var empresa = EmpresaValida();
        empresa.NomeFantasia = new string('A', 201);

        var resultado = _validator.Validate(empresa);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Empresa.NomeFantasia) &&
            e.ErrorMessage == "O nome fantasia deve conter no máximo 200 caracteres.");
    }

    // ── CEP ───────────────────────────────────────────────────────────────────

    [Fact]
    public void Validar_CepVazio_RetornaErro()
    {
        var empresa = EmpresaValida();
        empresa.Cep = "";

        var resultado = _validator.Validate(empresa);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e =>
            e.PropertyName == nameof(Empresa.Cep) &&
            e.ErrorMessage == "O CEP é obrigatório.");
    }

    [Fact]
    public void Validar_CepComMenosDeOitoCaracteres_RetornaErro()
    {
        var empresa = EmpresaValida();
        empresa.Cep = "1234567"; // 7 dígitos

        var resultado = _validator.Validate(empresa);

        resultado.IsValid.Should().BeFalse();
        resultado.Errors.Should().Contain(e => e.PropertyName == nameof(Empresa.Cep));
    }
}
