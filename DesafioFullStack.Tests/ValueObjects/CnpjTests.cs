using DesafioFullStack.Domain.ValueObjects;
using FluentAssertions;

namespace DesafioFullStack.Tests.ValueObjects;

public class CnpjTests
{
    [Theory]
    [InlineData("11222333000181")]
    [InlineData("45997418000153")]
    public void Validar_ComCnpjValido_RetornaTrue(string cnpj)
    {
        Cnpj.Validar(cnpj).Should().BeTrue();
    }

    [Fact]
    public void Validar_ComCnpjFormatado_RetornaTrue()
    {
        Cnpj.Validar("11.222.333/0001-81").Should().BeTrue();
    }

    [Theory]
    [InlineData("11222333000182")] // dígito verificador errado
    [InlineData("12345678000100")] // CNPJ inválido
    public void Validar_ComCnpjInvalido_RetornaFalse(string cnpj)
    {
        Cnpj.Validar(cnpj).Should().BeFalse();
    }

    [Theory]
    [InlineData("00000000000000")]
    [InlineData("11111111111111")]
    [InlineData("99999999999999")]
    public void Validar_ComTodosDigitosIguais_RetornaFalse(string cnpj)
    {
        Cnpj.Validar(cnpj).Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validar_ComStringVaziaOuEspacos_RetornaFalse(string cnpj)
    {
        Cnpj.Validar(cnpj).Should().BeFalse();
    }

    [Fact]
    public void Validar_ComMenosDeCatorzeDigitos_RetornaFalse()
    {
        Cnpj.Validar("1122233300018").Should().BeFalse();
    }

    [Fact]
    public void Validar_ComMaisDeCatorzeDigitos_RetornaFalse()
    {
        Cnpj.Validar("112223330001810").Should().BeFalse();
    }

    [Fact]
    public void Construtor_ComCnpjValido_CriaObjetoComValorSemFormatacao()
    {
        var cnpj = new Cnpj("11.222.333/0001-81");

        cnpj.Valor.Should().Be("11222333000181");
    }

    [Fact]
    public void Construtor_ComCnpjInvalido_LancaArgumentException()
    {
        var act = () => new Cnpj("12345678000100");

        act.Should().Throw<ArgumentException>().WithMessage("CNPJ inválido");
    }
}
