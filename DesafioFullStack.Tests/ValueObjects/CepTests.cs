using DesafioFullStack.Domain.ValueObjects;
using FluentAssertions;

namespace DesafioFullStack.Tests.ValueObjects;

public class CepTests
{
    [Theory]
    [InlineData("12345678")]
    [InlineData("80010000")]
    [InlineData("01310100")]
    public void Validar_ComCepValido_RetornaTrue(string cep)
    {
        Cep.Validar(cep).Should().BeTrue();
    }

    [Fact]
    public void Validar_ComCepFormatado_RetornaTrue()
    {
        Cep.Validar("80010-000").Should().BeTrue();
    }

    [Theory]
    [InlineData("1234567")]   // 7 dígitos
    [InlineData("123456789")] // 9 dígitos
    public void Validar_ComQuantidadeDeDigitosErrada_RetornaFalse(string cep)
    {
        Cep.Validar(cep).Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validar_ComStringVaziaOuEspacos_RetornaFalse(string cep)
    {
        Cep.Validar(cep).Should().BeFalse();
    }

    [Fact]
    public void Construtor_ComCepValido_CriaObjetoComValorSemFormatacao()
    {
        var cep = new Cep("80010-000");

        cep.Valor.Should().Be("80010000");
    }

    [Fact]
    public void Construtor_ComCepInvalido_LancaArgumentException()
    {
        var act = () => new Cep("1234567");

        act.Should().Throw<ArgumentException>().WithMessage("CEP inválido");
    }

    [Fact]
    public void Formatar_RetornaCepNoFormatoCorreto()
    {
        var cep = new Cep("80010000");

        cep.Formatar().Should().Be("80010-000");
    }
}
