using DesafioFullStack.Domain.ValueObjects;
using FluentAssertions;

namespace DesafioFullStack.Tests.ValueObjects;

public class CpfTests
{
    [Theory]
    [InlineData("52998224725")]
    [InlineData("11144477735")]
    public void Validar_ComCpfValido_RetornaTrue(string cpf)
    {
        Cpf.Validar(cpf).Should().BeTrue();
    }

    [Fact]
    public void Validar_ComCpfFormatado_RetornaTrue()
    {
        Cpf.Validar("529.982.247-25").Should().BeTrue();
    }

    [Theory]
    [InlineData("52998224726")] // dígito verificador errado
    [InlineData("12345678901")] // CPF inválido
    public void Validar_ComCpfInvalido_RetornaFalse(string cpf)
    {
        Cpf.Validar(cpf).Should().BeFalse();
    }

    [Theory]
    [InlineData("00000000000")]
    [InlineData("11111111111")]
    [InlineData("99999999999")]
    public void Validar_ComTodosDigitosIguais_RetornaFalse(string cpf)
    {
        Cpf.Validar(cpf).Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validar_ComStringVaziaOuEspacos_RetornaFalse(string cpf)
    {
        Cpf.Validar(cpf).Should().BeFalse();
    }

    [Fact]
    public void Validar_ComMenosDeOnzeDigitos_RetornaFalse()
    {
        Cpf.Validar("1234567890").Should().BeFalse();
    }

    [Fact]
    public void Validar_ComMaisDeOnzeDigitos_RetornaFalse()
    {
        Cpf.Validar("529982247250").Should().BeFalse();
    }

    [Fact]
    public void Construtor_ComCpfValido_CriaObjetoComValorSemFormatacao()
    {
        var cpf = new Cpf("529.982.247-25");

        cpf.Valor.Should().Be("52998224725");
    }

    [Fact]
    public void Construtor_ComCpfInvalido_LancaArgumentException()
    {
        var act = () => new Cpf("12345678901");

        act.Should().Throw<ArgumentException>().WithMessage("CPF inválido");
    }
}
