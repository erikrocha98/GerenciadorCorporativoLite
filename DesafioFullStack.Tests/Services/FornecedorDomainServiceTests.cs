using DesafioFullStack.Domain.Entities;
using DesafioFullStack.Domain.Services;
using FluentAssertions;

namespace DesafioFullStack.Tests.Services;

public class FornecedorDomainServiceTests
{
    private readonly FornecedorDomainService _service = new();

    // CPF de 11 dígitos → EhPessoaFisica = true
    private const string CpfValido = "52998224725";
    // CNPJ de 14 dígitos → EhPessoaFisica = false
    private const string CnpjValido = "11222333000181";

    private static Fornecedor CriarFornecedorPF(DateTime dataNascimento) => new()
    {
        CpfCnpj = CpfValido,
        Nome = "João Silva",
        Email = "joao@teste.com",
        Cep = "12345678",
        Rg = "1234567",
        DataNascimento = dataNascimento
    };

    private static Fornecedor CriarFornecedorPJ() => new()
    {
        CpfCnpj = CnpjValido,
        Nome = "Empresa XYZ",
        Email = "contato@xyz.com",
        Cep = "12345678"
    };

    // ── Regra principal ──────────────────────────────────────────────────────

    [Fact]
    public void ValidarRegraParana_PFMenorDeIdade_CepParana_RetornaFalse()
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-17));

        var resultado = _service.ValidarRegraParana(fornecedor, "80000000");

        resultado.Should().BeFalse();
    }

    [Fact]
    public void ValidarRegraParana_PFMaiorDeIdade_CepParana_RetornaTrue()
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-25));

        var resultado = _service.ValidarRegraParana(fornecedor, "80000000");

        resultado.Should().BeTrue();
    }

    [Fact]
    public void ValidarRegraParana_PFComExatamente18Anos_CepParana_RetornaTrue()
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-18));

        var resultado = _service.ValidarRegraParana(fornecedor, "80000000");

        resultado.Should().BeTrue();
    }

    [Fact]
    public void ValidarRegraParana_PFQuaseCompleta18Anos_CepParana_RetornaFalse()
    {
        // Faz 18 amanhã → ainda menor de idade hoje
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-18).AddDays(1));

        var resultado = _service.ValidarRegraParana(fornecedor, "80000000");

        resultado.Should().BeFalse();
    }

    [Fact]
    public void ValidarRegraParana_PFMenorDeIdade_CepForaDoParana_RetornaTrue()
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-17));

        var resultado = _service.ValidarRegraParana(fornecedor, "01310100"); // São Paulo

        resultado.Should().BeTrue();
    }

    [Fact]
    public void ValidarRegraParana_PJ_CepParana_RetornaTrue()
    {
        var fornecedor = CriarFornecedorPJ();

        var resultado = _service.ValidarRegraParana(fornecedor, "80000000");

        resultado.Should().BeTrue();
    }

    [Fact]
    public void ValidarRegraParana_PFSemDataNascimento_CepParana_RetornaFalse()
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-25));
        fornecedor.DataNascimento = null;

        var resultado = _service.ValidarRegraParana(fornecedor, "80000000");

        resultado.Should().BeFalse();
    }

    // ── Limites dos prefixos do Paraná (80 a 87) ────────────────────────────

    [Theory]
    [InlineData("80000000")] // limite inferior
    [InlineData("83500000")] // meio da faixa
    [InlineData("87999999")] // limite superior
    public void ValidarRegraParana_PFMenor_CepsDoParana_RetornaFalse(string cepEmpresa)
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-17));

        var resultado = _service.ValidarRegraParana(fornecedor, cepEmpresa);

        resultado.Should().BeFalse();
    }

    [Theory]
    [InlineData("79999999")] // imediatamente antes do Paraná
    [InlineData("88000000")] // imediatamente depois do Paraná
    [InlineData("01310100")] // São Paulo
    [InlineData("20040020")] // Rio de Janeiro
    public void ValidarRegraParana_PFMenor_CepsForaDoParana_RetornaTrue(string cepEmpresa)
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-17));

        var resultado = _service.ValidarRegraParana(fornecedor, cepEmpresa);

        resultado.Should().BeTrue();
    }

    // ── CEP formatado ────────────────────────────────────────────────────────

    [Fact]
    public void ValidarRegraParana_CepComMascara_AplicaRegraCorretamente()
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-17));

        // CEP do Paraná com formatação → deve bloquear
        var resultado = _service.ValidarRegraParana(fornecedor, "80010-000");

        resultado.Should().BeFalse();
    }

    // ── CEP inválido ─────────────────────────────────────────────────────────

    [Fact]
    public void ValidarRegraParana_CepComMenosDeOitoDigitos_NaoAplicaRestricao()
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-17));

        var resultado = _service.ValidarRegraParana(fornecedor, "8001");

        resultado.Should().BeTrue();
    }

    // ── Método async ─────────────────────────────────────────────────────────

    [Fact]
    public async Task PodeVincularFornecedorAsync_PFMenorDeIdade_CepParana_RetornaFalse()
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-17));

        var resultado = await _service.PodeVincularFornecedorAsync(fornecedor, "80000000");

        resultado.Should().BeFalse();
    }

    [Fact]
    public async Task PodeVincularFornecedorAsync_PFMaiorDeIdade_CepParana_RetornaTrue()
    {
        var fornecedor = CriarFornecedorPF(DateTime.Today.AddYears(-25));

        var resultado = await _service.PodeVincularFornecedorAsync(fornecedor, "80000000");

        resultado.Should().BeTrue();
    }
}
