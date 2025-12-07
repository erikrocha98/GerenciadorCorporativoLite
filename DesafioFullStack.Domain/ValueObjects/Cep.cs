using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioFullStack.Domain.ValueObjects
{
    public class Cep
    {
        public string Valor { get; private set; }

        public Cep(string valor)
        {
            var cepLimpo = LimparFormatacao(valor);

            if (!Validar(cepLimpo))
                throw new ArgumentException("CEP inválido");

            Valor = cepLimpo;
        }

        public static bool Validar(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return false;

            cep = LimparFormatacao(cep);

            return cep.Length == 8 && cep.All(char.IsDigit);
        }

        private static string LimparFormatacao(string valor)
        {
            return new string(valor.Where(char.IsDigit).ToArray());
        }

        public string Formatar()
        {
            return $"{Valor.Substring(0, 5)}-{Valor.Substring(5, 3)}";
        }

        public override string ToString() => Valor;
    }
}
