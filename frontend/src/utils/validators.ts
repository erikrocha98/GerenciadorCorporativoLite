/**
 * Remove caracteres não numéricos de uma string
 */
export function apenasNumeros(valor: string): string {
    return valor.replace(/\D/g, '');
}

/**
 * Valida se o CPF é válido
 */
export function validarCPF(cpf: string): boolean {
    cpf = apenasNumeros(cpf);

    if (cpf.length !== 11) return false;

    if (/^(\d)\1{10}$/.test(cpf)) return false; //todos os dígitos iguais
    var soma = 0;
    var resto = 0;

    for (let i = 1; i <= 9; i++) {
        soma += parseInt(cpf.substring(i - 1, i)) * (11 - i)
    }

    resto = (soma * 10) % 11
    if (resto === 10 || resto === 11) resto = 0
    if (resto !== parseInt(cpf.substring(9, 10))) return false

    soma = 0
    for (let i = 1; i <= 10; i++) {
        soma += parseInt(cpf.substring(i - 1, i)) * (12 - i)
    }

    resto = (soma * 10) % 11
    if (resto === 10 || resto === 11) resto = 0
    if (resto !== parseInt(cpf.substring(10, 11))) return false

    return true;

}

/**
 * Valida CNPJ
 */
export function validarCnpj(cnpj: string): boolean {
    cnpj = apenasNumeros(cnpj);

    if (cnpj.length !== 14) return false
    if (/^(\d)\1{13}$/.test(cnpj)) return false // Todos dígitos iguais

    var tamanho = cnpj.length - 2;
    var numeros = cnpj.substring(0, tamanho);
    const digitos = cnpj.substring(tamanho);
    var soma = 0;
    var pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
        soma += parseInt(numeros.charAt(tamanho - i)) * pos--
        if (pos < 2) pos = 9
    }

    var resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11);
    if (resultado !== parseInt(digitos.charAt(0))) return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;

    for (let i = tamanho; i >= 1; i--) {
        soma += parseInt(numeros.charAt(tamanho - i)) * pos--;
        if (pos < 2) pos = 9;
    }

    resultado = soma % 11 < 2 ? 0 : 11 - (soma % 11)
    if (resultado !== parseInt(digitos.charAt(1))) return false;

    return true;
}

/**
 * Valida formato de CEP
 */
export function validarCEP(cep: string): boolean {
    cep = apenasNumeros(cep);
    return cep.length === 8;
}

/**
 * Valida e-mail
 */
export function validarEmail(email: string): boolean {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

/**
 * Formata CPF: 000.000.000-00
 */
export function formatarCPF(cpf: string): string {
    cpf = apenasNumeros(cpf);
    return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
}

/**
 * Formata CNPJ: 00.000.000/0000-00
 */
export function formatarCNPJ(cnpj: string): string {
    cnpj = apenasNumeros(cnpj);
    return cnpj.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
}

/**
 * Formata CEP: 00000-000
 */
export function formatarCEP(cep: string): string {
    cep = apenasNumeros(cep);
    return cep.replace(/(\d{5})(\d{3})/, '$1-$2');
}

/**
 * Calcula idade a partir da data de nascimento
 */
export function calcularIdade(dataNascimento: string): number {
    const hoje = new Date();
    const nascimento = new Date(dataNascimento);
    var idade = hoje.getFullYear() - nascimento.getFullYear();
    const mes = hoje.getMonth() - nascimento.getMonth();
    if (mes < 0 || (mes === 0 && hoje.getDate() < nascimento.getDate())) {
        idade--
    }

    return idade;
}

/**
 * Verifica se é menor de idade (< 18 anos)
 */

export function ehMenordeIdade(dataNascimento: string) : boolean {
    return calcularIdade(dataNascimento) < 18;
}